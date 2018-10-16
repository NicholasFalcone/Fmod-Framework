using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public Rigidbody m_rb;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    public void Init(Vector3 direction)
    {
        m_rb.AddForce(direction * speed,ForceMode.Impulse);
    }
}
