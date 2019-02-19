
using UnityEngine;

public class AmbientChanger : MonoBehaviour
{
    private AmbientComponent m_AmbientComponent;
    [SerializeField]
    private GenericEvent m_hitSurfaceSound;
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

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (m_ambientType == SurfaceType.Water)
            FmodManager.instance.PlaySoundOneShot(m_hitSurfaceSound.eventPath, collider.transform.position);

        if (collider.CompareTag("Player"))
        {
            m_movementComponent.CheckSurface(m_ambientType);
            m_AmbientComponent.ChangeAmbientParameter((int)m_eventNameToChange, (int)m_EnterparameterValue);
        }
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            m_movementComponent.CheckSurface(m_exitType);
            m_AmbientComponent.ChangeAmbientParameter((int)m_eventNameToChange, (int)m_ExitparameterValue);

        }
    }
}
