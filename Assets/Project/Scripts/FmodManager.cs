using UnityEngine;
using FMOD.Studio;
using FMODUnity;
using System.Collections;

#region Base Structure

[System.Serializable]
public class FmodEvent
{
    public string path;
    public EventInstance eventInstance;
    public ParameterInstance[] parameters;
    public string[] parameterName;
}

/// <summary>
/// Generic bank with all reference than you need to play a simple sound
/// </summary>
[System.Serializable]
public class GenericEvent
{
    [EventRef]
    public string eventPath;
    public EventInstance fmodEvent;
}
/// <summary>
/// Generic bank with reference at parameter of the event
/// </summary>
[System.Serializable]
public class GenericEventMonoParameter : GenericEvent
{
    public ParameterInstance eventParameter;
    public string parameterName;
}
/// <summary>
/// Generic bank with reference at parameter[] of the event
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
            GetBus();
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
    /// Use to instance Generic bank in game
    /// </summary>
    /// <param name="genericEvent"></param>
    public void CreateGenericEnventInstance(ref GenericEvent genericEvent)
    {
        genericEvent.fmodEvent = RuntimeManager.CreateInstance(genericEvent.eventPath);
    }

    /// <summary>
    /// Use to instance Generic bank with parameter
    /// </summary>
    /// <param name="bank"></param>
    public void CreateGenericMonoBankParameterInstance(ref GenericEventMonoParameter bank)
    {
        bank.fmodEvent = RuntimeManager.CreateInstance(bank.eventPath);
        bank.fmodEvent.getParameter(bank.parameterName, out bank.eventParameter);
    }

    /// <summary>
    /// Use to instance Generic bank with multiple parameter
    /// </summary>
    /// <param name="bank"></param>
    public void CreateGenericBankMultipleParameter(ref GenericEventMultipleParameter bank)
    {
        bank.fmodEvent = RuntimeManager.CreateInstance(bank.eventPath);
        //Set number of parameter
        bank.eventParameter = new ParameterInstance[bank.parameterName.Length];

        for (int i = 0; i < bank.parameterName.Length; i++)
        {
            GetParameterByCount(ref bank, i);
        }
    }

    /// <summary>
    /// Trigger a cue of event
    /// </summary>
    /// <param name="eventCue"></param>
    public void TriggerCue(GenericEvent eventCue)
    {
        eventCue.fmodEvent.triggerCue();
    }

    /// <summary>
    /// Get a parameter by index
    /// usefule for array of parameter
    /// </summary>
    /// <param name="bank">paramete</param>
    /// <param name="index">index of paramete</param>
    public void GetParameterByCount(ref GenericEventMultipleParameter bank, int index )
    {
        bank.fmodEvent.getParameterByIndex(index, out bank.eventParameter[index]);
    }

    public void StartBankFade(GenericEvent bank, float speed)
    {
        if (bank == null)
            return;
        StartEvent(bank);
        StartCoroutine(C_LerpOverTime(bank.fmodEvent, speed));
    }

    /// <summary>
    /// Start the current event bank
    /// </summary>
    /// <param name="bank"></param>
    public void StartEvent(GenericEvent bank)
    {
        if (bank.eventPath != "")
        {
            bank.fmodEvent.start();
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
    /// Stop the current event bank without the fade
    /// </summary>
    /// <param name="bank"></param>
    public void StopEvent(EventInstance _eventInstance)
    {
        _eventInstance.stop(STOP_MODE.IMMEDIATE);
    }

    public static void QuickInit()
    {
        RuntimeManager.StudioSystem.flushCommands();
        //RuntimeManager.StudioSystem.release();
    }

    /// <summary>
    /// Stop all events
    /// </summary>
    public void StopAllEvents()
    {
        b_master.stopAllEvents(STOP_MODE.IMMEDIATE);
    }

    public void SetEventVolume(EventInstance _eventInstance, float _volume)
    {
        _eventInstance.setVolume(_volume);
    }

    /// <summary>
    /// Play a sound one shot on a current position
    /// </summary>
    /// <param name="_path">bank path</param>
    /// <param name="_pos"> position emitter</param>
    public void PlaySoundOneShoot(string _path, Vector3 _pos)
    {
        if (_path != "")
            RuntimeManager.PlayOneShot(_path, _pos);
        else
            Debug.LogWarning("Path dosen't found");
    }

    /// <summary>
    /// Attach an event bank to current transform
    /// </summary>
    /// <param name="_eventInstance"></param>
    /// <param name="emitterTransform"></param>
    public void AttachSfx(EventInstance _eventInstance, Transform emitterTransform)
    {
        RuntimeManager.AttachInstanceToGameObject(_eventInstance, emitterTransform, GetComponent<Rigidbody2D>());
    }
    
    /// <summary>
    /// Change the bank parameter with a new value
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
    /// Get all bus
    /// </summary>
    public void GetBus()
    {
        b_master = RuntimeManager.GetBus("bus:/Master");
        b_music = RuntimeManager.GetBus("bus:/Master/" + m_busMusic);
        b_sfx = RuntimeManager.GetBus("bus:/Master/" + m_busSFX);
    }
    #endregion

    #region AudioSettings
    /// <summary>
    /// Set the value of the bus
    /// </summary>
    public void InitSoundSetting()
    {
    }

    /// <summary>
    /// Set the value of the settings music value
    /// </summary>
    /// <param name="curVolume"></param>
    public void SetMusicVolume(float curVolume)
    {
        b_music.setVolume(curVolume);
    }

    /// <summary>
    /// Set the value of the settings SFX value
    /// </summary>
    /// <param name="curVolume"></param>
    public void SetSFXVolume(float curVolume)
    {
        b_sfx.setVolume(curVolume);
    }

    ///// <summary>
    ///// Apply the change made and saved on the playerRef
    ///// </summary>
    //public void ApplyAudioSettings()
    //{
    //    InitSoundSetting();
    //}

    ///// <summary>
    ///// Discard the last change
    ///// </summary>
    //public void DiscardSoundSetting()
    //{
    //    InitSoundSetting();
    //}
    #endregion

}
