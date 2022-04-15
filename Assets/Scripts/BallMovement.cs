using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(GroundDetector))]

public class BallMovement : MonoBehaviour
{
    [SerializeField] private float torqueSpeed = 20;
    [SerializeField] private float jumpSpeed = 6;
    [SerializeField] private float maxAngularVelocity = 180;
    private Rigidbody body;
    private GroundDetector gDetector;
    private Camera cam;

    private Vector3 inputDir = Vector3.zero;
    
    // Start is called before the first frame update
    void Awake()
    {
        body = GetComponent<Rigidbody>();
        
        gDetector = GetComponent<GroundDetector>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 forward = cam.transform.forward;
        forward.y = 0;
        forward.Normalize();
        
        Vector3 right = cam.transform.right;
        right.y = 0;
        right.Normalize();

        inputDir = Vector3.ClampMagnitude(forward * -Input.GetAxis("Horizontal") + right * Input.GetAxis("Vertical"), 1f);
        
        if (gDetector.OnGround && Input.GetButtonDown("Jump"))
        {
            body.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
        }
    }

    private void FixedUpdate()
    {
        body.angularDrag = maxAngularVelocity * .25f;
        body.maxAngularVelocity = maxAngularVelocity;
        
        body.AddTorque( torqueSpeed * inputDir, ForceMode.VelocityChange);
    }
}
