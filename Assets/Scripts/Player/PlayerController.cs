using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour,IDamageable
{
    public static PlayerController Instance;
    public Rigidbody2D rb;
    public KeyCode rollKey = KeyCode.LeftShift;
    public Tween rollCooldownTween;
 
    IState playerState;
    public IState PlayerState {
        get { return playerState; } 
        set { playerState?.OnStateEnd();playerState = value; playerState?.OnStateStart(); } 
    }

    public int Health {
        get => throw new System.NotImplementedException();
        set => throw new System.NotImplementedException(); 
    }
    
    public BoxCollider2D ActiveBoxCollider2D 
    {
        get
        {
            return _activeBoxCollider2D;
        }
        set
        {
            if (_activeBoxCollider2D) _activeBoxCollider2D.enabled = false;
            _activeBoxCollider2D = value;
            if (_activeBoxCollider2D) _activeBoxCollider2D.enabled = true;
        }
    }
    [Header("Colliders")]
    BoxCollider2D _activeBoxCollider2D;
    public BoxCollider2D standingCollider;
    public BoxCollider2D crouchCollider;
    public BoxCollider2D wallCheckCollider;
    public BoxCollider2D holdCheckCollider;
    public BoxCollider2D standingCheckCollider;
    public Collider2D hangingObjectCollider;
    public BoxCollider2D atk0Collider;

    [Header("Primitive Properties")]
    public float collisionCheckHeight = .01f;
    public bool canRoll = true;
    public float rollingCooldown = 1f;
    public int atkDamage = 10;
    public float maxCrouchWalkingAnimSpeed = 2f;
    public Vector2 playerSpriteOffset;
    public bool onGround = true;
    public float jumpCheckDeathSeconds = .1f;
    public float wallSlideCheckDeathSeconds = .1f;
    public bool hasJumpCut = false;

    [Header("Movement Values")]
    public float groundSlideRequiredHorizontalSpeed;
    public float groundSlideStoppingSpeed;
    public float rollingVelocity = 10;
    public float maxRunningVelocity = 6.5f;
    public float maxCrouchingVelocity = 1f;
    public float maxRunningAnimSpeed = 2f;
    public float jumpVelocity = 5f;
    public float onAirStrafingAcceleration;
    public float onAirHorizontalSpeedLimit;
    public float maxFallSpeed;

    [Header("UI")]
    public Image rollCoolDownImage;

    [Header("Animators")]
    public Animator animator;
    public Animator landFXAnimator;

    [Header("Transforms")]
    public Transform attackHitbox;
    public Transform landFXPlayerPoint;
    public Transform spriteTransform;
    public Transform hangObjectTransform;
    public Transform hangObjectHangPointTransform;
    public Transform hangObjectClimbPointTransform;
    public Transform wallSlideParticleTransform;

    [Header("Physic Materials")]
    public PhysicsMaterial2D slidingPhysicMaterial;
    public PhysicsMaterial2D normalPhysicMaterial;
    public PhysicsMaterial2D stoppingPhysicMaterial;

    [Header("ParticleSystems")]
    public ParticleSystem wallSlideParticle;
    public ParticleSystem slideParticle;

    [Header("CollisionCheckers")]
    public CollisionChecker groundCollisionChecker;
    public CollisionChecker standUpCollisionChecker;

    [Header("Rigidbody2D's")]
    public Rigidbody2D groundCheckerRb;

    [Header("LayerMasks")]
    public LayerMask attackMask;
    public LayerMask landingMask;
    public LayerMask hangMask;
    public LayerMask wallMask;

    [Header("SpriteRenderers")]
    public SpriteRenderer landFXRenderer;
    public SpriteRenderer spriteRenderer;

    [Header("Animation Event Points")]
    public bool swing0;
    public bool swing0Ended;
    public bool swing1Triggered;
    public bool swing1;
    public bool swing1Ended;
    public bool climbAnimEnded;

    //COROUTINES
    public Coroutine currentActionCoroutine;
    public Coroutine jumpCheckDeathCoroutine;
    public Coroutine rollCoolDownCoroutine;
    public Coroutine wallSlideCheckDeathCoroutine;

    [Header("PlayerStates")]
    public PlayerStandingState playerStandingState;
    public PlayerAttackingState playerAttackingState;
    public PlayerOnAirState playerOnAirState;
    public PlayerHangingState playerHangingState;
    public PlayerCrouchingState playerCrouchingState;
    public PlayerWallSlideState playerWallSlideState;
    public PlayerSlidingState playerSlidingState;
    private void Awake()
    {
        Instance = this;
        playerState = playerStandingState;
        ActiveBoxCollider2D = standingCollider;
    }
    private void FixedUpdate()
    {

        playerState.StateFixedUpdate();

    }
    private void Update()
    {
        playerState.StateUpdate();
    }
    public void SetFlip(bool value)
    {
        attackHitbox.localScale = new Vector3(value != spriteRenderer.flipX ? -attackHitbox.localScale.x : attackHitbox.localScale.x,attackHitbox.localScale.y,attackHitbox.localScale.z);
        landFXPlayerPoint.localPosition = new Vector3(value != spriteRenderer.flipX ? -landFXPlayerPoint.localPosition.x : landFXPlayerPoint.localPosition.x, landFXPlayerPoint.localPosition.y, landFXPlayerPoint.localPosition.z);
        landFXRenderer.flipX = value;
        spriteRenderer.flipX = value;
        wallSlideParticleTransform.localPosition = new Vector3(value ? .539f : -.539f, wallSlideParticleTransform.localPosition.y);
    }
    public void PlayLandFX()
    {
        landFXAnimator.transform.position = landFXPlayerPoint.position;
        landFXAnimator.SetTrigger("impactdustkick");
    }
    public void Attack(Collider2D atkBox)
    {
        RaycastHit2D hit = Physics2D.BoxCast(atkBox.bounds.center, atkBox.bounds.size, 0f, spriteRenderer.flipX ? transform.right : -transform.right,collisionCheckHeight,attackMask);
        if (hit.transform != null)
        {
            hit.transform.GetComponent<IDamageable>().Health -= atkDamage;
            
        }
    }

    public IEnumerator RollCooldown()
    {
        canRoll = false;
        rollCoolDownImage.fillAmount = 0;
        rollCoolDownImage.enabled = true;
        rollCooldownTween = DOTween.To(() => rollCoolDownImage.fillAmount, x => rollCoolDownImage.fillAmount = x, 1f, rollingCooldown).OnComplete(() => { rollCoolDownImage.enabled = false; }).SetEase(Ease.Linear);
        yield return new WaitForSeconds(rollingCooldown);
        canRoll = true;
        rollCoolDownCoroutine = null;
    }

    public IEnumerator AttackZero()
    {
        
        rb.velocity = Vector2.zero;
        animator.SetTrigger("atk0");

        yield return new WaitUntil(() => swing0);

        Attack(atk0Collider);

        yield return new WaitUntil(() => swing0Ended);
        if (swing1Triggered)
        {
            currentActionCoroutine = StartCoroutine(AttackOne());
        }
        else
        {
            PlayerState = playerStandingState;
        }

    }
    public IEnumerator AttackOne()
    {
        rb.velocity = Vector2.zero;
        animator.SetTrigger("atk1");

        yield return new WaitUntil(() => swing1);

        Attack(atk0Collider);

        yield return new WaitUntil(() => swing1Ended);

        PlayerState = playerStandingState;
    }
    public IEnumerator Climb()
    {
        animator.SetTrigger("climb");

        yield return new WaitUntil(() => climbAnimEnded);

        climbAnimEnded = false;
        transform.localPosition = hangObjectClimbPointTransform.localPosition;
        spriteTransform.localPosition = playerSpriteOffset;
        transform.parent = null;
        rb.velocity = Vector2.zero;
        rb.isKinematic = false;
        currentActionCoroutine = null;
        animator.SetTrigger("idle");
        Physics2D.IgnoreCollision(standingCollider, hangingObjectCollider, false);
        Physics2D.IgnoreCollision(crouchCollider, hangingObjectCollider, false);
        PlayerState = playerStandingState;
    }
    public IEnumerator JumpCheckDeath()
    {
        yield return new WaitForSeconds(jumpCheckDeathSeconds);
        jumpCheckDeathCoroutine = null;
    }
    public IEnumerator WallSlideCheckDeath()
    {
        yield return new WaitForSeconds(wallSlideCheckDeathSeconds);
        wallSlideCheckDeathCoroutine = null;
    }

    public void Jump()
    {
        rb.AddForce(Vector2.up * jumpVelocity, ForceMode2D.Impulse);
        jumpCheckDeathCoroutine = StartCoroutine(JumpCheckDeath());

    }
    public IEnumerator Roll()
    {
        this.enabled = false;
        PlayLandFX();
        rollCoolDownCoroutine = StartCoroutine(RollCooldown());
        rb.velocity = Vector2.right * Input.GetAxisRaw("Horizontal") * rollingVelocity;
        animator.SetTrigger("roll");
        yield return new WaitForSeconds(.625f);
        enabled = true;
    }

    public void HorizontalMove(PlayerHorizontalMoveType movetype)
    {
        float maxVel = 1f;
        float axisAndSpeedDiff = 0f;
        switch (movetype)
        {
            case PlayerHorizontalMoveType.Run:
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Run")) animator.SetTrigger("run");
                maxVel = maxRunningVelocity;
                axisAndSpeedDiff = Input.GetAxis("Horizontal") * maxVel - rb.velocity.x;
                animator.SetFloat("runAnimSpeed", Mathf.Abs(Input.GetAxis("Horizontal") * maxRunningAnimSpeed));
                break;
            case PlayerHorizontalMoveType.Crouch:
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Crouchwalk")) animator.SetTrigger("crouchwalk");
                maxVel = maxCrouchingVelocity;
                axisAndSpeedDiff = Input.GetAxis("Horizontal") * maxVel - rb.velocity.x;
                animator.SetFloat("crouchWalkSpeed", Mathf.Abs(Input.GetAxis("Horizontal") * maxCrouchWalkingAnimSpeed));
                break;
        }
        ActiveBoxCollider2D.sharedMaterial = normalPhysicMaterial;
        rb.AddForce(Vector2.right * axisAndSpeedDiff, ForceMode2D.Impulse);
        if (Input.GetAxisRaw("Horizontal") == 1) SetFlip(false);
        else if (Input.GetAxisRaw("Horizontal") == -1) SetFlip(true);
        
    }

}
public enum PlayerHorizontalMoveType
{
    Run,
    Crouch
}

