using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public Rigidbody m_rb;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        m_rb.AddForce(transform.up * speed,ForceMode.Impulse);

    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.CompareTag("Wall"))
        //{
        //    Debug.Log("Collide with Wall");
        //}
        //if (collision.gameObject.CompareTag("Metal"))
        //{
        //    Debug.Log("Metal");
        //}

        if(collision.gameObject.CompareTag("Glass"))
        {
            TriangleExplosion explosion = collision.gameObject.GetComponent<TriangleExplosion>();
            explosion.StartCoroutine(explosion.SplitMesh(false));
        }

        gameObject.SetActive(false);
    }

}
