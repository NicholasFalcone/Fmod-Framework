using UnityEngine;
using CustomFMOD;

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
    private FMODEventInstance m_hitSound;
    [SerializeField]
    private TargetMaterial m_targetMaterial;

    private void Start()
    {
        FMODDatabase.Instance.GetFmodEvent(m_hitSound, () =>
        {
            m_hitSound.AttachTo(transform);
            m_hitSound.ChangeParameter(m_hitSound.Parameters[0], (int)m_targetMaterial);
        });

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            m_hitSound.Play();
        }
    }
}
