
public class QuizeState : CPRbaseState
{
    public override void EnterState(SceneStateMachine sceneMachine)
    {
        sceneMachine.SetActiveMissionTime(false);
        sceneMachine.statesUiManager.QuizPanel.SetActive(true);
        sceneMachine.QuestionManager.CallQuestion();
        sceneMachine.CprSound.PlaySound("qusetion");
    }
    public override void ExitState(SceneStateMachine sceneMachine)
    {
        sceneMachine.SetActiveMissionTime(true);
        sceneMachine.statesUiManager.QuizPanel.SetActive(false);
        sceneMachine.QuestionManager.NextQuestion();
    }
    public override void UpdateState(SceneStateMachine sceneMachine){}
    public override CPRbaseStates GetKeyState() => CPRbaseStates.QuizeState;
}
