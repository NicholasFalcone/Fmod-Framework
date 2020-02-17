using FMODUnity;
using UnityEngine;

public class FMODCoreExample : MonoBehaviour
{
    ///sample path and name
    private const string m_soundPath = "/Audio/Sample/";
    private const string m_soundName = "sample.wav";
    ///structure containing the sound reference
    private FMOD.Sound m_sound = new FMOD.Sound();
    ///channel where the sfx will be played
    private FMOD.ChannelGroup m_channelGroup = new FMOD.ChannelGroup();

    private void Start()
    {
        ///get mastet channel
        RuntimeManager.CoreSystem.getMasterChannelGroup(out m_channelGroup);
        ///using core to load the sample and fill the data structure with the sfx values
        RuntimeManager.CoreSystem.createSound(Application.dataPath + m_soundPath + m_soundName, FMOD.MODE.CREATESAMPLE, out m_sound);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            PlaySampleSound();
        }
    }

    /// <summary>
    /// Exmaple of: how a sound be create and played
    /// </summary>
    private void PlaySampleSound()
    {
        ///using core to play the sfx in the channel
        RuntimeManager.CoreSystem.playSound(m_sound, m_channelGroup, false, out FMOD.Channel _outChannel);
    }

    private void OnApplicationQuit()
    {
        FreeMemory();
    }

    private void FreeMemory()
    {
        m_sound.release();
    }
}
