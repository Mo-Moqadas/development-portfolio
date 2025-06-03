
using UnityEngine;

public class CheckHealthState : CPRbaseState
{

    const float alarmHeight = .2f;
    readonly Vector3 targetposition = new(11f, 11.2f, 5.6f);
    readonly Vector3 targetRotation = new(35, -24, 0);
    public override void EnterState(SceneStateMachine sceneMachine)
    {
        sceneMachine.SetCurentStateBasics(0);
        sceneMachine.SetGrabble();
        sceneMachine.SetTimer(3);
       // sceneMachine.statesUiManager.SetDataBannerPosition(targetposition, targetRotation);
        sceneMachine.statesUiManager.SetTimerCanvasDistance(1);
    }
    public override void UpdateState(SceneStateMachine sceneMachine)
    {


        if (sceneMachine.StateBoxGrabbable.HandGrabbers.Count > 1)
        {

            sceneMachine.SetActiveMissionTime(true);
            sceneMachine.ShowTimerBanner();
            sceneMachine.HideShadowHands();

            if (sceneMachine.CountdownTimer.GetCurentTime() == 0)
            {
                sceneMachine.SetActiveCountDown(false);
                sceneMachine.SwitchState(sceneMachine.CallHelpState, .1f);//almost no dealy
            }
        }
        else
        {
            sceneMachine.HideTimerBanner();
            sceneMachine.ShowShadowHands();
            sceneMachine.SetTimer(5);
        }
        sceneMachine.CheckBodyHeight(alarmHeight);
    }

    public override void ExitState(SceneStateMachine sceneMachine)
    {
        sceneMachine.statesUiManager.HideHelpBanner(0);
        sceneMachine.DisactiveGrabBox();
        sceneMachine.HideTimerBanner();

    }

    public override CPRbaseStates GetKeyState() => CPRbaseStates.CheckHealthState;
}