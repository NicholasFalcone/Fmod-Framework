using UnityEngine;

public class InputController : MonoBehaviour
{
    private MovementComponent m_movement;

    private void Awake()
    {
        m_movement = GetComponent<MovementComponent>();
    }

    private void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");

        m_movement.Movement(h);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_movement.Jump();
        }

    }
}
