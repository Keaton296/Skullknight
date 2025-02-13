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
using UnityEngine.Serialization;

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
    [SerializeField] public PlayerInput playerInput;
    
    [SerializeField] private CutsceneManager m_cutsceneManager; //cutsceneManager 
    [SerializeField] private CheckpointManager m_checkpointManager; //checkpointManager
    [SerializeField] public bool OnBossFight => currentBoss!=null;
    [SerializeField] private GameObject currentBoss;
    public UnityEvent<GameObject> OnBossChange;
    public GameObject CurrentBoss
    {
        get
        {
            return currentBoss;
        }
        set
        {
            currentBoss = value;
            OnBossChange?.Invoke(currentBoss);
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
        m_cutsceneState = new CutsceneGameState(0,m_cutsceneManager);
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
    public void OnEscapeMenuToggle(InputAction.CallbackContext obj)
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
            var escapeState = states[EGameManagerState.EscapeMenu] as EscapeMenuGameState;
            if (escapeState.IsStoppedCutscene)
            {
                ChangeState(EGameManagerState.Cutscene);
                escapeState.IsStoppedCutscene = false; 
            }
            else
            {
                ChangeState(EGameManagerState.Playing);
            }
        }
        else if (stateEnum == EGameManagerState.Cutscene)
        { 
            //m_cutsceneManager.Pause();
            var escapeState = states[EGameManagerState.EscapeMenu] as EscapeMenuGameState;
            var cutsceneState = states[EGameManagerState.Cutscene] as CutsceneGameState;
            cutsceneState.isStoppedByEscapeMenu = true;
            escapeState.IsStoppedCutscene = true;
            Time.timeScale = 0;
            ChangeState(EGameManagerState.EscapeMenu);
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
    private CutsceneManager cutsceneManager;
    public bool isStoppedByEscapeMenu = false;
    public CutsceneGameState(int _cutsceneIndex, CutsceneManager manager)
    {
        cutsceneIndex = _cutsceneIndex;
        cutsceneManager = manager;
    }

    public override void StateUpdate()
    { 
        
    }

    public override void StateFixedUpdate()
    {
        
    }

    public override void SubscribeEvents()
    {
        GameManager.Instance.playerInput.actions["Menu"].performed += GameManager.Instance.OnEscapeMenuToggle;
        GameManager.Instance.playerInput.actions["MenuUI"].performed += GameManager.Instance.OnEscapeMenuToggle;
        
        if(cutsceneManager.Cutscenes[cutsceneIndex].skippable)
        {
            GameManager.Instance.PlayerInput.actions["Submit"].performed += OnSkipPressed;
            GameManager.Instance.PlayerInput.actions["Left Click"].performed += OnSkipPressed;
        }
        
        GameManager.Instance.CutsceneManager.OnCutsceneFinished.AddListener(OnCutsceneEnd);
    }

    private void OnCutsceneEnd()
    {
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
        if(cutsceneManager.Cutscenes[cutsceneIndex].skippable)
        {
            GameManager.Instance.PlayerInput.actions["Submit"].performed -= OnSkipPressed;
            GameManager.Instance.PlayerInput.actions["Left Click"].performed -= OnSkipPressed;
        }
        GameManager.Instance.playerInput.actions["Menu"].performed -= GameManager.Instance.OnEscapeMenuToggle;
        GameManager.Instance.playerInput.actions["MenuUI"].performed -= GameManager.Instance.OnEscapeMenuToggle;
        
        GameManager.Instance.CutsceneManager.OnCutsceneFinished.RemoveListener(OnCutsceneEnd);
    }

    public override void EnterState()
    {
        //disable the player controls  done
        //easing in animation, screen getting darker
        //on end of easing, start the cutscene.
        //enable the escape menu
        if (isStoppedByEscapeMenu)
        {
            isStoppedByEscapeMenu = false;
        }
        else
        {
            pressedSkip = false;
            GameManager.Instance.CutsceneManager.PlayCutscene(cutsceneIndex);
        }
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
    public bool IsStoppedCutscene = false; 
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
    }

    public override void ExitState()
    {
        
    }
}