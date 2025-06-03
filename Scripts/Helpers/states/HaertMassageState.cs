using Palapal.Shared;
using RTLTMPro;
using UnityEngine;

public class HaertMassageState : CPRbaseState
{
  private const float distance_limit = .22f;
  public static int massage_limit = 30;
  public static int massage_round = 8;
  private GameObject _leftHand, _rightHandExtra, _massageBannerUpDown, _massageDataBanner;
  private int massageCounter;
  private bool isHitThehead, isSendMassageScore, isUpdate;
  private Transform _ExtraHandPhysicParent, _rightHandModel;
  private int layerMask;
  private SceneStateMachine _sceneStateMachine;
  // readonly public Vector3 targetposition = new(11.4f, 11.6f, 5.75f);
  // readonly public Vector3 targetRotation = new(0, 30, 0);

  public override void EnterState(SceneStateMachine sceneMachine)
  {
    sceneMachine.SetCurentStateBasics(5);
    sceneMachine.SetTimer(60);
    // sceneMachine.statesUiManager.SetDataBannerPosition(targetposition, targetRotation);

    HandSetUpForTwoHandPose(sceneMachine);
    SetupMassageBanner(sceneMachine);

    layerMask = LayerMask.GetMask("head");
    isSendMassageScore = true;
    isUpdate = true;
    _sceneStateMachine = sceneMachine;
  }

  private void SetupMassageBanner(SceneStateMachine sceneMachine)
  {
    _massageBannerUpDown = sceneMachine.statesUiManager.GetUpDownBanner();
    _massageDataBanner = sceneMachine.statesUiManager.MassageDataCanvas;

    _massageDataBanner.SetActive(true);
    _massageBannerUpDown.SetActive(true);
    massageCounter = 0;
   // _massageBannerUpDown.transform.GetChild(5).GetComponent<RTLTextMeshPro>().text = massageCounter.ToString() + "/30";
    _massageDataBanner.transform.GetChild(1).GetChild(3).GetComponent<RTLTextMeshPro>().text = massageCounter + "/30";
  }

  private void HandSetUpForTwoHandPose(SceneStateMachine sceneMachine)
  {
    _leftHand = sceneMachine.leftHandModel;
    _rightHandExtra = sceneMachine.LefthandModelExtra;
    //as left hand extra model is in hiarchy of right hand physic 
    _ExtraHandPhysicParent = _rightHandExtra.transform.parent.parent;
    _rightHandModel = _rightHandExtra.transform.parent.GetChild(0);
  }

  public override void UpdateState(SceneStateMachine sceneMachine)
  {
    if (isUpdate)
    {
      float distance = Vector3.Distance(_leftHand.transform.position, _rightHandExtra.transform.position);

      if (distance < distance_limit)
      {
        HandOver(true);
        SendRaycastFromHand();
      }
      else
        HandOver(false);
    }
  }
  public override void ExitState(SceneStateMachine sceneMachine)
  {
    sceneMachine.statesUiManager.HideHelpBanner(5);
    //activate massage two hint shadow hands again for other rounds
    sceneMachine.StateGrabBoxGameObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
    sceneMachine.HideShadowHands();
    sceneMachine.SetActiveCountDown(false);

    //disable hand over situation and the up down data
    HandOver(false);
    _massageBannerUpDown.transform.GetChild(1).gameObject.SetActive(false);
    _massageBannerUpDown.transform.GetChild(0).gameObject.SetActive(false);
    _massageDataBanner.SetActive(false);
    sceneMachine.TapCounter.SetMassagingTime(false);
  }
  private void CheckTOUpdateState(SceneStateMachine sceneMachine)
  {
    if (massageCounter == massage_limit)
    {
      if (isSendMassageScore)
      {
        sceneMachine.ScoreCalculator.UpdateMassageCalculator(60 - sceneMachine.CountdownTimer.GetCurentTime());
        isSendMassageScore = false;
        sceneMachine.CprSessionRepeatTime += 1;
        sceneMachine.UpdateRepeat();
      }
      isUpdate = false;

      sceneMachine.SwitchState(sceneMachine.CheckVain, 0); //almost no delay
    }
  }

  private void SendRaycastFromHand()
  {
    //check distance  up to 10 units and in layer 
    //we use project magnitude to make sure the hands are toward up not only straight to head
    if (Vector3.Project(_ExtraHandPhysicParent.right, Vector3.up).magnitude > 0.8 &&
      Physics.Raycast(_ExtraHandPhysicParent.position, _ExtraHandPhysicParent.right, out RaycastHit hit, 100, layerMask))
    {
      if (hit.collider.CompareTag("head"))
        isHitThehead = true;
    }
    else
      isHitThehead = false;
  }

  private void HandOver(bool _IsOver)
  {
    if (_IsOver)
    {
      _leftHand.SetActive(false);
      _rightHandExtra.SetActive(true);
      _rightHandModel.transform.localEulerAngles = new(0, 30, 0);
    }
    else
    {
      _leftHand.SetActive(true);
      _rightHandExtra.SetActive(false);
      _rightHandModel.transform.localEulerAngles = new(0, 0, 0);
    }
  }

  /// <summary>
  /// heart massage call in haert massage state to check is down or up
  /// </summary>
  /// <param name="isdown"></param>
  public void MassageCall(bool Isdown)
  {
    ShowMassageSituatuin(Isdown);
    OnPushDownHaertMassage(Isdown);
  }
  private void OnPushDownHaertMassage(bool Isdown)
  {
    if (Isdown)
    {
      _massageDataBanner.transform.GetChild(0).gameObject.SetActive(true);
      if (_rightHandExtra.activeSelf)
      {
        // Hide only hint Hands;
        _sceneStateMachine.StateGrabBoxGameObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(false); //hide massage two hint shadow hands
        _sceneStateMachine.SetActiveCountDown(true);
        massageCounter += 1;
       // _massageBannerUpDown.transform.GetChild(5).GetComponent<RTLTextMeshPro>().text = massageCounter + "/30";
        _massageDataBanner.transform.GetChild(1).GetChild(3).GetComponent<RTLTextMeshPro>().text = massageCounter+ "/30";

        if (!isHitThehead)
        {
          _sceneStateMachine.ScoreCalculator.UpdateWrongMassage();
          ShowWrongHandMessage(); //show hint text to straigt hands
        }
        _sceneStateMachine.TapCounter.SetMassagingTime(true);
      }
    }
    else
    {
      _sceneStateMachine.StateGrabBoxGameObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
      CheckTOUpdateState(_sceneStateMachine);
    }
  }

  private void ShowMassageSituatuin(bool _isdown)
  {
    if (_isdown)
    {
      //up down arrow signs location
      _massageBannerUpDown.transform.GetChild(0).gameObject.SetActive(true); //up sign
      _massageBannerUpDown.transform.GetChild(1).gameObject.SetActive(false); //down sign
    }
    else
    {
      _massageBannerUpDown.transform.GetChild(0).gameObject.SetActive(false);
      _massageBannerUpDown.transform.GetChild(1).gameObject.SetActive(true);
    }
  }
  private void ShowWrongHandMessage()
  {
    CameraOverlayDrawer.Instance.ShowToast("خطا : دست ها در راستای سر نیست !", 2, 0.5f, Color.red, Color.black);
  }
  public override CPRbaseStates GetKeyState() => CPRbaseStates.HaertMassageState;
}