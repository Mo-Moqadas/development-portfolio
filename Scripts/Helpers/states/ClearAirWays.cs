//using UnityEditor.Localization.Plugins.XLIFF.V20;

using UnityEngine;

public class ClearAirWays : CPRbaseState {
  const float alarmHeight = .1f;
  private SceneStateMachine _sceneStateMachine;
  private Transform _rightHandModel;

  public override void EnterState (SceneStateMachine sceneMachine) {
    sceneMachine.SetCurentStateBasics (2);
    sceneMachine.SetGrabble ();
    _sceneStateMachine = sceneMachine;
    _rightHandModel = sceneMachine.LefthandModelExtra.transform.parent.GetChild (0);

    /*find two point grabbable points and disactiavate them before next state to make sure it is working fine
    there is a not working issue when grab points activates in for first time in the middle of the game  */
    sceneMachine.GrabBoxesGameObject[2].transform.GetChild (1).gameObject.SetActive (false);;
  }
  public override void UpdateState (SceneStateMachine sceneMachine) {
    if (sceneMachine.StateBoxGrabbable.HandGrabbers.Count > 0) {
      sceneMachine.TwoSegmaentGrabbed (true);
      sceneMachine.CheckBodyHeight (alarmHeight);
    } else {
      sceneMachine.TwoSegmaentGrabbed (false);
      sceneMachine.HideTimerBanner ();
      sceneMachine.SetTimer (1);
    }
  }

  public override void ExitState (SceneStateMachine sceneMachine) {
    sceneMachine.statesUiManager.HideHelpBanner (2);
    sceneMachine.ForceReleaseAll ();
    sceneMachine.DisactiveGrabBox ();
    sceneMachine.HideShadowHands ();
    sceneMachine.HideTimerBanner ();
     Rotatehand (false);
  }

  /// <summary>
  /// the mouth will opene in clear air state after 1 second 
  /// </summary>
  public void OpenMouth () {
    //do animation to open mouth
    _sceneStateMachine.StateGrabBoxGameObject.transform.GetChild (3).localEulerAngles = new Vector3 (156, 0, 0); //this is the jaw of the head
    _sceneStateMachine.SetActiveCountDown (true);
    Rotatehand (true);
    _sceneStateMachine.ShowTimerBanner ();
    if (_sceneStateMachine.CountdownTimer.GetCurentTime () == 0)
      _sceneStateMachine.SwitchState (_sceneStateMachine.CheckVain, .1f); //1s  delay
  }

  private void Rotatehand (bool _isInMouth) => _rightHandModel.transform.localEulerAngles = _isInMouth? new (0, 0, 90) : new (0, 0, 0);

  /// <summary>
  /// exit from clearing the mouth and stop countdown
  /// </summary>
  public void OpenMouthExit () => _sceneStateMachine.SetActiveCountDown (false); //stop Count down

  public override CPRbaseStates GetKeyState () => CPRbaseStates.ClearAirWays;
}