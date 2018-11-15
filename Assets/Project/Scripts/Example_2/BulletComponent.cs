using UnityEngine;
using UnityEngine.Networking;

namespace NetworkingExample
{
    public class BulletComponent : NetworkBehaviour
    {
        void OnCollisionEnter(Collision collision)
        {
            var hit = collision.gameObject;
            var health = hit.GetComponent<HealthComponent>();
            if (health != null)
            {
                health.TakeDamage(10);
            }
            Destroy(gameObject);
        }
    }
}