using UnityEngine;
using CustomFMOD;

public class AmbientChanger : MonoBehaviour
{
    [SerializeField]
    private FMODEventInstance m_hitSurfaceSound;
    private AmbientComponent m_AmbientComponent;
    private MovementComponent m_movementComponent;
    [SerializeField]
    private EventName m_eventNameToChange;
    [SerializeField]
    private SurfaceType m_ambientType;
    [SerializeField]
    private SurfaceType m_exitType = SurfaceType.Grass;
    [SerializeField]
    private ParameterValue m_EnterparameterValue;
    [SerializeField]
    private ParameterValue m_ExitparameterValue;
    void Awake()
    {
        m_AmbientComponent = FindObjectOfType<AmbientComponent>();
        m_movementComponent = FindObjectOfType<MovementComponent>();
    }

    private void Start()
    {
        ///create a request to get fmod event
        FMODDatabase.Instance.GetFmodEvent(m_hitSurfaceSound);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (m_ambientType == SurfaceType.Water)
        {
            ///play event
            m_hitSurfaceSound.Play();
            m_hitSurfaceSound.AttachTo(collider.transform);
        }

        if (collider.CompareTag(Globals.Tags.Player))
        {
            m_movementComponent.CheckSurface(m_ambientType);
            m_AmbientComponent.ChangeAmbientParameter((int)m_eventNameToChange, (int)m_EnterparameterValue);
        }
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag(Globals.Tags.Player))
        {
            m_movementComponent.CheckSurface(m_exitType);
            m_AmbientComponent.ChangeAmbientParameter((int)m_eventNameToChange, (int)m_ExitparameterValue);
        }
    }
}
