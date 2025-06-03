using RTLTMPro;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

/// <summary>
/// this ui class for start scene, handle user selection between train and test mode and transition to next scene.
/// </summary>
public class UiMaanger : MonoBehaviour {
    public GameObject PanelScenario, PanelScenarioTrainInfo, PanelScenarioTestInfo;
    [Tooltip ("the scriptable that save user setting data")] public UserCprData userCprData;
    [SerializeField] private RTLTextMeshPro _usercprDataText;

    private CprSound _cprSound;
    public void OnbtnTrainSelect () {
        //cprSound.PlaySound("click");
        PanelScenario.SetActive (false);
        PanelScenarioTrainInfo.SetActive (true);
        userCprData.IsTesting = false;
    }
    public void OnbtnTestSelect () {
        // cprSound.PlaySound("click");
        PanelScenario.SetActive (false);
        PanelScenarioTestInfo.SetActive (true);
        userCprData.IsTesting = true;
    }
    public void OnbtnTtrainTestContinueSelected () {
        // cprSound.PlaySound("click");
        SceneManager.LoadSceneAsync ("FireStationScene");
    }
    public void OnbtnAppTest () {
        userCprData.MassageLimit = 6;
        userCprData.MassageRound = 4;
        UpdateUserCprText ();

    }
    public void OnbtnAppOrigin () {
        userCprData.MassageLimit = 30;
        userCprData.MassageRound = 8;
        UpdateUserCprText ();

    }

    private void UpdateUserCprText() => _usercprDataText.text = "massage limit: " + userCprData.MassageLimit +"\n"+ " massage round: " + userCprData.MassageRound;
}