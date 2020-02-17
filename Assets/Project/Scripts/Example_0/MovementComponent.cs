using UnityEngine;
using CustomFMOD;

#region Enums
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
#endregion

public class MovementComponent : MonoBehaviour
{
    [SerializeField]
    private float m_speed = 1;
    [SerializeField]
    private float m_jumpForce = 10;
    [SerializeField]
    private LayerMask m_walkableLayer;
    private float landingDelta = 0;
    private const float landingThreshold = 0.1f;
    #region Sfx
    [Header("Sound")]
    [SerializeField]
    private FMODEventInstance m_footsteps;
    [SerializeField]
    private FMODEventInstance m_waterMovementSound;
    [SerializeField]
    private FMODEventInstance m_jumpStart;
    [SerializeField]
    private FMODEventInstance m_jumpEnd;
    #endregion
    private Collider2D[] m_colliders = null;
    private Animator m_animator = null;
    private Rigidbody2D m_rigidbody = null;
    private SpriteRenderer m_render = null;
    private bool m_onAir = false;
    [SerializeField]
    private SurfaceType m_surfaceType;
    private EventName m_eventName;

    public SurfaceType SurfaceType { get; set; }
    public EventName EventName { get { return m_eventName; } set { m_eventName = value; } }

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
        FMODDatabase.Instance.GetFmodEvent(m_footsteps);
        FMODDatabase.Instance.GetFmodEvent(m_jumpStart);
        FMODDatabase.Instance.GetFmodEvent(m_jumpEnd);
        FMODDatabase.Instance.GetFmodEvent(m_waterMovementSound, () =>
        {
            m_waterMovementSound.AttachTo(transform);
        });
    }


    private void Update()
    {
        if (m_onAir)
            Landing();
    }

    //Called on InputController
    public void Jump()
    {
        //check if is grounded and add force
        if (IsGrounded())
        {
            m_jumpStart.Play();
            //FmodManager.instance.StartEvent(m_jumpStart);
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
            m_animator.SetFloat(Globals.AnimationParameters.PlayerSpeed, magintude);
        }
    }

    private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.8f, m_walkableLayer);

        if (hit.transform == null)
            return false;
        else
        {
            if (hit.transform.gameObject.tag == Globals.Tags.Grass)
                m_surfaceType = SurfaceType.Grass;
            else if (hit.transform.gameObject.tag == Globals.Tags.Grass)
                m_surfaceType = SurfaceType.Cave;
            else
                m_surfaceType = SurfaceType.Water;
            return true;

        }
    }

    private void Landing()
    {
        landingDelta += Time.deltaTime;
        if ((IsGrounded() && m_onAir) && landingDelta > landingThreshold)
        {
            m_onAir = false;
            landingDelta = 0;
            m_jumpEnd.Play();
        }
    }

    public void CheckSurface(SurfaceType _surfaceType)
    {
        m_surfaceType = _surfaceType;
        if (m_surfaceType != SurfaceType.Water)
        {
            m_footsteps.ChangeParameter(m_footsteps.Parameters[0], (float)m_surfaceType++);
            m_jumpStart.ChangeParameter(m_footsteps.Parameters[0], (float)m_surfaceType++);
            m_jumpEnd.ChangeParameter(m_footsteps.Parameters[0], (float)m_surfaceType++);
        }
    }

    //Called each step
    public void PlayFootStep()
    {
        //Check if is grounded
        if (IsGrounded())
        {
            if (m_surfaceType == SurfaceType.Grass || m_surfaceType == SurfaceType.Cave)
            {
                m_footsteps.Play();
            }
            else
            {
                m_waterMovementSound.Play();
            }
        }
    }
}
