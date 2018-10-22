using UnityEngine;
using UnityEngine.Networking;

namespace NetworkingExample
{
    public class MovementComponent : NetworkBehaviour
    {
        private Rigidbody m_rb;
        [SerializeField]
        private float m_speed;

        private void Awake()
        {
            m_rb = GetComponent<Rigidbody>();
        }

        public void Move(Vector2 _inputDirection)
        {
            m_rb.velocity = new Vector3(_inputDirection.x * m_speed, 0, _inputDirection.y * m_speed);
        }

    }
}
