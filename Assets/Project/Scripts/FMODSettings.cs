using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class FMODSettings : MonoBehaviour
{
    private const string m_masterBusName = "Bus:/";
    private Bus m_masterBus;

    void Start()
    {
        m_masterBus = RuntimeManager.GetBus(m_masterBusName); 
    }

    public void SetBusValue(float _nextValue)
    {
        m_masterBus.setVolume(_nextValue);
    }
}
