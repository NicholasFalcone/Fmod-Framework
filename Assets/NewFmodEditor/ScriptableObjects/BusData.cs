using FMOD.Studio;
using UnityEngine;
using FMODUnity;

namespace FmodEditor
{
    /// <summary>
    /// Scriptableobject to collect all data for bus
    /// </summary>
    public class BusData : ScriptableObject
    {
        #region Private-Field
        //Bus Path
        [SerializeField]
        private string m_busPath;
        //Bus Data
        private Bus m_bus;
        //Channel Group
        private FMOD.ChannelGroup m_channelGruop;
        //Bus Volume
        private float m_busVolume;
        //Bus is Muted
        private bool m_isMuted;
        #endregion

        #region Public-Field
        public string BusName { get { return m_busPath; } }
        public Bus Bus { get { return m_bus; } }
        public string Path{get {return m_busPath;}}
        public float BusVolume
        {
            get { return m_busVolume; }
            set
            {
                m_busVolume = value;
                if(!m_bus.isValid())
                {
                    m_bus =  RuntimeManager.GetBus(m_busPath);
                }
           
               FMOD.RESULT r =  m_bus.setVolume(value);
            }
        }
        public bool Muted { get { return m_isMuted; } set { m_isMuted = value; m_bus.setMute(value); } }
        #endregion

        public void Init(string _busPath, Bus _currentBus, FMOD.ChannelGroup _channelGroup, float _volume, bool _muted)
        {
            Debug.Log("Init");
            m_busPath = _busPath;
            m_bus = _currentBus;
            m_channelGruop = _channelGroup;
            m_busVolume = _volume;
            m_isMuted = _muted;
        }

        public void SetVolumeSlider(UnityEngine.UI.Slider slider)
        {
            BusVolume = slider.value;
        }

    }
}