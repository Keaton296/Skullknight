using UnityEngine;
using DG.Tweening;
using System.Collections;
using Unity.VisualScripting;
using Cinemachine;
public class DemonBossController : MonoBehaviour,IDamageable
{
    public Vector2 fireBreathDemonOffset;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    [SerializeField] int health = 300;
    Tween takingDamageCoroutine;
    public float moveLerpSpeed = .2f;

    public bool canMove = false;
    public bool canTurn = true;
    public Coroutine currentMove;

    [Header("Transforms")]
    public Transform moveTarget;
    public Transform flyingPlaceTransform;
    public Transform playerTransform;
    public Transform breathAttackTransform;

    [Header("Animators")]
    public Animator floorFireAnimator;
    [Header("Cinemachine")]
    public CinemachineVirtualCamera vcam;
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
                if( takingDamageCoroutine != null)
                {
                    takingDamageCoroutine.Kill();
                    takingDamageCoroutine = null;
                }
                takingDamageCoroutine = DOTween.To(() => spriteRenderer.material.GetFloat("_HitFXPercent"), x => spriteRenderer.material.SetFloat("_HitFXPercent",x),0f,.3f);
            }
            BossBar.Instance?.statBar.OnValueChange(value);
        }
    }
    IState State { get { return _state; } set { _state?.OnStateEnd();_state = value; _state.OnStateStart(); } }


    IState _state;
    public IState DemonState
    {
        get
        {
            return _state;
        }
        set
        {
            if (_state != null) _state.OnStateEnd();
            _state = value;
            _state.OnStateStart();
        }
    }
    public DemonStateFirstPhase demonStateFirstPhase;
    public DemonStateSecondPhase demonStateSecondPhase;
    private void Start()
    {
        State = demonStateFirstPhase;
    }
    private void Update()
    {
        State.StateUpdate();
    }
    private void FixedUpdate()
    {
        State.StateFixedUpdate();
    }
    public IEnumerator GoToPlace(DemonBossController controller, Transform place)
    {
        controller.moveTarget = place;
        canMove = true;
        yield return new WaitForSeconds(1);
        canMove = false;
        controller.moveTarget = null;
        currentMove = null;
    }

}
