using UnityEngine;

public class MouthCall : MonoBehaviour {

   private void OnTriggerStay (Collider other) {

      if (other.CompareTag ("head")) {
         SceneStateMachine.Inctance.BreathingMouthState.MouthBreathingCall (false);
      }
   }
   private void OnTriggerExit (Collider other) {

      if (other.CompareTag ("head")) {
         SceneStateMachine.Inctance.BreathingMouthState.MouthBreathingCall (true);
      }
   }

}