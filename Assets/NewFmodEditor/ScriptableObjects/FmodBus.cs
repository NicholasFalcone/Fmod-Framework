using FMODUnity;
using UnityEngine;
using FMOD.Studio;
using System.Collections.Generic;


namespace FmodEditor
{
    [System.Serializable]
    public struct BusData
    {
        //Bus Path
        public string busPath;
        //Bus Data
        public Bus bus;
        //Channel Group
        FMOD.ChannelGroup channelGruop;

        public BusData(string _busPath, Bus _currentBus, FMOD.ChannelGroup _channelGroup)
        {
            busPath = _busPath;
            bus = _currentBus;
            channelGruop = _channelGroup;
        }
    }

    [CreateAssetMenu(menuName = "FmodData/BusData")]
    public class FmodBus : ScriptableObject
    {
        private string m_busPrefix = "bus:/";
        public List<BusData> busData;
        /// <summary>
        /// Initialize all Data
        /// </summary>
        public void Init()
        {
            busData = new List<BusData>();

            int numBanks = 0;
            FMOD.Studio.Bank[] banks = null;
            RuntimeManager.StudioSystem.getBankCount(out numBanks);
            RuntimeManager.StudioSystem.getBankList(out banks);
            for (int currentBank = 0; currentBank < numBanks; ++currentBank)
            {
                int numBusses = 0;
                Bus[] busses = null;
                banks[currentBank].getBusCount(out numBusses);
                banks[currentBank].getBusList(out busses);
                for (int currentBus = 0; currentBus < numBusses; ++currentBus)
                {
                    // Make sure the channel group of the current bus is assigned properly.
                    string busPath = null;
                    busses[currentBus].getPath(out busPath);
                    RuntimeManager.StudioSystem.getBus(busPath, out busses[currentBus]);
                    RuntimeManager.StudioSystem.flushCommands();
                    FMOD.ChannelGroup channelGroup;
                    busses[currentBus].getChannelGroup(out channelGroup);
                    busData.Add(new BusData(busPath, busses[currentBus], channelGroup));
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="_bus">Variable instance</param>
        /// <param name="_path">Channel path</param>
        public void SetBus(Bus _bus, string _path)
        {
            _bus = RuntimeManager.GetBus(m_busPrefix + _path);
        }

    }
}