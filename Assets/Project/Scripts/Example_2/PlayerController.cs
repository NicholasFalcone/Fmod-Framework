using UnityEngine;
using UnityEngine.Networking;

namespace NetworkingExample
{
    public class PlayerController : NetworkBehaviour
    {
        private MovementComponent m_movementComponent;
        private WeaponComponent m_weaponComponent;

        private void Awake()
        {
            m_movementComponent = GetComponent<MovementComponent>();
            m_weaponComponent = GetComponent<WeaponComponent>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!isLocalPlayer)
                return;

            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            Vector2 joyDirection = new Vector2(h, v);
            if (m_movementComponent == null)
            {
                Debug.LogError("Missing Movement");
            }
            m_movementComponent.Move(joyDirection);

            if (Input.GetMouseButtonDown(0))
            {
                m_weaponComponent.Fire();
            }
        }
    }
}