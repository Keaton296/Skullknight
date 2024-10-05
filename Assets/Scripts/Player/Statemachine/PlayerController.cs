using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Player.Statemachine
{
    public class PlayerController : MonoBehaviour,IDamageable
    {
        public static PlayerController Instance;
        public Rigidbody2D rb;
        public Animator animator;
        public SpriteRenderer spriteRenderer;
        public PlayerInputActions inputSystem;
 
        PlayerState _playerState;
        public PlayerState PlayerState {
            get { return _playerState; }
            set
            {
                _playerState?.OnStateEnd();
                _playerState = value;
                _playerState?.OnStateStart(); 
                OnStateChange?.Invoke(value);
            } 
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
        public float Stamina
        {
            get
            {
                return m_stamina;
            }
            set
            {
                m_stamina = Mathf.Clamp(value, 0, m_maxStamina);
            }
        }
        public float MaxStamina => m_maxStamina;
        public float StaminaSpeed => m_staminaSpeed;
        public float SlidingSpeed => m_slidingSpeed;
        public float SlidingSpeedScale => m_SlidingSpeedScale;
        public float SlideStoppingSpeed => m_slideStoppingSpeed;
        public float FallingGravityScale => m_fallingGravityScale;
        public float SlidingSpeedCap => m_slidingSpeedCap;
        public float MaxFallingVelocity => m_maxFallingVelocity;

        public bool CanJump => (Time.time - lastJumpTime) > jumpCheckDeathSeconds;
    
        BoxCollider2D _activeBoxCollider2D;
        [HideInInspector] public BoxCollider2D standingCollider;
        [HideInInspector] public BoxCollider2D crouchCollider;
        [HideInInspector] public BoxCollider2D wallCheckCollider;
        [HideInInspector] public BoxCollider2D hangCheckCollider;
        [HideInInspector] public BoxCollider2D standingCheckCollider;
        [HideInInspector] public Collider2D hungObjectCollider;
        [HideInInspector] public BoxCollider2D atk0Collider;

        //Events
        [HideInInspector] public UnityEvent OnRoll;
        [HideInInspector] public UnityEvent OnLand;
        [HideInInspector] public UnityEvent OnCrouch;
        [HideInInspector] public UnityEvent OnSlide;
        [HideInInspector] public UnityEvent OnJump;
        [HideInInspector] public UnityEvent<PlayerState> OnStateChange;
    
        //Member values
        public float rollingCooldown = 1f;
        public IHangPoint hangPoint;
        private float m_stamina = 100;
        private float m_maxStamina = 100;
        private float m_staminaSpeed = 20;
        private float m_groundSlideDeathSeconds = .5f;
        private float jumpCheckDeathSeconds = .1f;
        [SerializeField] private float m_fallingGravityScale = 3f;
        private float m_slidingSpeed = 2f;
        private float m_slidingSpeedCap = 7f;
        private float m_SlidingSpeedScale = .5f;
        private float m_slideStoppingSpeed = .2f;
        [SerializeField] private float m_jumpCutPercentage = .25f;
        [SerializeField] private float m_maxFallingVelocity = 6f;
        [SerializeField] private float m_maxAirstrafingVelocity = 2f;
        [SerializeField] private float m_airstrafingAcceleration = 1f;
        public float rollingVelocity = 10;
        public float maxRunningVelocity = 6.5f;
        public float maxCrouchingVelocity = 1f;
        public float maxRunningAnimSpeed = 2f;
        public float jumpVelocity = 5f;

        [Header("Transforms")]
        public Transform attackHitbox;

        [Header("Physic Materials")]
        public PhysicsMaterial2D slidingPhysicMaterial;
        public PhysicsMaterial2D normalPhysicMaterial;
        public PhysicsMaterial2D stoppingPhysicMaterial;
    

        [Header("CollisionCheckers")]
        public CollisionChecker groundCollisionChecker;
        public CollisionChecker standUpCollisionChecker;
    

        [Header("LayerMasks")]
        public LayerMask attackMask;
        public LayerMask landingMask;
        public LayerMask hangMask;
        public LayerMask wallMask;
    

        //COROUTINES
        public Coroutine currentActionCoroutine;
        public float lastJumpTime; 
        public Coroutine groundSlideDeathCoroutine;
        public Coroutine rollCoolDownCoroutine;
        public Coroutine wallSlideCheckDeathCoroutine;
    
        //States 
        public PlayerIdleState IdleState;
        public PlayerRunningState RunningState;
        public PlayerAttackingState AttackingState;
        public PlayerFallingState FallingState;
        public PlayerHangingState HangingState;
        public PlayerCrouchingState CrouchState;
        public PlayerWallSlideState WallslideState;
        public PlayerSlidingState SlidingState;
        public PlayerRollState RollState;
        public PlayerJumpState JumpState;
        public PlayerClimbState ClimbState;
        private void Awake()
        {
            Instance = this;
            inputSystem = new PlayerInputActions();
            inputSystem?.Default.Enable();
        
            IdleState = new PlayerIdleState(this);
            RunningState = new PlayerRunningState(this);
            CrouchState = new PlayerCrouchingState(this);
            RollState = new PlayerRollState(this);
            SlidingState = new PlayerSlidingState(this);
            JumpState = new PlayerJumpState(this);
            FallingState = new PlayerFallingState(this);
        
            PlayerState = IdleState;
            ActiveBoxCollider2D = standingCollider;
        }
        private void Update()
        {
            _playerState.StateUpdate();
        }

        private void FixedUpdate()
        {
            _playerState.StateFixedUpdate();
        }
        public void Roll()
        {
            m_stamina -= 50;
            rb.velocity = Vector2.right * (Input.GetAxisRaw("Horizontal") * rollingVelocity);
            OnRoll?.Invoke();
        }
        public void Run(float inputAxis)
        {
            float ACCELERATION = 1f;
            float diffToMaxVel = inputAxis * maxRunningVelocity - rb.velocity.x;
            float fixedAcceleration = Mathf.Clamp(diffToMaxVel, -ACCELERATION, ACCELERATION);
            ActiveBoxCollider2D.sharedMaterial = normalPhysicMaterial;
            if (inputAxis == 1f) SetFlip(false);
            else if (inputAxis == -1) SetFlip(true);
            rb.AddForce(Vector2.right * fixedAcceleration,ForceMode2D.Impulse);
        }
        public void Airstrafe(float inputAxis)
        {
            float diffToMaxVel = inputAxis * m_maxAirstrafingVelocity - rb.velocity.x;
            float fixedAcceleration = Mathf.Clamp(diffToMaxVel, -m_airstrafingAcceleration, m_airstrafingAcceleration);
            ActiveBoxCollider2D.sharedMaterial = normalPhysicMaterial;
            if (inputAxis == 1f) SetFlip(false);
            else if (inputAxis == -1) SetFlip(true);
            rb.AddForce(Vector2.right * fixedAcceleration,ForceMode2D.Impulse);
        }

        public void Jump()
        {
            rb.AddForce(Vector2.up * jumpVelocity, ForceMode2D.Impulse);
            lastJumpTime = Time.time;
            OnJump?.Invoke();
        }
        public void OnJumpPerformed(InputAction.CallbackContext context)
        {
            if (CanJump)
            {
                PlayerState = JumpState;
            }
        }

        public void OnHoldPerformed(InputAction.CallbackContext context)
        {
            IHangPoint point = GetHangPoint();
            if (point != null)
            {
                PlayerState = HangingState;
            }
        }

        public void JumpCut()
        {
            rb.AddForce(Vector2.down * (rb.velocity.y * m_jumpCutPercentage),ForceMode2D.Impulse);
        }
        public void Crouchwalk(float inputAxis)
        {
            float ACCELERATION = .5f;
        
            float diffToMaxVel = inputAxis * maxCrouchingVelocity - rb.velocity.x;
            float fixedAcceleration = Mathf.Clamp(diffToMaxVel, -ACCELERATION, ACCELERATION);
            ActiveBoxCollider2D.sharedMaterial = normalPhysicMaterial;
            rb.AddForce(Vector2.right * fixedAcceleration,ForceMode2D.Impulse);
        
            if (inputAxis == 1f) SetFlip(false);
            else if (inputAxis == -1) SetFlip(true);
        }

        public IHangPoint GetHangPoint()
        {
            RaycastHit2D hitInfo;
            hitInfo = Physics2D.BoxCast(hangCheckCollider.bounds.center,
                hangCheckCollider.bounds.size,
                0f,
                rb.velocity,
                .1f);
            return hitInfo.collider.GetComponent<IHangPoint>();
        }

        public void Hang()
        {
            rb.isKinematic = true;
            Physics2D.IgnoreCollision(standingCollider, hangPoint.HandCollider, true);
            Physics2D.IgnoreCollision(crouchCollider, hangPoint.HandCollider, true);
            SetFlip(hangPoint.SpriteFlip);

            animator.SetTrigger("hang");
        
            transform.parent = hangPoint.HandCollider.transform;

            transform.localPosition = hangPoint.HoldPoint.localPosition;
            rb.velocity = Vector2.zero;
        }

        public void Unhang()
        {
            transform.parent = null;
            transform.position = hangPoint.ClimbPoint.position;
            rb.isKinematic = false;
            Physics2D.IgnoreCollision(standingCollider, hangPoint.HandCollider, false);
            Physics2D.IgnoreCollision(crouchCollider, hangPoint.HandCollider, false);
            hangPoint = null;
        }

        public void Groundslide()
        {
            rb.AddForce( rb.velocity.x * SlidingSpeedScale * Vector2.right,ForceMode2D.Impulse);
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -SlidingSpeedCap,SlidingSpeedCap), rb.velocity.y);
            if (groundSlideDeathCoroutine != null)
            {
                StopCoroutine(groundSlideDeathCoroutine);
                groundSlideDeathCoroutine = StartCoroutine(GroundslideDeath());
            }
        }

        public IEnumerator GroundslideDeath()
        {
        
            yield return new WaitForSeconds(m_groundSlideDeathSeconds);
            groundSlideDeathCoroutine = null;
        }
        public void SetFlip(bool value)
        {
            //attackHitbox.localScale = new Vector3(value != spriteRenderer.flipX ? -attackHitbox.localScale.x : attackHitbox.localScale.x,attackHitbox.localScale.y,attackHitbox.localScale.z);
            //landFXPlayerPoint.localPosition = new Vector3(value != spriteRenderer.flipX ? -landFXPlayerPoint.localPosition.x : landFXPlayerPoint.localPosition.x, landFXPlayerPoint.localPosition.y, landFXPlayerPoint.localPosition.z);
            //landFXRenderer.flipX = value;
            spriteRenderer.flipX = value;
            //wallSlideParticleTransform.localPosition = new Vector3(value ? .539f : -.539f, wallSlideParticleTransform.localPosition.y);
        }

        public void RegenerateStamina()
        {
            Stamina += StaminaSpeed * Time.deltaTime;
        }
    }
}