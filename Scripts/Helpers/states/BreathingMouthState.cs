using HurricaneVR.Framework.Core;
using RTLTMPro;
using UnityEngine;

public class BreathingMouthState : CPRbaseState {
    const float alarmHeight = .1f;
    private GameObject headGameObject, massageBanner;
    private HVRGrabbable headGrabbable;
    private int breathCounter;
    private bool _isTwoHandGrabded, _isOutOfHeadBox;
    private bool isFirstTrigger = true;
    private SceneStateMachine _sceneStateMachine;

    public override void EnterState (SceneStateMachine sceneMachine) {

        sceneMachine.statesUiManager.ShowHelpBanner (6);
        sceneMachine.SetTimer (1);
        HeadSetupForBreathingPose (sceneMachine);
        SetupMassageBanner (sceneMachine);
        _sceneStateMachine = sceneMachine;
        sceneMachine.statesUiManager.SetTimerCanvasDistance (.3f);
    }

    private void HeadSetupForBreathingPose (SceneStateMachine sceneMachine) {

        headGameObject = sceneMachine.GrabBoxesGameObject[2];
        headGameObject.transform.GetChild (2).gameObject.SetActive (true); //head breathing shadow hands
        SetHeadActiveGrabPoint (true);
    }
    private void SetupMassageBanner (SceneStateMachine sceneMachine) {

        massageBanner = sceneMachine.statesUiManager.GetUpDownBanner ();
        breathCounter = 0;
        massageBanner.transform.GetChild (7).GetComponent<RTLTextMeshPro> ().text = breathCounter.ToString ();
    }

    public override void UpdateState (SceneStateMachine sceneMachine) {

        if (headGrabbable.HandGrabbers.Count > 1) {
            _isTwoHandGrabded = true;
            headGameObject.transform.GetChild (2).GetChild (0).gameObject.SetActive (false); //hide Mouth breathing two  hands
            if (_isOutOfHeadBox)
                headGameObject.transform.GetChild (2).GetChild (1).gameObject.SetActive (true); //show head shadow
            else
                headGameObject.transform.GetChild (2).GetChild (1).gameObject.SetActive (false); //hide head shadow

        } else {
            _isTwoHandGrabded = false;
            headGameObject.transform.GetChild (2).GetChild (0).gameObject.SetActive (true);
            headGameObject.transform.GetChild (2).GetChild (1).gameObject.SetActive (false); //hide head shadow
        }
        sceneMachine.CheckBodyHeight (alarmHeight);
    }
    public override void ExitState (SceneStateMachine sceneMachine) {

        sceneMachine.ForceReleaseAll ();
        SetHeadActiveGrabPoint (false);
        headGameObject.transform.GetChild (2).gameObject.SetActive (false); //head breathing shadow hands and bound
        sceneMachine.statesUiManager.HideHelpBanner (6);
    }
    private void SetHeadActiveGrabPoint (bool _IsActieve) {

        headGameObject.transform.GetChild (1).gameObject.SetActive (_IsActieve); //if there is a grab point, is placed as a second child 
        headGrabbable = headGameObject.GetComponent<HVRGrabbable> ();
        headGrabbable.enabled = _IsActieve;
    }

    /// <summary>
    ///  calls when contact mouth breath box in mouth breathing state
    /// </summary>
    /// <param name="isOutOfHeadBox"> is triggered with box or not</param>
    public void MouthBreathingCall (bool isOutOfHeadBox) {
        _isOutOfHeadBox = isOutOfHeadBox;
        if (!isOutOfHeadBox) {
            if (_isTwoHandGrabded) {
                if (isFirstTrigger) {
                    _sceneStateMachine.ShowTimerBanner ();
                    isFirstTrigger = false;
                    _sceneStateMachine.CprSound.PlaySound ("breath_out");
                }

                if (_sceneStateMachine.CountdownTimer.GetCurentTime () == 0) {
                    breathCounter += 1;
                    massageBanner.transform.GetChild (7).GetComponent<RTLTextMeshPro> ().text = breathCounter.ToString ();
                    _sceneStateMachine.SetTimer (1);
                    _sceneStateMachine.HideTimerBanner ();
                }
            }
        } else {
            _sceneStateMachine.SetTimer (1);
            _sceneStateMachine.HideTimerBanner ();
            isFirstTrigger = true;
            UpdateByBreathCount (_sceneStateMachine);
        }
    }

    private void UpdateByBreathCount (SceneStateMachine sceneMachine) {
        if (breathCounter == 2) {
            sceneMachine.CprSessionRepeatTime += 1;
            sceneMachine.UpdateRepeat ();
            if (sceneMachine.CprSessionRepeatTime == HaertMassageState.massage_round / 2 ||
                sceneMachine.CprSessionRepeatTime == HaertMassageState.massage_round) {
                SceneStateMachine.Inctance.SwitchState (SceneStateMachine.Inctance.CheckVain, 2f); //2 s  dealy
            } else
                SceneStateMachine.Inctance.SwitchState (SceneStateMachine.Inctance.HaertMassageState, 0f); //almost no dealy
        }
    }
    public override CPRbaseStates GetKeyState () {
        return CPRbaseStates.BreathingMouthState;
    }
}