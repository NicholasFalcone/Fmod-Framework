using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Bullet : MonoBehaviour
{
    public Rigidbody m_rb;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float m_bulletLifeTime = 3;
    private void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        m_rb.velocity = Vector3.zero;
        m_rb.angularVelocity =Vector3.zero;
        StartCoroutine(BulletLifeTime());

    }

    private void Update()
    {
        m_rb.AddForce(transform.up * speed,ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject particle;

        if (collision.gameObject.CompareTag("Stone"))
        {
            particle = PoolObject.GetObject(Globals.PoolKey.VFXStone);
            particle.transform.position = transform.position;
        }

        if (collision.gameObject.CompareTag("Metal"))
        {
            particle = PoolObject.GetObject(Globals.PoolKey.VFXMetal);
            particle.transform.position = transform.position;
            particle.transform.rotation = Quaternion.FromToRotation(Vector3.up, collision.contacts[0].normal); ;
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            particle = PoolObject.GetObject(Globals.PoolKey.VFXSend);
            particle.transform.position = transform.position;
        }

        if (collision.gameObject.CompareTag("Glass"))
        {
            TriangleExplosion explosion = collision.gameObject.GetComponent<TriangleExplosion>();
            explosion.StartCoroutine(explosion.SplitMesh(false));
        }
        StopCoroutine(BulletLifeTime());
        gameObject.SetActive(false);
    }

    IEnumerator BulletLifeTime()
    {
        yield return new WaitForSeconds(m_bulletLifeTime);
        gameObject.SetActive(false);
    }
}
