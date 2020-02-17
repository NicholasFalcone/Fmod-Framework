using UnityEngine;
using CustomFMOD;

public class AmbientComponent : MonoBehaviour {

	[SerializeField]
	private FMODEventInstance m_SoundAmbience;
	// Use this for initialization
	void Start ()
	{
        FMODDatabase.Instance.GetFmodEvent(m_SoundAmbience, ()=> {
            m_SoundAmbience.Play();
        });
	}
	
	public void ChangeAmbientParameter(int index, float nextValue)
	{
        m_SoundAmbience.ChangeParameter(m_SoundAmbience.Parameters[index], nextValue);
	}
}
