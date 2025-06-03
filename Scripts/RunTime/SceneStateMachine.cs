using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HurricaneVR.Framework.Core;
using HurricaneVR.Framework.Core.Grabbers;
using RTLTMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SceneStateMachine : MonoBehaviour {

    [Header ("All grabbable objects")]
    public GameObject[] GrabBoxesGameObject;

    [Header ("All state classes")]
    public CheckHealthState CheckHealthState = new ();
    public CallHelpState CallHelpState = new ();
    public CheckVain CheckVain = new ();
    public BreathingMouthState BreathingMouthState = new ();
    public HaertMassageState HaertMassageState = new ();
    public ClearAirWays clearAirWays = new ();
    public CutTHeShirt CutTHeShirt = new ();
    public EndOfCprSession EndOfCprSession = new ();
    public QuizeState QuizeState = new ();
    public MenuState MenuState = new ();

    [Header ("dead man body skin")]
    public SkinnedMeshRenderer deadmanSkinMeshRenderer;
    public Texture2D cutShirtTexture;

    [Header ("player body part")]
    [Tooltip (" left hand for getting disactive in massage state")] public GameObject leftHandModel;
    [Tooltip ("this is an extra hand model in right hand for massage satate")] public GameObject LefthandModelExtra;

    [Header ("test/train bool")]
    public bool IsTest;

    /// <summary>
    ///  single instance of HVR Grabbable object the current state
    /// </summary>
    internal HVRGrabbable StateBoxGrabbable;

    /// <summary>
    /// this th class that manage UI of states
    /// </summary>
    internal StatesUiManager statesUiManager;

    [SerializeField, Tooltip ("put user cpr setting scriptable ")] private UserCprData userCprData;

    private CPRbaseState _curentState, _prevState;
    private float _injuredBodyHeight;

    /// <summary>
    /// sceneManager singleton pattern. only one instance of this script can exist in the scene at a time.
    /// </summary>
    public static SceneStateMachine Inctance { get; private set; }

    /// <summary>
    /// the script that handle user massage speed
    /// </summary>
    public TapCounter TapCounter { get; set; }

    /// <summary>
    /// single instance of hint shadow object or box colider in the current state
    /// </summary>
    public GameObject StateGrabBoxGameObject { get; set; }

    /// <summary>
    /// countdown timer to control the time for cpr sessions
    /// </summary>
    public CountdownTimer CountdownTimer { get; set; }

    /// <summary>
    /// this the class that calculate the score of the user
    /// </summary>
    public ScoreCalculator ScoreCalculator { get; set; }

    /// <summary>
    /// the class that manage the sound effects
    /// </summary>
    public CprSound CprSound { get; set; }

    /// <summary>
    /// the class that manage the question and answer system
    /// </summary>
    public QuestionManager QuestionManager { get; set; }
    public int CprSessionRepeatTime { get; set; }

    /// <summary>
    /// the hashset of valid states that have a question in train mode
    /// </summary>
    readonly HashSet<CPRbaseStates> validStates = new () {
        CPRbaseStates.CheckVain, CPRbaseStates.CallHelpState, CPRbaseStates.CutTHeShirt, CPRbaseStates.HaertMassageState
    };
#region states
    private void Awake () {
        if (Inctance != null) {
            Debug.Log ("return: singleton pattern");
            Destroy (gameObject);
        } else {
            Inctance = this;
            //  DontDestroyOnLoad(gameObject);
        }
    }
    void Start ()
    {
        CprSound = transform.GetComponent<CprSound>();
        CountdownTimer = transform.GetComponent<CountdownTimer>();
        TapCounter = transform.GetComponent<TapCounter>();
        statesUiManager = transform.GetComponent<StatesUiManager>();
        QuestionManager = statesUiManager.QuizPanel.transform.GetChild(0).GetComponent<QuestionManager>();
        ScoreCalculator = transform.GetComponent<ScoreCalculator>();

        SetCprData();
        _curentState = CheckHealthState;
        _curentState!.EnterState(this);
    }

    void Update () => _curentState.UpdateState (this);

    /// <summary>
    /// the switch state metode that switch between states
    /// </summary>
    /// <param name="state">next state</param>
    /// <param name="_delayTime">delay time to switch</param>
    public void SwitchState (CPRbaseState state, float _delayTime) {
        _curentState.ExitState (this); //leaving state
        StartCoroutine (WaitsecondsToActivateNextState (_delayTime, state));
    }

    /// <summary>
    /// set the curent state basics based on the given state index
    /// </summary>
    /// <param name="stateIndex">state index</param>
    public void SetCurentStateBasics (int stateIndex) {

        statesUiManager.ShowHelpBanner (stateIndex);
        SetGrabBoxGameobject (stateIndex);
        ShowShadowHands ();
    }
    private IEnumerator WaitsecondsToActivateNextState (float _second, CPRbaseState _newCurrentState) {
        yield return new WaitForSeconds (_second);
        //quiz part
        if (IsTest && validStates.Contains (_newCurrentState.GetKeyState ())) {
            if (CprSessionRepeatTime < HaertMassageState.massage_round / 2) {
                _prevState = _newCurrentState;
                _curentState = QuizeState;
                statesUiManager.ShowhintCanvas (true);
            } else {
                _curentState = _newCurrentState;
            }
        } else {
            _curentState = _newCurrentState;
            if (!IsTest)
                statesUiManager.ShowhintCanvas (false);
        }
        //start new state
        _curentState.EnterState (this);
        CprSound.PlaySound ("hint");
    }

    /// <summary>
    /// calls when exit quiz state and continue to last state
    /// </summary>
    public void ContinueLastState () {
        _curentState.ExitState (this); //exit quiz state and continue
        _curentState = _prevState;
        _curentState.EnterState (this); //start new state
    }
#endregion states    

#region grabbable 
    private void SetGrabBoxGameobject (int index) => StateGrabBoxGameObject = GrabBoxesGameObject[index];

    /// <summary>
    /// set curent state HVR Grabbable object active and save its position height
    /// </summary>
    public void SetGrabble () {
        StateBoxGrabbable = StateGrabBoxGameObject.GetComponent<HVRGrabbable> ();
        StateBoxGrabbable.enabled = true;
        _injuredBodyHeight = StateBoxGrabbable.transform.position.y;
    }

    public void DisactiveGrabBox () => StateBoxGrabbable.enabled = false;
    public void ShowShadowHands () => StateGrabBoxGameObject.transform.GetChild (0).gameObject.SetActive (true); //shadow hands always as a first child
    public void HideShadowHands () => StateGrabBoxGameObject.transform.GetChild (0).gameObject.SetActive (false);

    /// <summary>
    /// change the mobile hint shadows by garbbing phone or head shadows for celaring the airWAy or checking the Vain
    /// </summary>
    /// <param name="IsGrabbed"></param>
    public void TwoSegmaentGrabbed (bool IsGrabbed) {
        if (IsGrabbed)
            FirstPointSegmentGrabbed ();
        else
            FirstPointSegmentReleased ();
    }

    private void FirstPointSegmentReleased () {
        StateGrabBoxGameObject.transform.GetChild (0).GetChild (0).gameObject.SetActive (true); //head point or //phoen grab point
        StateGrabBoxGameObject.transform.GetChild (0).GetChild (1).gameObject.SetActive (false); //second hint and box
        StateGrabBoxGameObject.transform.GetChild (0).GetChild (2).gameObject.SetActive (false);
    }

    private void FirstPointSegmentGrabbed () {
        StateGrabBoxGameObject.transform.GetChild (0).GetChild (0).gameObject.SetActive (false); //head point or //phoen grab point
        StateGrabBoxGameObject.transform.GetChild (0).GetChild (1).gameObject.SetActive (true); //second hint and box
        StateGrabBoxGameObject.transform.GetChild (0).GetChild (2).gameObject.SetActive (true);
    }

    /// <summary>
    /// force release all hand grabbers that grabs the GRabbable object 
    /// </summary>
    public void ForceReleaseAll () {
        foreach (var item in StateBoxGrabbable.HandGrabbers.ToList ()) {
            item.ForceRelease ();
        }
    }
    public void CheckBodyHeight (float _alarmHeight) {
        if (StateBoxGrabbable.transform.position.y - _injuredBodyHeight > _alarmHeight) {
            ForceReleaseAll ();
            //alarm messaga for movement
        }
    }
#endregion grabbable

#region Set data
    /// <summary>
    /// incresses when a cycle of massage state,cmouth breathing state and check vain state passed
    /// </summary>
    public void UpdateRepeat () {
        statesUiManager.MassageDataCanvas.transform.GetChild (1).GetChild(1).GetComponent<RTLTextMeshPro> ().text = (CprSessionRepeatTime + 1)
            .ToString ();
    }
    private void SetCprData()
    {
        IsTest = userCprData.IsTesting;
        HaertMassageState.massage_limit=userCprData.MassageLimit;
        HaertMassageState.massage_round=userCprData.MassageRound;
        
    }
#endregion Set data

#region timer
    public void SetTimer (float time) => CountdownTimer.SetTimer (time);
    public void ShowTimerBanner () {
        statesUiManager.UIdataCanvasParent.transform.GetChild (0).gameObject.SetActive (true);
        SetActiveCountDown (true);
    }
    public void HideTimerBanner () {
        statesUiManager.UIdataCanvasParent.transform.GetChild (0).gameObject.SetActive (false);
        SetActiveCountDown (false);
    }
    public void SetActiveCountDown (bool isActive) => CountdownTimer.IstimerStart = isActive;
    public void SetActiveMissionTime (bool isActive) => CountdownTimer.IsMissionTime = isActive;
#endregion timer
}