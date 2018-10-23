using UnityEngine;
using UnityEngine.Networking;

namespace NetworkingExample
{
    public class MovementComponent : NetworkBehaviour
    {
        private Rigidbody m_rb;
        [SerializeField]
        private float m_movementSpeed;
        private float m_yaw;
        [SerializeField]
        private float m_rotationSpeed;

        private void Awake()
        {
            m_rb = GetComponent<Rigidbody>();
        }

        public void Move(Vector2 direction)
        {
            m_rb.velocity = new Vector3(direction.x * m_movementSpeed, 0, direction.y * m_movementSpeed);
            Rotate();
        }

        public void Rotate()
        {
            m_yaw += m_rotationSpeed * Input.GetAxis("Mouse X");
            Vector3 direction = new Vector3(0, m_yaw, 0);
            m_rb.rotation = Quaternion.Euler(direction);
        }

        //public void Move(Vector2 _inputDirection)
        //{
        //    m_rb.velocity = new Vector3(_inputDirection.x * m_movementSpeed, 0, _inputDirection.y * m_movementSpeed);
        //}

        //public void Rotate()
        //{
        //    ///Take input from mouse
        //    m_yaw += m_rotationSpeed * Input.GetAxis("Mouse X");
        //    ///Set Rotation
        //    transform.eulerAngles = new Vector3(0, m_yaw, 0.0f);
        //}
    }
}
