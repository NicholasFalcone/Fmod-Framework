using UnityEngine;

namespace CustomFMOD
{
    public class FMODSoundEmitter : MonoBehaviour
    {
        [SerializeField]
        private FMODEventInstance m_eventInstance;

        private void Start()
        {
            FMODDatabase.Instance.GetFmodEvent(m_eventInstance);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                PlayEvent();
            }
        }

        private void PlayEvent()
        {
            Debug.Log(m_eventInstance.Play());
        }

    }
}
