using UnityEngine;

public class InputController : MonoBehaviour
{
    private MovementComponent m_movement = null;

    private void Awake()
    {
        m_movement = GetComponent<MovementComponent>();
    }

    private void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        float h = Input.GetAxisRaw(Globals.InputName.Horizontal);

        m_movement.Movement(h);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_movement.Jump();
        }
    }

}
