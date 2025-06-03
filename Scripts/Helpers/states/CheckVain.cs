using HurricaneVR.Framework.Core.Grabbers;
using Unity.VisualScripting;
using UnityEngine;

public class CheckVain : CPRbaseState
{
    // readonly Vector3 targetposition = new(11.3f, 11.4f, 5.1f);
    // readonly Vector3 targetRotation = new(0, 130, 0);
    private Collider headColider;
    private SceneStateMachine _sceneStateMachine;
    private bool _isActiveAlive;
    private float _vibrateTime;
    private HVRHandGrabber _rightHandGrabber;

    public override void EnterState(SceneStateMachine sceneMachine)
    {
        sceneMachine.SetCurentStateBasics(3);
        sceneMachine.SetTimer(10);
        //sceneMachine.statesUiManager.SetDataBannerPosition(targetposition, targetRotation);
        sceneMachine.statesUiManager.SetTimerCanvasDistance(1);
        sceneMachine.statesUiManager.GetUpDownBanner().SetActive(false);
        headColider = sceneMachine.GrabBoxesGameObject[2].GetComponent<Collider>();
        if (sceneMachine.CprSessionRepeatTime == HaertMassageState.massage_round)
        {
            //heart sign activated
            sceneMachine.statesUiManager.UIdataCanvasParent.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).gameObject.SetActive(true);
            sceneMachine.statesUiManager.UIdataCanvasParent.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0)
                .localPosition = new(0, 19.7f, 0);
            _isActiveAlive = true;
            //as left hand extra model is in hiarcy of right hand physic we can reach to its hvrgrabber component
            _rightHandGrabber = sceneMachine.LefthandModelExtra.transform.parent.parent.GetComponent<HVRHandGrabber>();
        }
        _sceneStateMachine = sceneMachine;

    }
    public override void UpdateState(SceneStateMachine sceneMachine) { }

    public override void ExitState(SceneStateMachine sceneMachine)
    {
        sceneMachine.statesUiManager.HideHelpBanner(3);
        sceneMachine.HideTimerBanner();
        sceneMachine.TwoSegmaentGrabbed(false);
        sceneMachine.HideShadowHands();
        headColider.enabled = true;
    }

    /// <summary>
    /// call an update in check vain state to indicate there is contact with the head or not
    /// </summary>
    /// ///<param name="_IsChecking"></param>
    public void HeadUpdate(bool IsChestChecking)
    {
        if (IsChestChecking)
        {
            _sceneStateMachine.ShowTimerBanner();
            _sceneStateMachine.StateGrabBoxGameObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(false); //human head shadow location
            if (_isActiveAlive)
            {
                _vibrateTime += Time.deltaTime;
                if (_vibrateTime > 1)
                {
                    Vibrate(.01f, .1f, 1f);
                    _vibrateTime = 0;
                }
            }
            if (_sceneStateMachine.CountdownTimer.GetCurentTime() == 0)
            {
                CheckToSwichState(_sceneStateMachine);
            }
        }
        else
        {
            _sceneStateMachine.StateGrabBoxGameObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
            _sceneStateMachine.SetTimer(10);
            _sceneStateMachine.SetActiveCountDown(false);
        }
    }
    private static void CheckToSwichState(SceneStateMachine sceneMachine)
    {
        if (sceneMachine.CprSessionRepeatTime == HaertMassageState.massage_round)
            sceneMachine.SwitchState(sceneMachine.EndOfCprSession, .1f); //almost no dealy

        else if (sceneMachine.CprSessionRepeatTime == 0)
            sceneMachine.SwitchState(sceneMachine.CutTHeShirt, .1f); //almost no dealy
        else if (sceneMachine.CprSessionRepeatTime < HaertMassageState.massage_round)
            sceneMachine.SwitchState(sceneMachine.HaertMassageState, .1f); //almost no dealy
    }

    /// <summary>
    /// call in check the vain state to check is touching the vain or not
    /// </summary>
    /// <param name="_IsTouched"></param>
    public void VainTouchUpdate(bool isTouching)
    {
        if (isTouching)
        {
            _sceneStateMachine.TwoSegmaentGrabbed(true);
            headColider.enabled = false;
        }
        else
        {
            _sceneStateMachine.TwoSegmaentGrabbed(false);
            _sceneStateMachine.HideTimerBanner();
            _sceneStateMachine.SetTimer(10);
            headColider.enabled = true;
        }
    }
    private void Vibrate(float duration, float amplitude, float frequency) => _rightHandGrabber.Controller.Vibrate(duration, amplitude, frequency);

    public override CPRbaseStates GetKeyState() => CPRbaseStates.CheckVain;
}