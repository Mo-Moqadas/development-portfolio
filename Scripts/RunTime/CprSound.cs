using HurricaneVR.Framework.Core.Utils;
using UnityEngine;

/// <summary>
/// the class that gets sound effects from the inspector and manages them.
/// </summary>
public class CprSound : MonoBehaviour {
    [SerializeField] private AudioClip Click, True, False, Success, Fail, Qusestion, Hint,BreathOut;
    public void PlaySound (string soundName) {
        switch (soundName.ToLower ()) {
            case "click":
                PlayClip (Click);
                break;
            case "true":
                PlayClip (True);
                break;
            case "false":
                PlayClip (False);
                break;
            case "success":
                PlayClip (Success);
                break;
            case "fail":
                PlayClip (Fail);
                break;
            case "qusetion":
                PlayClip (Qusestion);
                break;
            case "hint":
                PlayClip (Hint);
                break;
            case "breath_out":
                PlayClip (BreathOut);    
                break;
            default:
                break;
        }
    }
    protected void PlayClip (AudioClip clip) {
        if (SFXPlayer.Instance)
            SFXPlayer.Instance.PlaySFX (clip, transform.position);
    }
    
}