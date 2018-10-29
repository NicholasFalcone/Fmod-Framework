using UnityEngine;

public enum SurfaceType
{
    Ground,
    Wather
}

public class MovementComponent : MonoBehaviour
{
    [SerializeField]
    private float m_speed = 1;
    [SerializeField]
    private float m_jumpForce = 10;
    [SerializeField]
    private LayerMask m_walkableLayer;
    #region Sfx
    [Header("Sound")]
    [SerializeField]
    private GenericEventMonoParameter m_footsteps;
    #endregion
    private Collider2D[] m_colliders;
    private Animator m_animator;
    private Rigidbody2D m_rigidbody;
    private SpriteRenderer m_render;
    private bool m_onAir = false;
    private SurfaceType m_surfaceType;
    public SurfaceType SurfaceType { get; set; }

    private void Awake()
    {
        //Get all component
        m_animator = GetComponent<Animator>();
        m_render = GetComponent<SpriteRenderer>();
        m_rigidbody = GetComponent<Rigidbody2D>();
    }
    
    private void Start()
    {
        GenerateFmodEvents();
    } 
    
    private void GenerateFmodEvents()
    {
        FmodManager.instance.CreateGenericMonoEventParameterInstance(ref m_footsteps);
    }


    private void Update()
    {
        Landing();
    }

    //Called on InputController
    public void Jump()
    {
        //check if is grounded and add force
        if (IsGrounded())
        {
            m_rigidbody.AddForce(Vector2.up * m_jumpForce, ForceMode2D.Impulse);
        }
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

    private void Landing()
    {
        if (IsGrounded() && m_onAir)
            m_onAir = false;
        else
            m_onAir = true;

        if(IsGrounded() && m_onAir && m_surfaceType == SurfaceType.Ground)
        {
            //Play Landing Sound
        }
    }

    //Called each step
    public void PlayFootStep()
    {
        //Check if is grounded
        if(IsGrounded())
        {
            if(m_surfaceType == SurfaceType.Ground)
                FmodManager.instance.StartEvent(m_footsteps);
            else
            {
                //Play WatherMovement sound
            }

        }
    }
}
