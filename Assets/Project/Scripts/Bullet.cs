using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public Rigidbody2D m_rb;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
    }

    public void Init(Vector2 direction)
    {
        m_rb.AddForce(direction * speed,ForceMode2D.Impulse);
    }
}
