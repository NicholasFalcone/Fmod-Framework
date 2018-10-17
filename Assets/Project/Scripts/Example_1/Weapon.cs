using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private Transform m_muzzle;
    [SerializeField]
    private GameObject m_bulletPrefab;
    [SerializeField]
    private int m_bullets;
    [SerializeField]
    private float m_rate;

    public void OnEquip()
    {
        //Load Pool and Stuff
        gameObject.SetActive(true);

    }

    public void OnUnEquip()
    {
        //Release Event and stuff
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Called on FPSController
    /// </summary>
    public void Fire()
    {
        GameObject currentBullet = Instantiate(m_bulletPrefab, m_muzzle.position, m_muzzle.rotation);
    }
}
