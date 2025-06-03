using System.Collections;
using Palapal.Shared;
using RTLTMPro;
using UnityEngine;

public class StatesUiManager : MonoBehaviour {
    public GameObject UIdataCanvasParent, QuizPanel, MassageCanvasUPDown, MassageDataCanvas;
    [Tooltip ("All help Panels")] public GameObject[] uiPanels;

    /// <summary>
    /// update position and rotation of the help banner canvas to the specified position of timer banner in each state
    /// </summary>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    public void SetDataBannerPosition (Vector3 position, Vector3 rotation) {
        UIdataCanvasParent.transform.position = position;
        UIdataCanvasParent.transform.localEulerAngles = rotation;
    }

    /// <summary>
    /// update distance of timer banner canvas to the specified distance 
    /// </summary>
    /// <param name="distance"></param>
    public void SetTimerCanvasDistance (float distance) => UIdataCanvasParent.transform.GetChild (0).GetComponent<Canvas> ().planeDistance = distance;

    /// <summary>
    /// show help banner for the specified state in the UI panels array
    /// </summary>
    /// <param name="index"></param>
    public void ShowHelpBanner (int index) {
        if (!SceneStateMachine.Inctance.IsTest)
            uiPanels[index].SetActive (true);
    }
    public void HideHelpBanner (int index) => uiPanels[index].SetActive (false);

    /// <summary>
    /// get massage banner game object for upadte
    /// </summary>
    /// <returns></returns>
    public GameObject GetUpDownBanner () => MassageCanvasUPDown.transform.GetChild (0).gameObject;

    /// <summary>
    /// show hint canvas to guide  help panel or question
    /// </summary>
    /// <param name="isTest"></param>
    public void ShowhintCanvas (bool isTest) {
         CameraOverlayDrawer.Instance.ShowToast(isTest ? "به سوال توجه کن!" : "به راهنما توجه کن!",2,0.5f,Color.white,Color.black);
    }
}