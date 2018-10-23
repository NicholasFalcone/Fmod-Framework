using UnityEngine;
using UnityEngine.Networking;
namespace NetworkingExample
{
    public class WeaponComponent : NetworkBehaviour
    {
        [SerializeField]
        private GameObject m_bullet;
        [SerializeField]
        private Transform m_muzzle;
        
        public void Fire()
        {
            if (m_bullet == null)
            {
                Debug.LogError("Missing Bullet prefab");
                return;
            }

            Instantiate(m_bullet, m_muzzle.position, m_bullet.transform.rotation);
        }

    }
}