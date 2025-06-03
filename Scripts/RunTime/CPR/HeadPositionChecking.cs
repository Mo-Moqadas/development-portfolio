using UnityEngine;

public class HeadPositionChecking : MonoBehaviour {
    private Transform transformCam;
    private void OnTriggerEnter (Collider other) {
        if (other.CompareTag ("head"))
            transformCam = other.transform.parent; //get camera transform     
    }
    private void OnTriggerStay (Collider other) {
        if (other.CompareTag ("head")) {
            if (Physics.Raycast (transformCam.position, transformCam.forward, 2, LayerMask.GetMask ("Stomach"))) {
                SceneStateMachine.Inctance.CheckVain.HeadUpdate (true);
                transform.localScale = Vector3.one * 1.5f;
                transform.parent.GetChild (3).localScale = Vector3.one * 1.5f;
            } else {
                SceneStateMachine.Inctance.CheckVain.HeadUpdate (false);
                transform.localScale = Vector3.one;
                transform.parent.GetChild (3).localScale = Vector3.one;
            }
        }
    }
    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable () {
        transform.localScale = Vector3.one;
        transform.parent.GetChild (3).localScale = Vector3.one;
    }
}