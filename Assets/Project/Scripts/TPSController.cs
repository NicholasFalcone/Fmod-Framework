using UnityEngine;

public class TPSController : MonoBehaviour
{
    private Animator m_animator;
    private Rigidbody m_rb;
    private Weapon m_Weapon;
    [SerializeField]
    private float m_rotationSpeed;
    [SerializeField]
    private float m_movementSpeed;
    
    public Weapon Weapon { get { return m_Weapon; } }

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
        m_Weapon = GetComponentInChildren<Weapon>();
    }

    public void Fireing()
    {
        m_Weapon.Fire();
        //Add Animation
    }

    public void Movement(Vector3 _velocity)
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
