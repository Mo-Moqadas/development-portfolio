using UnityEngine;

public class SpineHingeChecker : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private new HingeJoint hingeJoint;
    private float _xAngle;

    void Start()
    {
        _xAngle = transform.eulerAngles.x;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("angle " + hingeJoint.angle);
        transform.eulerAngles = new Vector3(Mathf.Clamp(_xAngle - hingeJoint.angle, _xAngle, _xAngle + 15), transform.eulerAngles.y, transform.eulerAngles.z);
    }
}
