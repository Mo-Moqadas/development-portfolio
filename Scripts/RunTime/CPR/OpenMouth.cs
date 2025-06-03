using UnityEngine;

public class OpenMouth : MonoBehaviour {
  private void OnTriggerStay (Collider other) {

    if (other.CompareTag("FingerTip")) //the left hand index tip toe hands
      SceneStateMachine.Inctance.clearAirWays.OpenMouth ();
  }
  private void OnTriggerExit (Collider other) {

    if (other.CompareTag("FingerTip")) //the left hand index tip toe hands
      SceneStateMachine.Inctance.clearAirWays.OpenMouthExit ();
  }
}