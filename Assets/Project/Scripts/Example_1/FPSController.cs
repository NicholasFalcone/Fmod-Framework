using UnityEngine;

public class FPSController : MonoBehaviour
{
    [SerializeField]
    private Weapon[] m_weapons;
    [SerializeField]
    private Weapon m_equippedWeapon;
    private Camera m_camera;
    [SerializeField]
    private float m_speedH = 2.0f;
    [SerializeField]
    private float m_speedV = 2.0f;
    [SerializeField]
    private float m_maxYDegree = 45f;
    [SerializeField]
    private float m_maxXDegree = 60f;
    private bool m_canUseWeapon;

    private float m_yaw = 0.0f;
    private float m_pitch = 0.0f;

    private int m_weaponIndex;
                                        //Worst stuff evenr done
    private int WeaponIndex { get { return m_weaponIndex > m_weapons.Length-1 ? m_weaponIndex = 0 : m_weaponIndex < 0 ? m_weaponIndex = m_weapons.Length - 1 : m_weaponIndex; } }

    private void Awake()
    {
        m_camera = Camera.main;
    }

    private void Start()
    {
        //disable cursor and locked on the middle of the screen
        Cursor.visible = false;
        Cursor.lockState  = CursorLockMode.Locked;
        //take a next weapon
        ChangeWeapon(1);
    }

    private void Update()
    {
        if (!m_canUseWeapon)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            m_equippedWeapon.Fire();
        }
        if(Input.GetAxisRaw("Mouse ScrollWheel") > 0)
        {
            ChangeWeapon(1);
        }
        if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
        {
            ChangeWeapon(-1);
        }
    }

    /// <summary>
    /// Change Weapon with a positve or negative index
    /// </summary>
    /// <param name="index"></param>
    private void ChangeWeapon(int index)
    {
        m_canUseWeapon = false;
        Debug.Log("ChangingWeapon " + index);
        if(m_equippedWeapon != null)
            m_equippedWeapon.OnUnEquip();
        m_weaponIndex += index;
        m_equippedWeapon = m_weapons[WeaponIndex];
        m_equippedWeapon.OnEquip();
        m_canUseWeapon = true;
    }

    private void LateUpdate()
    {
        //Take input from mouse
        m_yaw += m_speedH * Input.GetAxis("Mouse X");
        m_pitch -= m_speedV * Input.GetAxis("Mouse Y");
        //Rotate Player clamped for max X and max Y
        transform.eulerAngles = new Vector3(Mathf.Clamp(m_pitch, -m_maxYDegree, m_maxYDegree),Mathf.Clamp(m_yaw,-m_maxXDegree,m_maxXDegree), 0.0f);
        //Rotate camera
        m_camera.transform.eulerAngles = new Vector3(Mathf.Clamp(m_pitch, -m_maxYDegree, m_maxYDegree), transform.rotation.y, 0.0f);
        m_camera.transform.localRotation = Quaternion.Euler(Mathf.Clamp(m_pitch, -m_maxYDegree, m_maxYDegree), transform.rotation.y, transform.rotation.z);
    }

}
