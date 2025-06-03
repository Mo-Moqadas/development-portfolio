public class CutTHeShirt : CPRbaseState {
    private SceneStateMachine _sceneStateMachine;
    public override void EnterState (SceneStateMachine sceneMachine) {
        sceneMachine.SetCurentStateBasics (4);
        sceneMachine.SetTimer (1);
        _sceneStateMachine = sceneMachine;
    }
    public override void UpdateState (SceneStateMachine sceneMachine) { }
    public override void ExitState (SceneStateMachine sceneMachine) {
        sceneMachine.statesUiManager.HideHelpBanner (4);
        sceneMachine.HideShadowHands ();
        sceneMachine.HideTimerBanner ();
    }

    /// <summary>
    /// change shirt material in the cut the shirt state aafter 1 second
    /// </summary>
    public void ChangeShirt () {
        _sceneStateMachine.SetActiveCountDown (true);
        _sceneStateMachine.ShowTimerBanner ();
        if (_sceneStateMachine.CountdownTimer.GetCurentTime () == 0) {
            _sceneStateMachine.deadmanSkinMeshRenderer.material.mainTexture = _sceneStateMachine.cutShirtTexture;
            _sceneStateMachine.SwitchState (_sceneStateMachine.HaertMassageState, .1f); //2s delay
        }
    }

    /// <summary>
    /// exit for cutting the shirt and stop countdown
    /// </summary>
    public void ChangeShirtExit () => _sceneStateMachine.SetActiveCountDown (false); //stop countdown

    public override CPRbaseStates GetKeyState () => CPRbaseStates.CutTHeShirt;
}