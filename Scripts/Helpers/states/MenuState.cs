
public class MenuState : CPRbaseState
{
    public override void EnterState(SceneStateMachine sceneMachine) => sceneMachine.statesUiManager.ShowHelpBanner(8);
    public override void ExitState(SceneStateMachine sceneMachine) => sceneMachine.statesUiManager.HideHelpBanner(8);
    public override void UpdateState(SceneStateMachine sceneMachine){}
     public override CPRbaseStates GetKeyState() => CPRbaseStates.MenuState;
}
