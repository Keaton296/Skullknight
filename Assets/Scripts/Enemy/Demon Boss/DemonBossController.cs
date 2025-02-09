using UnityEngine;
using DG.Tweening;
using System.Collections;
using Cinemachine;
using Skullknight.Core;
using Skullknight.Enemy.Demon_Boss;
using Skullknight.Player.Statemachine;
using Skullknight.State;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class DemonBossController : StateManager<EDemonBossState>,IDamageable
{
    public enum DemonBossPhase
    {
        FirstPhase,
        SecondPhase
    }

    private DemonBossPhase _phase;
    public DemonBossPhase Phase => _phase;
    
    public SpriteRenderer spriteRenderer;
    public Animator animator;

    [SerializeField] private Animator fireAnimator;
    [SerializeField] private GameObject fireballPrefab;
    
    
    public Tween takingDamageTween;
    public Tween movementTween;
    
    public bool canTurn = true;
    public bool canTakeDamage = false;

    public Transform IdlingPoint;
    public Transform FireBreathTransform;
    public Transform mouthPoint;

    [SerializeField] private float movementDuration = 1f;
    
    [SerializeField] int health = 300;
    public int Health {
        get {
            return health;
        }
        set {
            if (value > health) { health = value; }
            else if (value < health)
            {
                health = value;
                spriteRenderer.material.SetFloat("_HitFXPercent", 1f);
                if( takingDamageTween != null)
                {
                    takingDamageTween.Kill();
                    takingDamageTween = null;
                }
                takingDamageTween = DOTween.To(() => spriteRenderer.material.GetFloat("_HitFXPercent"), x => spriteRenderer.material.SetFloat("_HitFXPercent",x),0f,.3f);
            }
            BossBar.Instance?.statBar.OnValueChange(value);
        }
    }

    public UnityEvent OnHealthChanged { get; set; }

    protected override void Awake()
    {
        states.Add(EDemonBossState.Idle,new DemonIdleState(this));
        states.Add(EDemonBossState.BreathAttack,new DemonBreathAttackState(this));
        states.Add(EDemonBossState.FireballAttack,new DemonBossFireballAttackState(this));
        GameManager.Instance?.OnStateChange.AddListener(OnGameStateChanged);
    }

    protected override void Start()
    {
        ChangeState(EDemonBossState.Idle);
    }

    private void OnGameStateChanged(GameManager.EGameManagerState newState)
    {
        if (newState == GameManager.EGameManagerState.Playing && GameManager.Instance.OnBossFight)
        {
            this.enabled = true;
        }
        else
        {
            this.enabled = false;
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }
    public void MoveToTransform(Vector3 ToPoint, float duration)
    {
        if(movementTween != null) movementTween.Kill();
        movementTween = transform.DOMove(ToPoint, duration, false);
    }

    public void MoveToIdlePosition()
    {
        MoveToTransform(IdlingPoint.position, 1f);
    }

    public void MoveToPlayerAttackPoint()
    {
        MoveToTransform(PlayerController.Instance.transform.position - FireBreathTransform.localPosition, 0.166f);
    }

    public void ShootBreath()
    {
        fireAnimator.Play("firebreath");
    }
    public void ShootFireball(Transform target)
    {
         Instantiate(fireballPrefab,mouthPoint.position , FireBreathTransform.rotation).GetComponent<Fireball>().SetFireballTarget(target);
    }

    public void ShootFireballToPlayer()
    {
        ShootFireball(PlayerController.Instance.transform);
    }

    public void LookPlayer()
    {
        if (PlayerController.Instance?.transform.position.x - transform.position.x > 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }

}

public enum EDemonBossState
{
    Idle,
    BreathAttack,
    FireballAttack,
    Transforming,
    IdleTwo
}

public abstract class DemonBossState : BaseState<EDemonBossState>
{
    protected DemonBossController controller;

    public DemonBossState(DemonBossController _controller)
    {
        controller = _controller;
    }
    public void SetFlip(bool value)
    {
        //attackHitbox.localScale = new Vector3(value != spriteRenderer.flipX ? -attackHitbox.localScale.x : attackHitbox.localScale.x,attackHitbox.localScale.y,attackHitbox.localScale.z);
        //landFXPlayerPoint.localPosition = new Vector3(value != spriteRenderer.flipX ? -landFXPlayerPoint.localPosition.x : landFXPlayerPoint.localPosition.x, landFXPlayerPoint.localPosition.y, landFXPlayerPoint.localPosition.z);
        //landFXRenderer.flipX = value;
        controller.spriteRenderer.flipX = value;
        //wallSlideParticleTransform.localPosition = new Vector3(value ? .539f : -.539f, wallSlideParticleTransform.localPosition.y);
    }
}
