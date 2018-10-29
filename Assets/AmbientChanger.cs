using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientChanger : MonoBehaviour
{

    private AmbientComponent m_AmbientComponent;

    [SerializeField]
    private SurfaceType m_AmbientType;

    void Awake()
    {
        m_AmbientComponent = FindObjectOfType<AmbientComponent>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            collider.GetComponent<MovementComponent>().SurfaceType = m_AmbientType;
            m_AmbientComponent.ChangeAmbientParameter((int)m_AmbientType, 1);
        }
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            m_AmbientComponent.ChangeAmbientParameter((int)m_AmbientType, 0);
        }
    }
}
