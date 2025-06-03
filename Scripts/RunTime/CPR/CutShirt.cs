using UnityEngine;

public class CutShirt : MonoBehaviour {
    private void OnTriggerStay (Collider other) {

        if (other.CompareTag ("FingerTip")) //the right hand index tip toe hands
            SceneStateMachine.Inctance.CutTHeShirt.ChangeShirt ();
    }
     private void OnTriggerExit (Collider other) {

        if (other.CompareTag ("FingerTip")) //the right hand index tip toe hands
            SceneStateMachine.Inctance.CutTHeShirt.ChangeShirtExit ();
    }
}