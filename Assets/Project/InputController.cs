using UnityEngine;

public class InputController : MonoBehaviour
{
    private MovementComponent m_movement;

    private Weapon m_weapon;

    private void Awake()
    {
        m_movement = GetComponent<MovementComponent>();
        m_weapon = GetComponent<Weapon>();
    }

    private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");

        m_movement.Movement(h);

        if (Input.GetKeyDown(KeyCode.Space))
            m_movement.Jump();


        if (Input.GetKeyDown(KeyCode.C) && m_weapon != null)
            m_weapon.Fire();

    }



}
