using UnityEngine;

public enum SurfaceType
{
    Grass = 0,
    Water,
    Cave
}

public enum EventName
{
    ambience,
    underwater
}

public enum ParameterValue
{
    Zero = 0,
    One,
    Two,
    Three
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
    [SerializeField]
    private GenericEvent m_watherMovementSound;
    [SerializeField]
    private GenericEventMonoParameter m_jumpStart;
    [SerializeField]
    private GenericEventMonoParameter m_jumpEnd;
    #endregion
    private Collider2D[] m_colliders;
    private Animator m_animator;
    private Rigidbody2D m_rigidbody;
    private SpriteRenderer m_render;
    private bool m_onAir = false;
    [SerializeField]
    private SurfaceType m_surfaceType;
    private EventName m_eventName;

    public SurfaceType SurfaceType { get; set; }
    public EventName EventName{get{return m_eventName;} set{m_eventName = value;}}
    
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
        m_surfaceType = SurfaceType.Grass;
    } 
    
    private void GenerateFmodEvents()
    {
        FmodManager.instance.CreateGenericMonoEventParameterInstance(ref m_footsteps);
        FmodManager.instance.CreateGenericMonoEventParameterInstance(ref m_jumpStart);
        FmodManager.instance.CreateGenericMonoEventParameterInstance(ref m_jumpEnd);
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
            FmodManager.instance.StartEvent(m_jumpStart);
            m_onAir = true;
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
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.8f, m_walkableLayer);

        if (hit.transform == null)
            return false;
        else
        {
            if (hit.transform.gameObject.tag == "Grass")
                m_surfaceType = SurfaceType.Grass;
            else if(hit.transform.gameObject.tag == "Cave")
                m_surfaceType = SurfaceType.Cave;
            else
                m_surfaceType = SurfaceType.Water;
            return true;

        }
    }

    float _delta = 0;
    private void Landing()
    {
       
        if (m_onAir)
        {
            _delta += Time.deltaTime;
        }

        if ((IsGrounded() && m_onAir) && _delta > 0.1f)
        {
            m_onAir = false;
            _delta = 0;
            FmodManager.instance.StartEvent(m_jumpEnd);
        }
    }

    public void CheckSurface(SurfaceType _surfaceType)
    {
        m_surfaceType = _surfaceType;
        if (m_surfaceType != SurfaceType.Water)
        {
            FmodManager.instance.ChangeParameter(ref m_footsteps.eventParameter, (float)m_surfaceType++);
            FmodManager.instance.ChangeParameter(ref m_jumpStart.eventParameter, (float)m_surfaceType++);
            FmodManager.instance.ChangeParameter(ref m_jumpEnd.eventParameter, (float)m_surfaceType++);
        }
    }

    //Called each step
    public void PlayFootStep()
    {
        //Check if is grounded
        if(IsGrounded())
        {
            if (m_surfaceType == SurfaceType.Grass || m_surfaceType == SurfaceType.Cave)
            {
                FmodManager.instance.StartEvent(m_footsteps);
            }
            else
            {
                Debug.Log("Play Wather");
                FmodManager.instance.PlaySoundOneShot(m_watherMovementSound.eventPath, transform.position);
            }

        }
    }
}
