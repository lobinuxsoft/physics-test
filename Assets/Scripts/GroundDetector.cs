using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    int groundContactCount = 0;
    Vector3 contactNormal = Vector3.zero;

    public bool OnGround { get => groundContactCount > 0; }
    public Vector3 Normal { get => contactNormal; }

    [SerializeField, Range(0f, 90f)] float maxGroundAngle = 25f;
    float minGroundDotProduct = 0;

    void OnValidate()
    {
        minGroundDotProduct = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);
    }

    private void Awake()
    {
        OnValidate();
    }

    private void FixedUpdate()
    {
        contactNormal = OnGround ? contactNormal.normalized : Vector3.up;
        ClearState();
    }

    void ClearState()
    {
        groundContactCount = 0;
        contactNormal = Vector3.zero;
    }

    private void OnCollisionEnter(Collision collision)
    {
        EvaluateCollision(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        EvaluateCollision(collision);
    }

    private void EvaluateCollision(Collision collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector3 normal = collision.GetContact(i).normal;
            if (normal.y >= minGroundDotProduct)
            {
                groundContactCount++;
                contactNormal += normal;
            }
        }
    }
}
