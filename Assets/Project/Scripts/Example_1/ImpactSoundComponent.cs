using UnityEngine;

public enum TargetMaterial
{
    Stone = 1,
    Glass,
    Sand,
    Metal,
}

public class ImpactSoundComponent : MonoBehaviour
{
    [SerializeField]
    private GenericEventMonoParameter m_hitSound;
    [SerializeField]
    private TargetMaterial m_targetMaterial;

    private void Start()
    {
        FmodManager.instance.CreateGenericMonoEventParameterInstance(ref m_hitSound);
        FmodManager.instance.ChangeParameter(ref m_hitSound.eventParameter, (int)m_targetMaterial);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Bullet"))
        {
            FmodManager.instance.AttachSfx(m_hitSound.fmodEvent, collision.gameObject.transform);
            FmodManager.instance.StartEvent(m_hitSound);
        }
    }
}
