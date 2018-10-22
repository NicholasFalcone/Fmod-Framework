using UnityEngine;
using UnityEngine.Networking;

namespace NetworkingExample
{
    public class BulletComponent : NetworkBehaviour
    {
        private Rigidbody m_rb;
        [SerializeField]
        private float m_speed;

        private void Awake()
        {
            m_rb = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            m_rb.AddForce(Vector3.up * m_speed, ForceMode.Impulse);
        }
    }
}