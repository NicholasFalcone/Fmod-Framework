using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientChanger : MonoBehaviour {

private AmbientComponent m_AmbientComponent;

[SerializeField]
private int m_AmbientType;

	void Awake()
	{
	m_AmbientComponent = FindObjectOfType <AmbientComponent>();

	}
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnTriggerEnter2D(Collider2D collider)
	{
		if(collider.tag=="Player")
		{
		m_AmbientComponent.ChangeAmbientParameter(m_AmbientType ,1);
		}
	}
	void OnTriggerExit2D(Collider2D collider)
	{
		if(collider.tag=="Player")
		{
		m_AmbientComponent.ChangeAmbientParameter(m_AmbientType, 0);
		}
	}
}
