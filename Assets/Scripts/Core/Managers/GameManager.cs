using System;
using System.Collections;
using UnityEngine;
using Skullknight;
using Skullknight.Player.Statemachine;
using Skullknight.State;
using Skullknight.Core;
using Unity.VisualScripting;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : StateManager<GameManager.EGameManagerState>
{
    public enum EGameManagerState
    {
        Playing,
        EscapeMenu,
        Cutscene,
        Gameover
    }
    public static GameManager Instance;
    [SerializeField] private PlayerInput playerInput;
    
    [SerializeField] private CutsceneManager m_cutsceneManager; //cutsceneManager 
    [SerializeField] private CheckpointManager m_checkpointManager; //checkpointManager
    [SerializeField] private bool onBossfight = false;
    public UnityEvent<bool> OnBossFightToggle;
    public bool OnBossFight
    {
        get
        {
            return onBossfight;
        }
        set
        {
            onBossfight = value;
            OnBossFightToggle?.Invoke(onBossfight);
        }
    }
    private CutsceneGameState m_cutsceneState; 
    public CutsceneManager CutsceneManager => m_cutsceneManager;
    public PlayerInput PlayerInput => playerInput;
    public CheckpointManager CheckpointManager => m_checkpointManager;
    protected override void Awake()
    {
        Instance = this;
        states.Add(EGameManagerState.Playing,new PlayingGameState());
        states.Add(EGameManagerState.EscapeMenu,new EscapeMenuGameState());
        states.Add(EGameManagerState.Gameover,new GameOverGameState(this));
        m_cutsceneState = new CutsceneGameState(0);
        states.Add(EGameManagerState.Cutscene, m_cutsceneState);
    }

    protected override void Start()
    {
        CutsceneInstance first = m_cutsceneManager.GetFirstCutscene();
        if (first != null && first.playOnStart)
        {
            ChangeState(EGameManagerState.Cutscene);
        }
        else
        {
            ChangeState(EGameManagerState.Playing);
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (playerInput != null)
        {
            playerInput.actions["Menu"].performed += OnEscapeMenuToggle;
            playerInput.actions["MenuUI"].performed += OnEscapeMenuToggle;
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (playerInput != null)
        {
            playerInput.actions["Menu"].performed -= OnEscapeMenuToggle;
            playerInput.actions["MenuUI"].performed -= OnEscapeMenuToggle;
        }
    }
    private void OnEscapeMenuToggle(InputAction.CallbackContext obj)
    { 
        ToggleEscapeMenu();
    }

    public void ToggleEscapeMenu()
    {
        if(stateEnum == EGameManagerState.Playing)
        {
            Time.timeScale = 0;
            ChangeState(EGameManagerState.EscapeMenu);
        }
        else if(stateEnum == EGameManagerState.EscapeMenu)
        {
            Time.timeScale = 1;
            ChangeState(EGameManagerState.Playing);
        }
    }

    public void GameOverTransition()
    {
        if (stateEnum == EGameManagerState.Gameover) return;
        ChangeState(EGameManagerState.Gameover);
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ToCutscene(int sceneIndex)
    {
        m_cutsceneState.cutsceneIndex = sceneIndex;
        ChangeState(EGameManagerState.Cutscene);
    }
    
}
internal class CutsceneGameState : BaseState<GameManager.EGameManagerState>
{
    public int cutsceneIndex;
    private bool pressedSkip = false;
    
    public CutsceneGameState(int _cutsceneIndex)
    {
        cutsceneIndex = _cutsceneIndex;
    }

    public override void StateUpdate()
    { 
        
    }

    public override void StateFixedUpdate()
    {
        
    }

    public override void SubscribeEvents()
    {
        GameManager.Instance.PlayerInput.actions["Submit"].performed += OnSkipPressed;
        GameManager.Instance.PlayerInput.actions["Left Click"].performed += OnSkipPressed;
        GameManager.Instance.CutsceneManager.OnCutsceneFinished.AddListener(OnCutsceneEnd);
    }

    private void OnCutsceneEnd()
    {
        if (GameManager.Instance.CutsceneManager.Cutscenes[cutsceneIndex].isBossCutscene)
        {
            GameManager.Instance.OnBossFight = true;
        }
        GameManager.Instance.ChangeState(GameManager.EGameManagerState.Playing);
    }

    private void OnSkipPressed(InputAction.CallbackContext obj)
    {
        if (pressedSkip)
        {
            GameManager.Instance.CutsceneManager.EndCutscene();
            GameManager.Instance.ChangeState(GameManager.EGameManagerState.Playing);
        }
        else
        {
            pressedSkip = true;
        }
    }

    public override void UnsubscribeEvents()
    {
        GameManager.Instance.PlayerInput.actions["Submit"].performed -= OnSkipPressed;
        GameManager.Instance.PlayerInput.actions["Left Click"].performed -= OnSkipPressed;
        GameManager.Instance.CutsceneManager.OnCutsceneFinished.RemoveListener(OnCutsceneEnd);
    }

    public override void EnterState()
    {
        //disable the player controls  done
        //easing in animation, screen getting darker
        //on end of easing, start the cutscene.
        //enable the escape menu
        pressedSkip = false;
        GameManager.Instance.CutsceneManager.PlayCutscene(cutsceneIndex);
    }

    public override void ExitState()
    {
        
    }
}

internal class PlayingGameState : BaseState<GameManager.EGameManagerState>
{
    public override void EnterState()
    {
        
    }

    public override void ExitState()
    {
        
    }

    public override void StateUpdate()
    {
        
    }

    public override void StateFixedUpdate()
    {
        
    }

    public override void SubscribeEvents()
    {
        
    }

    public override void UnsubscribeEvents()
    {
        
    }
}

internal class GameOverGameState : BaseState<GameManager.EGameManagerState>
{
    private GameManager gameManager;
    //disable player controls
    //start fading out animation
    //load the last checkpoint
    public GameOverGameState(GameManager manager)
    {
        gameManager = manager;
    }
    public override void EnterState()
    {
        gameManager.StartCoroutine(GameOverAnimation());
    }

    IEnumerator GameOverAnimation()
    { // needs rework
        
        UITransitionManager.Instance.blackoutAnimator.SetTrigger("In");
        yield return null;
        yield return new WaitUntil(() =>
            !UITransitionManager.Instance.blackoutAnimator.GetCurrentAnimatorStateInfo(0).IsName("In")); //stays here
        gameManager.RestartLevel();
    }
    public override void ExitState()
    {
        
    }

    public override void StateUpdate()
    {
        
    }

    public override void StateFixedUpdate()
    {
        
    }

    public override void SubscribeEvents()
    {
        
    }

    public override void UnsubscribeEvents()
    {
        
    }
}
internal class EscapeMenuGameState : BaseState<GameManager.EGameManagerState>
{
    public override void StateUpdate()
    {
        
    }

    public override void StateFixedUpdate()
    {
        
    }

    public override void SubscribeEvents()
    {
        
    }

    public override void UnsubscribeEvents()
    {
        
    }

    public override void EnterState()
    {
        //disable the player controls
        //easing in animation, screen getting darker
        //on end of easing, start the cutscene.
        //enable the escape menu
        if(PlayerController.Instance != null) PlayerController.Instance.enabled = false;
    }

    public override void ExitState()
    {
        PlayerController.Instance.enabled = true;
    }
}