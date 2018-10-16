using UnityEngine;

public class Brain : MonoBehaviour
{
    private TPSController m_tpsController;

    private void Awake()
    {
        m_tpsController = GetComponent<TPSController>();
    }

    private void Update()
    {
        UpdateInput();
    }

    private void UpdateInput()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector2 _velocity = new Vector2(h, v);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_tpsController.Fireing();
        }
    }

}
