using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSController : MonoBehaviour {
    [SerializeField]
    private float m_rotationSpeed;
    [SerializeField]
    private float m_movementSpeed;
    private Animator m_animator;
    private Rigidbody m_rigidbody;
    Transform camera;
    private void Awake()
    {
        camera = Camera.main.transform;

        m_rigidbody = GetComponent<Rigidbody>();
    }

	// Update is called once per frame
	void Update ()
    {
        //Movemnt Input
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector2 _velocity = new Vector2(h, v);
        float rotationX = Input.GetAxisRaw("Mouse X");
        float rotationY = Input.GetAxisRaw("Mouse Y");
        Vector3 _rotation = new Vector3(rotationY, rotationX,0);
        transform.rotation = Camera.main.transform.rotation;
        //Update Movement Rotation
        Movement(_velocity);
    }

    private void Movement(Vector3 _velocity)
    {
        m_rigidbody.velocity = new Vector3(_velocity.x, m_rigidbody.velocity.y, _velocity.y);
    }

}
