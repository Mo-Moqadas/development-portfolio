
public class EndOfCprSession : CPRbaseState
{
    public override void EnterState(SceneStateMachine sceneMachine)
    {
        sceneMachine.SetActiveMissionTime(false);
        sceneMachine.statesUiManager.uiPanels[7].SetActive(true);
        sceneMachine.ScoreCalculator.CalculateTotalScore();
        sceneMachine.ScoreCalculator.ShowAllData();
    }

    public override void ExitState(SceneStateMachine sceneMachine){}
    public override void UpdateState(SceneStateMachine sceneMachine){}
    public override CPRbaseStates GetKeyState() => CPRbaseStates.EndOfCprSession;
}