using UnityEngine;

public enum TargetMaterial
{
    Stone = 0,
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
        //Add new position by contactPoint
        FmodManager.instance.StartEvent(m_hitSound);
    }
}
