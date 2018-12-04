using UnityEngine;
using FMOD.Studio;
using FMODUnity;
using FMOD;
using System;

public enum SoundType
{
    is2D,
    is3D
}


[CreateAssetMenu(menuName ="FmodEvent/NewEvent")]
public class FmodEvent : ScriptableObject
{
    [EventRef]
    [SerializeField]
    private String m_eventPath;

    [SerializeField]
    private EventInstance m_fmodEventInstance;
    [SerializeField]
    private ParameterInstance[] m_parameterInstance;
    [SerializeField]
    private bool m_hasCue = false;
    [SerializeField]
    private SoundType m_soundType;
    [SerializeField]
    private string[] m_parameterName;

    public bool HasCue { get { return m_hasCue; } set { m_hasCue = false; } }
    public string EventPath { get { return m_eventPath; } set { m_eventPath = value; } }
    public EventInstance FmodEventInstance { get { return m_fmodEventInstance; } set { m_fmodEventInstance = value; } }
    public ParameterInstance[] ParamenterInstance { get { return m_parameterInstance; } set { m_parameterInstance = value; } }


    /// <summary>
    /// Called to initialize the fmod event
    /// </summary>
    public void InitFmodEvent()
    {
        if(m_eventPath == null)
        {
            UnityEngine.Debug.LogError("Event path not available");
        }


        ///Create the event
        m_fmodEventInstance = RuntimeManager.CreateInstance(m_eventPath);
        ///Get event info: is3D, hasCue, exc...
        GetEventInfo(m_fmodEventInstance);
        ///Create all parameters
        ///and Set number of parameter
        int _parameterCount;
        m_fmodEventInstance.getParameterCount(out _parameterCount);
        ///Riassigne the lenght of parameter
        m_parameterInstance = new ParameterInstance[_parameterCount];
        ///Set Lenght of parameter name on inspector
        m_parameterName = new string[_parameterCount];
        if (m_parameterInstance.Length == 0)
            return;
        ///Set parameter
        for (int i = 0; i < _parameterCount; i++)
        {
            m_fmodEventInstance.getParameterByIndex(i, out m_parameterInstance[i]);
            m_parameterName[i] = (string)GetParameterName(m_parameterInstance[i]);
        }
    }

    public void GetEventInfo(EventInstance eventInstance)
    {
        EventDescription eventDescription = new EventDescription();
        eventInstance.getDescription(out eventDescription);
        eventDescription.hasCue(out m_hasCue);
        bool _is3D = false;
        eventDescription.is3D(out _is3D);
        if (_is3D)
            m_soundType = SoundType.is3D;
        else
            m_soundType = SoundType.is2D; 
    }

    public StringWrapper  GetParameterName(ParameterInstance instance) 
    {
        PARAMETER_DESCRIPTION desc = new PARAMETER_DESCRIPTION();
        instance.getDescription(out desc);
        return desc.name;
    }
}