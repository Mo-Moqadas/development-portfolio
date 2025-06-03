using UnityEngine;

public class CalButem : MonoBehaviour
{
   private void OnTriggerStay(Collider other) {
    if(other.CompareTag("FingerTip"))//the left hand index tip toe hands
    SceneStateMachine.Inctance.CallHelpState.Phonecall();
   }
   private void OnTriggerExit(Collider other) {
    if(other.CompareTag("FingerTip"))//the left hand index tip toe hands
    SceneStateMachine.Inctance.CallHelpState.PhonecallExit();
   }
}
