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
    private float m_maxYDegree = 45;

    private bool m_canUseWeapon;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    private int m_weaponIndex;
                                        //Worst stuff evenr done
    private int WeaponIndex { get { return m_weaponIndex > m_weapons.Length-1 ? m_weaponIndex = 0 : m_weaponIndex < 0 ? m_weaponIndex = m_weapons.Length - 1 : m_weaponIndex; } }

    private void Awake()
    {
        m_camera = Camera.main;
    }

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState  = CursorLockMode.Locked;
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
        yaw += m_speedH * Input.GetAxis("Mouse X");
        pitch -= m_speedV * Input.GetAxis("Mouse Y");
        transform.eulerAngles = new Vector3(pitch, yaw, 0);
        m_camera.transform.eulerAngles = new Vector3(Mathf.Clamp(pitch, -m_maxYDegree, m_maxYDegree), transform.rotation.y, 0.0f);
        m_camera.transform.localRotation = Quaternion.Euler(Mathf.Clamp(pitch, -m_maxYDegree, m_maxYDegree), transform.rotation.y, transform.rotation.z);
    }

}
