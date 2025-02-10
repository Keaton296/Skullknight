using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
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
        states.Add(EDemonBossState.Idle,new DemonIdleState(this));
        states.Add(EDemonBossState.BreathAttack,new DemonBreathAttackState(this));
        states.Add(EDemonBossState.FireballAttack,new DemonBossFireballAttackState(this));
        GameManager.Instance?.OnStateChange.AddListener(OnGameStateChanged);
        onHealthChanged = new UnityEvent<int>();
        GameManager.Instance.CurrentBoss = gameObject;
    }

    protected override void Start()
    {
        
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
    public void ToggleHeadGlow()
    {
        DOTween.To(() => spriteRenderer.material.GetFloat("_EmissionPercent1"), x => spriteRenderer.material.SetFloat("_EmissionPercent1",x),headGlow? 0 : 2,1f);
        headGlow = !headGlow;
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
    protected List<Coroutine> coroutines; 

    public DemonBossState(DemonBossController _controller)
    {
        controller = _controller;
        coroutines = new List<Coroutine>();
    }
    public void SetFlip(bool value)
    {
        controller.spriteRenderer.flipX = value;
    }
    public void KillCoroutines()
    {
        foreach (var coroutine in coroutines)
        {
            controller.StopCoroutine(coroutine);
        }
    }
}
