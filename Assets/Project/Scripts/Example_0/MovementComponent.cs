using UnityEngine;

public class MovementComponent : MonoBehaviour
{
    [SerializeField]
    private float m_speed = 1;
    [SerializeField]
    private float m_jumpForce = 10;
    [SerializeField]
    private LayerMask m_walkableLayer;
    [SerializeField]
    private GenericEventMonoParameter  m_footsteps;
    private Collider2D[] m_colliders;
    private Animator m_animator;
    private Rigidbody2D m_rigidbody;
    private SpriteRenderer m_render;
    
    private void Awake()
    {
        //Get all component
        m_animator = GetComponent<Animator>();
        m_render = GetComponent<SpriteRenderer>();
        m_rigidbody = GetComponent<Rigidbody2D>();
    }
    
    private void Start()
    {
        FmodManager.instance.CreateGenericMonoBankParameterInstance(ref m_footsteps);
    } 
    
    //Called on InputController
    public void Jump()
    {
        //check if is grounded and add force
        if (IsGrounded())
            m_rigidbody.AddForce(Vector2.up * m_jumpForce, ForceMode2D.Impulse);
    }

    public void Movement(float _direction)
    {
        //Set the direction by input
        if (_direction < 0)
            m_render.flipX = true;
        else
            m_render.flipX = false;

        //Set the velocity x with a new value by inputController 
        m_rigidbody.velocity = new Vector2(_direction * m_speed, m_rigidbody.velocity.y);

        //if is grounded check the magnitude of velocity
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

    //Called each step
    public void PlayFootStep()
    {
        //Check if is grounded
        if(IsGrounded())
        {
           FmodManager.instance.StartEvent(m_footsteps);
        }
    }
}
