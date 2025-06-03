using UnityEngine;

public class MassageCall : MonoBehaviour {
  private void OnTriggerEnter (Collider other) {

    if (other.CompareTag("palm")) //the right hand 
      SceneStateMachine.Inctance.HaertMassageState.MassageCall (true);
  }
  private void OnTriggerExit (Collider other) {

    if (other.CompareTag("palm")) //the right hand 
      SceneStateMachine.Inctance.HaertMassageState.MassageCall (false);
  }
}