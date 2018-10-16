using UnityEngine;
using FMOD.Studio;
using FMODUnity;
using System.Collections;

#region Base Structure
/// <summary>
/// Generic event with all reference than you need to play a simple sound
/// and mantain the reference
/// </summary>
[System.Serializable]
public class GenericEvent
{
    [EventRef]
    public string eventPath;
    public EventInstance fmodEvent;
}
/// <summary>
/// Generic event with reference at parameter of the event
/// </summary>
[System.Serializable]
public class GenericEventMonoParameter : GenericEvent
{
    public ParameterInstance eventParameter;
    public string parameterName;
}
/// <summary>
/// Generic event with reference at parameter[] of the event
/// </summary>
[System.Serializable]
public class GenericEventMultipleParameter : GenericEvent
{
    public ParameterInstance[] eventParameter;
    public string[] parameterName;
}
#endregion

public class FmodManager : StudioBankLoader
{
    public static FmodManager instance;

    #region Settings Variable
    public Settings settings;
    private Bus b_master;
    private Bus b_sfx;
    private Bus b_music;

    private string m_busMusic = "Music";
    private string m_busSFX = "Sound"; 
    
    #endregion

    #region Unity Method
    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
            SetBus();
            InitSoundSetting();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void OnApplicationQuit()
    {
        if (instance == this)
        {
            Debug.LogWarning("[FMOD Manager] All bank was Unloaded");
        }
    }

    #endregion

    #region SoundMethod
    /// <summary>
    /// Use to instance Generic event in game
    /// </summary>
    /// <param name="_genericEvent"></param>
    public void CreateGenericEnventInstance(ref GenericEvent _genericEvent)
    {
        _genericEvent.fmodEvent = RuntimeManager.CreateInstance(_genericEvent.eventPath);
    }

    /// <summary>
    /// Use to instance Generic event with parameter
    /// </summary>
    /// <param name="_genericEventInstance"></param>
    public void CreateGenericMonoBankParameterInstance(ref GenericEventMonoParameter _genericEventInstance)
    {
        _genericEventInstance.fmodEvent = RuntimeManager.CreateInstance(_genericEventInstance.eventPath);
        _genericEventInstance.fmodEvent.getParameter(_genericEventInstance.parameterName, out _genericEventInstance.eventParameter);
    }

    /// <summary>
    /// Use to instance Generic event with multiple parameter
    /// </summary>
    /// <param name="_genericEventInstance"></param>
    public void CreateGenericBankMultipleParameter(ref GenericEventMultipleParameter _genericEventInstance)
    {
        _genericEventInstance.fmodEvent = RuntimeManager.CreateInstance(_genericEventInstance.eventPath);
        //Set number of parameter
        _genericEventInstance.eventParameter = new ParameterInstance[_genericEventInstance.parameterName.Length];

        for (int i = 0; i < _genericEventInstance.parameterName.Length; i++)
        {
            GetParameterByCount(ref _genericEventInstance, i);
        }
    }

    /// <summary>
    /// Trigger a cue of event
    /// </summary>
    /// <param name="_genericEvent"></param>
    public void TriggerCue(GenericEvent _genericEvent)
    {
        _genericEvent.fmodEvent.triggerCue();
    }

    /// <summary>
    /// Get a parameter by index
    /// usefule for array of parameter
    /// </summary>
    /// <param name="_genericEvent">paramete</param>
    /// <param name="_index">index of paramete</param>
    public void GetParameterByCount(ref GenericEventMultipleParameter _genericEvent, int _index )
    {
        _genericEvent.fmodEvent.getParameterByIndex(_index, out _genericEvent.eventParameter[_index]);
    }

    public void StartBankFade(GenericEvent _genericEvent, float _speed)
    {
        if (_genericEvent == null)
            return;
        StartEvent(_genericEvent);
        StartCoroutine(C_LerpOverTime(_genericEvent.fmodEvent, _speed));
    }

    /// <summary>
    /// Start the current event bank
    /// </summary>
    /// <param name="_genericEvent"></param>
    public void StartEvent(GenericEvent _genericEvent)
    {
        if (_genericEvent.eventPath != "")
        {
            _genericEvent.fmodEvent.start();
        }
    }

    /// <summary>
    /// Stop the current event bank with the fade
    /// </summary>
    /// <param name="bank"></param>
    public void StopEventFade(EventInstance _eventInstance)
    {
        _eventInstance.stop(STOP_MODE.ALLOWFADEOUT);
    }

    /// <summary>
    /// Stop the current event without the fade
    /// </summary>
    /// <param name="bank"></param>
    public void StopEvent(EventInstance _eventInstance)
    {
        _eventInstance.stop(STOP_MODE.IMMEDIATE);
    }
   
    /// <summary>
    /// Stop all events
    /// </summary>
    public void StopAllEvents()
    {
        b_master.stopAllEvents(STOP_MODE.IMMEDIATE);
    }

    /// <summary>
    /// Change the volume value of a event
    /// </summary>
    /// <param name="_eventInstance"></param>
    /// <param name="_volume"></param>
    public void SetEventVolume(EventInstance _eventInstance, float _volume)
    {
        _eventInstance.setVolume(_volume);
    }

    /// <summary>
    /// Play a sound one shot on a current position
    /// </summary>
    /// <param name="_path">event path</param>
    /// <param name="_pos"> position emitter</param>
    public void PlaySoundOneShot(string _path, Vector3 _pos)
    {
        if (_path != "")
            RuntimeManager.PlayOneShot(_path, _pos);
        else
            Debug.LogWarning("Path dosen't found");
    }

    public void PlaySoundOneShot(string _path, GameObject _gameObject)
    {
        if (_path != "")
            RuntimeManager.PlayOneShotAttached(_path, _gameObject);
        else
            Debug.LogWarning("Path dosen't found");
    }

    /// <summary>
    /// Attach an event to current transform
    /// </summary>
    /// <param name="_eventInstance"></param>
    /// <param name="emitterTransform"></param>
    public void AttachSfx(EventInstance _eventInstance, Transform emitterTransform)
    {
        RuntimeManager.AttachInstanceToGameObject(_eventInstance, emitterTransform, GetComponent<Rigidbody2D>());
    }
    
    /// <summary>
    /// Change the event parameter with a new value
    /// </summary>
    /// <param name="_eventParameter"></param>
    /// <param name="value"></param>
    public void ChangeParameter(ref ParameterInstance _eventParameter, float value)
    {
        _eventParameter.setValue(value);
    }

    /// <summary>
    /// Lerp the event audio volume from 0 to 1 with a speed parameter
    /// </summary>
    /// <param name="_eventInstance"></param>
    /// <param name="_step"></param>
    /// <returns></returns>
    private IEnumerator C_LerpOverTime(EventInstance _eventInstance, float _step)
    {
        float elapsedTime = 0;
        const float startVolume = 0;
        float currentVolume = 0;
        const float targetVolume = 1;
        while (elapsedTime < _step)
        {
            //UnityEngine.Debug.LogError(currentVolume);
            currentVolume = (Mathf.Lerp(startVolume, targetVolume, (elapsedTime / _step)));
            _eventInstance.setVolume(currentVolume);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    /// <summary>
    /// Set Bus Value
    /// </summary>
    public void SetBus(Bus _bus, string _path)
    {
        _bus = RuntimeManager.GetBus("bus:/" + _path);
    }
    #endregion
}
