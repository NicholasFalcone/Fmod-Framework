using UnityEngine;

public class MovementComponent : MonoBehaviour
{
    [SerializeField]
    private float m_speed = 1;
    [SerializeField]
    private float m_jumpForce = 10;
    private Collider2D[] m_colliders;
    private Animator m_animator;
    private Rigidbody2D m_rigidbody;
    private SpriteRenderer m_render;
    [SerializeField]
    private LayerMask m_walkableLayer;

    private void Awake()
    {
        //Get all component
        m_animator = GetComponent<Animator>();
        m_render = GetComponent<SpriteRenderer>();
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

	// Update is called once per frame
	void Update ()
    {

    }

    public void Jump()
    {
        if (IsGrounded())
            m_rigidbody.AddForce(Vector2.up * m_jumpForce, ForceMode2D.Impulse);
    }

    public void Movement(float _direction)
    {
        if (_direction < 0)
            m_render.flipX = true;
        else
            m_render.flipX = false;

        m_rigidbody.velocity = new Vector2(_direction * m_speed, m_rigidbody.velocity.y);

        if (IsGrounded())
        {
            float magintude = m_rigidbody.velocity.magnitude;
            m_animator.SetFloat("Speed", magintude);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, 0.8f, m_walkableLayer);
    }

    public void PlayFootStep()
    {
        if(IsGrounded())
            Debug.Log("FootStep");
    }
}
