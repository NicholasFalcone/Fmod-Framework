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
    public EventDescription m_eventDescription;
    public EventInstance fmodEvent;
    public PARAMETER_ID[] parameterName;
}
#endregion

public class FmodManager : StudioBankLoader
{
    /// <summary>
    /// singleton
    /// </summary>
    public static FmodManager instance;

    private string m_busPrefix = "bus:/";

    #region Unity Method
    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
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
            Unload();
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
    /// Trigger a cue of event
    /// </summary>
    /// <param name="_genericEvent"></param>
    public FMOD.RESULT TriggerCue(GenericEvent _genericEvent)
    {
        return _genericEvent.fmodEvent.triggerCue();
    }

    public void StartEventFade(GenericEvent _genericEvent, float _speed)
    {
        ///check if event is null
        if (_genericEvent.fmodEvent.handle == null)
        {
            Debug.LogWarning("Fmod event doesent exist");
            return;
        }
        ///Start the current event
        StartEvent(_genericEvent);
        ///Lerp the event volume from 0 to 1 with a custom speed 
        StartCoroutine(C_LerpOverTime(_genericEvent.fmodEvent, _speed));
    }

    /// <summary>
    /// Start the current event bank
    /// </summary>
    /// <param name="_genericEvent"></param>
    public FMOD.RESULT StartEvent(GenericEvent _genericEvent)
    {
        return _genericEvent.fmodEvent.start();
    }

    /// <summary>
    /// Stop the current event bank with the fade
    /// </summary>
    /// <param name="_eventInstance"></param>
    public FMOD.RESULT StopEvent(EventInstance _eventInstance, FMOD.Studio.STOP_MODE _mode)
    {
        return _eventInstance.stop(_mode);
    }

    /// <summary>
    /// Release the current event instance
    /// </summary>
    /// <param name="_eventInstance"></param>
    public FMOD.RESULT ReleaseEvent(EventInstance _eventInstance)
    {
        return _eventInstance.release();
    }

    /// <summary>
    /// Stop all event on the master bus
    /// </summary>
    /// <param name="_masterBus">master bus</param>
    public FMOD.RESULT StopAllEvents(Bus _masterBus)
    {
        return _masterBus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    /// <summary>
    /// Change the volume value of a event
    /// </summary>
    /// <param name="_eventInstance"></param>
    /// <param name="_volume"></param>
    public FMOD.RESULT SetEventVolume(EventInstance _eventInstance, float _volume)
    {
        return _eventInstance.setVolume(_volume);
    }

    public void DebugResult(FMOD.RESULT _result)
    {
        Debug.Log(_result);
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

    /// <summary>
    /// Play a sound one shot and attach to gameobject
    /// </summary>
    /// <param name="_path">event path</param>
    /// <param name="_gameObject">gameobject to attach</param>
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
    /// <param name="_eventInstance">fmod event instance</param>
    /// <param name="_emitterTransform">transform to attach event</param>
    public void AttachSfx(EventInstance _eventInstance, Transform _emitterTransform)
    {
        RuntimeManager.AttachInstanceToGameObject(_eventInstance, _emitterTransform, GetComponent<Rigidbody2D>());
        _eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(_emitterTransform.position));
    }

    /// <summary>
    /// Check if this event is playing
    /// </summary>
    /// <returns></returns>
    public bool IsPlaying(EventInstance _eventInstance)
    {
        FMOD.Studio.PLAYBACK_STATE playbackState;
        _eventInstance.getPlaybackState(out playbackState);
        return playbackState != FMOD.Studio.PLAYBACK_STATE.STOPPED;
    }

    /// <summary>
    /// Lerp the event audio volume from 0 to 1 with a speed parameter
    /// </summary>
    /// <param name="_eventInstance"></param>
    /// <param name="_step">increesing time from 0 to 1</param>
    /// <returns></returns>
    private IEnumerator C_LerpOverTime(EventInstance _eventInstance, float _step)
    {
        float elapsedTime = 0;
        const float startVolume = 0;
        float currentVolume = 0;
        const float targetVolume = 1;

        while (elapsedTime < _step)
        {
            currentVolume = (Mathf.Lerp(startVolume, targetVolume, (elapsedTime / _step)));
            _eventInstance.setVolume(currentVolume);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_bus">Variable instance</param>
    /// <param name="_path">Channel path</param>
    public void SetBus(ref Bus _bus, string _path)
    {
        _bus = RuntimeManager.GetBus(m_busPrefix + _path);
    }
    #endregion
}