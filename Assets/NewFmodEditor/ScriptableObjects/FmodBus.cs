using FMODUnity;
using UnityEngine;
using FMOD.Studio;
using System.Collections.Generic;

namespace FmodEditor
{
    /// <summary>
    /// Struct to collect all data for bus
    /// </summary>
    [System.Serializable]
    public struct BusData
    {
        #region Private-Field
        //Bus Path
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
        public float BusVolume { get { return m_busVolume; } set { m_busVolume = value; m_bus.setVolume(value); } }
        public bool Muted { get { return m_isMuted; } set { m_isMuted = value; m_bus.setMute(value);} }

        public BusData(string _busPath, Bus _currentBus, FMOD.ChannelGroup _channelGroup, float _volume, bool _muted)
        {
            m_busPath = _busPath;
            m_bus = _currentBus;
            m_channelGruop = _channelGroup;
            m_busVolume = _volume;
            m_isMuted = _muted;
        }
        #endregion
    }

    [CreateAssetMenu(menuName = "FmodData/BusData")]
    public class FmodBus : ScriptableObject
    {
        [HideInInspector]
        public List<BusData> busData;
        
        /// <summary>
        /// Initialize all Data
        /// </summary>
        public void Init()
        {
            busData = new List<BusData>();

            int numBanks = 0;
            FMOD.Studio.Bank[] banks = null;
            ///take bank count
            RuntimeManager.StudioSystem.getBankCount(out numBanks);
            //get all bank
            RuntimeManager.StudioSystem.getBankList(out banks);
            ///Scroll bank to get the relative bus
            for (int currentBank = 0; currentBank < numBanks; ++currentBank)
            {
                int numBusses = 0;
                Bus[] busses = null;
                //get bus count 
                banks[currentBank].getBusCount(out numBusses);
                //get bus
                banks[currentBank].getBusList(out busses);
                for (int currentBus = 0; currentBus < numBusses; ++currentBus)
                {
                    // Make sure the channel group of the current bus is assigned properly.
                    string busPath = null;
                    busses[currentBus].getPath(out busPath);
                    RuntimeManager.StudioSystem.getBus(busPath, out busses[currentBus]);
                    RuntimeManager.StudioSystem.flushCommands();
                    ///Check if has groupChannel
                    FMOD.ChannelGroup channelGroup;
                    busses[currentBus].getChannelGroup(out channelGroup);
                    busData.Add(new BusData(busPath, busses[currentBus], channelGroup, 1, false));
                }
            }
        }
    }
}