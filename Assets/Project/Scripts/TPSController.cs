using System.Collections;
using UnityEngine;

public class TPSController : MonoBehaviour {

    [SerializeField]
    private float m_rotationSpeed;
    [SerializeField]
    private float m_movementSpeed;
    private Animator m_animator;

    private Rigidbody m_rb;

    private Weapon m_Weapon;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody>();

        m_Weapon = GetComponentInChildren<Weapon>();
    }

	void Update ()
    {
        //Movemnt Input
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector2 _velocity = new Vector2(h, v);
        float rotationX = Input.GetAxisRaw("Mouse X");
        float rotationY = Input.GetAxisRaw("Mouse Y");
        //Update Movement Rotation

        Movement(_velocity);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_Weapon.Fire();
            //Add fireing animation
        }
    }

    private void Movement(Vector3 _velocity)
    {
        float magnitude = _velocity.magnitude;
        Debug.Log(magnitude);

        if (magnitude == 0)
            Debug.Log("Stopped");
        else
            Debug.Log("Moving");
        


        //Rooted movement?
    }

}
