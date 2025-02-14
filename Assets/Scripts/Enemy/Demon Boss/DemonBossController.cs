using UnityEngine;
using DG.Tweening;
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
    [SerializeField] private SpriteRenderer fireSpriteRenderer;
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private LaserShooter laserShooter;
    
    
    public Tween takingDamageTween;
    public Tween movementTween;
    
    public bool canTurn = true;
    public bool canTakeDamage = false;
    public bool headGlow = false;

    public Transform IdlingPoint;
    public Transform FireBreathTransform;
    public Transform mouthPoint;

    [SerializeField] private float movementDuration = 1f;
    
    [SerializeField] int health = 300;
    [SerializeField] private int maxHealth = 300;
    public int Health {
        get {
            return health;
        }
        set {
            if (value > health) { health = Mathf.Clamp(health, 0, maxHealth); }
            else if (value < health)
            {
                if (value <= 0)
                {
                    if (Phase == DemonBossPhase.FirstPhase)
                    {
                        health = maxHealth;
                        _phase = DemonBossPhase.SecondPhase;
                        GameManager.Instance.ToCutscene(2);
                        return;
                    }
                    else if (Phase == DemonBossPhase.SecondPhase)
                    {
                        GameManager.Instance.ToCutscene(3);
                    }
                }
                health = value;
                OnHealthChanged?.Invoke(value);
                
                spriteRenderer.material.SetFloat("_HitFXPercent", 1f);
                if( takingDamageTween != null)
                {
                    takingDamageTween.Kill();
                    takingDamageTween = null;
                }
                takingDamageTween = DOTween.To(() => spriteRenderer.material.GetFloat("_HitFXPercent"), x => spriteRenderer.material.SetFloat("_HitFXPercent",x),0f,.3f);
            }
        }
    }

    public int MaxHealth => maxHealth;
    public UnityEvent<int> OnHealthChanged => onHealthChanged;
    private UnityEvent<int> onHealthChanged;
    protected override void Awake()
    {
        onHealthChanged = new UnityEvent<int>();
        states.Add(EDemonBossState.Idle,new DemonIdleState(this));
        states.Add(EDemonBossState.BreathAttack,new DemonBreathAttackState(this));
        states.Add(EDemonBossState.FireballAttack,new DemonBossFireballAttackState(this));
        states.Add(EDemonBossState.LaserAttack, new DemonBossLaserAttack(this));
        GameManager.Instance?.OnStateChange.AddListener(OnGameStateChanged);
        GameManager.Instance.CurrentBoss = gameObject;
    }

    public void LaserAttack()
    {
        canTurn = false;
        laserShooter.ShootTarget(PlayerController.Instance.transform);
    }
    protected override void Start()
    {
        
    }
    public override void ChangeState(EDemonBossState newState) 
    {
        if (states.ContainsKey(newState))
        {
            currentState?.UnsubscribeEvents();
            DemonBossState dState = currentState as DemonBossState;
            dState?.KillCoroutines();
            currentState?.ExitState();
            currentState = states[newState];
            stateEnum = newState;
            OnStateChange?.Invoke(stateEnum);
            currentState.EnterState();
            currentState.SubscribeEvents();
        }
        else
        {
            Debug.LogError(string.Format("State '{0}' not found", newState));
        }
    }
    private void OnGameStateChanged(GameManager.EGameManagerState newState)
    {
        if (newState == GameManager.EGameManagerState.Playing && GameManager.Instance.CurrentBoss == gameObject)
        {
            this.enabled = true;
            ChangeState(EDemonBossState.Idle);
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
        Debug.Log("easdasd");
        base.OnDisable();
        animator.Play("Idle");
        DemonBossState state = currentState as DemonBossState;
        state.KillCoroutines();
    }
    public void MoveToTransform(Vector3 ToPoint, float duration)
    {
        if(movementTween != null)
        {
            movementTween.Kill();
            movementTween = null;
        }
        movementTween = transform.DOMove(ToPoint, duration, false);
    }

    public void MoveToIdlePosition()
    {
        MoveToTransform(IdlingPoint.position, 1f);
    }

    public void MoveToPlayerAttackPoint()
    {
        canTurn = false;
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
            SetFlip(true);
        }
        else
        {
            SetFlip(false);
        }
    }
    /// <summary>
    /// Looks to left unflipped.
    /// </summary>
    /// <param name="flip"></param>
    public void SetFlip(bool value)
    {
        spriteRenderer.flipX = value;
        FireBreathTransform.localPosition = new Vector3(Mathf.Abs(FireBreathTransform.localPosition.x) * (value ? 1 : -1), FireBreathTransform.localPosition.y,0);
        fireAnimator.GetComponent<SpriteRenderer>().flipX = value;
        fireSpriteRenderer.flipX = value;
        mouthPoint.localPosition = new Vector3(Mathf.Abs(mouthPoint.localPosition.x) * (value ? 1 : -1),mouthPoint.localPosition.y,0f);
    }
}

public enum EDemonBossState
{
    Idle,
    BreathAttack,
    FireballAttack,
    LaserAttack,
    Transforming,
    IdleTwo
}