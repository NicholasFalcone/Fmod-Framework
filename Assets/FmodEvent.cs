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

[System.Serializable]
public struct ParameterInfo
{
    [SerializeField]
    private string m_parameterName;
    [SerializeField]
    private float m_minIndex;
    [SerializeField]
    private float m_maxIndex;

    private float m_value;

    public string ParameterName   { get { return m_parameterName; } }
    public float MinIndex         { get { return m_minIndex; } }
    public float MaxIndex         { get { return m_maxIndex; } }
    public float Value            { get { return m_value; } }

    public ParameterInfo(string _parameterName, float _minParameter, float _maxParameter, float _currentVale)
    {
        m_parameterName =   _parameterName;
        m_minIndex      =   _minParameter;
        m_maxIndex      =   _maxParameter;
        m_value         =   _currentVale;
    }
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
    private ParameterInfo[] m_parameterInfo;

    public bool HasCue { get { return m_hasCue; } set { m_hasCue = false; } }
    public string EventPath { get { return m_eventPath; } set { m_eventPath = value; } }
    public EventInstance FmodEventInstance { get { return m_fmodEventInstance; } set { m_fmodEventInstance = value; } }
    public ParameterInstance[] ParamenterInstance { get { return m_parameterInstance; } set { m_parameterInstance = value; } }
    public ParameterInfo[] ParameterInfourn{ get { return m_parameterInfo; } }

    /// <summary>
    /// Called to initialize the fmod event
    /// </summary>
    public void InitFmodEvent()
    {
        ///Check if event path is different of null
        if (m_eventPath == null)
            UnityEngine.Debug.LogError("Event path not available");
        
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
        m_parameterInfo = new ParameterInfo[_parameterCount];
        if (m_parameterInstance.Length == 0)
            return;

        ///Set parameter
        for (int i = 0; i < _parameterCount; i++)
        {
            m_fmodEventInstance.getParameterByIndex(i, out m_parameterInstance[i]);
            m_parameterInfo[i] = GetParameterName(m_parameterInstance[i]);
        }
    }

    public void PlayAudio()
    {

    }


    #region Private-Method
    /// <summary>
    /// Get all important information of EventInstance
    /// </summary>
    /// <param name="eventInstance">current EvnentInstance</param>
    private void GetEventInfo(EventInstance eventInstance)
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

    /// <summary>
    /// Get all important information of ParameterInstance
    /// Return a parameterInfo with:
    /// -ParameterName
    /// -ParameterRange(start,end)
    /// </summary>
    /// <param name="instance">current ParameterInstance</param>
    /// <returns></returns>
    private ParameterInfo GetParameterName(ParameterInstance instance) 
    {
        ///Create the parameter description
        ///useflue to get all information
        PARAMETER_DESCRIPTION desc = new PARAMETER_DESCRIPTION();
        instance.getDescription(out desc);
        ParameterInfo parameterInfo = new ParameterInfo(desc.name, desc.minimum, desc.maximum, desc.defaultvalue);
        return parameterInfo;
    }
    #endregion

}