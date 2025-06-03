using UnityEngine;

public class VainTouched : MonoBehaviour {
      private void OnTriggerEnter (Collider other) {
            if (other.CompareTag("FingerTip")) //the left hand index tip toe hands           
                  SceneStateMachine.Inctance.CheckVain.VainTouchUpdate (true);
      }
      private void OnTriggerExit (Collider other) {
            if (other.CompareTag("FingerTip")) //the left hand index tip toe hands
                  SceneStateMachine.Inctance.CheckVain.VainTouchUpdate (false);
      }

}