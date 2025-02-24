using UnityEngine;
using DG.Tweening;
using Skullknight.Core;
using Skullknight.Enemy.Demon_Boss;
using Skullknight.Entity;
using Skullknight.Player.Statemachine;
using Skullknight.State;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class DemonBossController : EntityController<EDemonBossState,DemonBossController>
{
    public enum DemonBossPhase
    {
        FirstPhase,
        SecondPhase
    }

    private DemonBossPhase _phase;
    public DemonBossPhase Phase => _phase;

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

    public override bool TakeDamage(int amount)
    {
        if(isDamageable)
        {
            spriteRenderer.material.SetFloat("_HitFXPercent", 1f);
            if (takingDamageTween != null)
            {
                takingDamageTween.Kill();
                takingDamageTween = null;
            }

            takingDamageTween = DOTween.To(() => spriteRenderer.material.GetFloat("_HitFXPercent"),
                x => spriteRenderer.material.SetFloat("_HitFXPercent", x), 0f, .3f);
            health = Mathf.Clamp(health - amount, 0, maxHealth);
            OnHealthChanged?.Invoke(health);
            if (health <= 0)
            {
                if (Phase == DemonBossPhase.FirstPhase)
                {
                    health = maxHealth;
                    _phase = DemonBossPhase.SecondPhase;
                    GameManager.Instance.ToCutscene(2);
                }
                else if (Phase == DemonBossPhase.SecondPhase)
                {
                    GameManager.Instance.ToCutscene(3);
                }
            }

            return true;
        }

        return false;
    }
    private void Awake()
    {
        onHealthChanged = new UnityEvent<int>();
        health = 400;
        maxHealth = 400;
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
    protected override void OnDisable()
    {
        base.OnDisable();
        animator?.Play("Idle");
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
        if(Phase == DemonBossPhase.FirstPhase) fireAnimator.Play("firebreath");
        else fireAnimator.Play("bluefirebreath");
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
            FlipX(true);
        }
        else
        {
            FlipX(false);
        }
    }
    public override void FlipX(bool value)
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