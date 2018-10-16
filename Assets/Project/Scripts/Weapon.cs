using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform m_muzzle;
    public GameObject m_bulletPrefab;

    public void Fire()
    {
        Bullet currentBullet = Instantiate(m_bulletPrefab, m_muzzle.position, Quaternion.identity).GetComponent<Bullet>();
    }
}
