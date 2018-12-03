using UnityEngine;
using FMOD.Studio;
using FMODUnity;
using FMOD;
using System.Runtime.InteropServices;
using System;

[System.Serializable]
public class FmodEvent
{
    [EventRef]
    [SerializeField]
    private String m_eventPath;

    [SerializeField]
    private EventInstance m_fmodEventInstance;
    [SerializeField]
    private ParameterInstance[] m_parameterInstance;

    private bool m_hasCue = false;


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
        ///Check if has cue

        //Set number of parameter
        int _parameterCount;
        m_fmodEventInstance.getParameterCount(out _parameterCount);
        ///Riassigne the lenght of parameter
        m_parameterInstance = new ParameterInstance[_parameterCount];
        if (m_parameterInstance.Length == 0)
            return;
        ///Set parameter
        for (int i = 0; i < _parameterCount; i++)
        {
            m_fmodEventInstance.getParameterByIndex(i, out m_parameterInstance[i]);
        }
        ///Create the event
        m_fmodEventInstance = RuntimeManager.CreateInstance(m_eventPath);
        ///Create all parameters
    }

    public StringWrapper  GetParameterName(ParameterInstance instance) 
    {
        PARAMETER_DESCRIPTION desc = new PARAMETER_DESCRIPTION();
        instance.getDescription(out desc);
        return desc.name;
    }
}