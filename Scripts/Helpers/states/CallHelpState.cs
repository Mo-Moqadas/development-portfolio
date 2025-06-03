public class CallHelpState : CPRbaseState {
    private bool _isPhoneGrabed;
    private SceneStateMachine _sceneStateMachine;

    public override void EnterState (SceneStateMachine sceneMachine) {

        sceneMachine.SetCurentStateBasics (1);
        sceneMachine.SetGrabble ();
        _sceneStateMachine = sceneMachine;
    }
    public override void UpdateState (SceneStateMachine sceneMachine) {

        if (sceneMachine.StateBoxGrabbable.HandGrabbers.Count > 0) {
            sceneMachine.TwoSegmaentGrabbed (true);
            _isPhoneGrabed = true;
        } else {
            _isPhoneGrabed = false;
            sceneMachine.TwoSegmaentGrabbed (false);
            sceneMachine.HideTimerBanner ();
            sceneMachine.SetTimer (1);
        }
    }
    public override void ExitState (SceneStateMachine sceneMachine) {

        sceneMachine.statesUiManager.HideHelpBanner (1);
        sceneMachine.DisactiveGrabBox ();
        sceneMachine.HideShadowHands ();
        sceneMachine.HideTimerBanner ();
    }
    /// <summary>
    /// /// the phone called button, it calls from on stay trigger
    /// </summary>
    public void Phonecall () {
        if (_isPhoneGrabed) {
            _sceneStateMachine.SetActiveCountDown (true);
            _sceneStateMachine.ShowTimerBanner ();
            if (_sceneStateMachine.CountdownTimer.GetCurentTime () == 0)
                _sceneStateMachine.SwitchState (_sceneStateMachine.clearAirWays, .1f); //almost no dealy
        }
    }

    /// <summary>
    /// exit the phone called button, it calls from on Exit Trigger
    /// </summary>
    public void PhonecallExit () {
        // _sceneStateMachine.SetTimer (1);
        //stop Count down
        _sceneStateMachine.SetActiveCountDown (false);
    }
    public override CPRbaseStates GetKeyState () {
        return CPRbaseStates.CallHelpState;
    }
}