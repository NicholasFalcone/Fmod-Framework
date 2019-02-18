using FMODUnity;
using UnityEngine;
using FMOD.Studio;
using System.Collections.Generic;

namespace FmodEditor
{
    public class FmodBus : ScriptableObject
    {
        [HideInInspector]
        public List<BusData> busData;

        /// <summary>
        /// Initialize all Data
        /// </summary>
        public void Init(string _path = null)
        {
            #if UNITY_EDITOR
            if(!Application.isEditor)
                return;

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
                    busses[currentBus] = RuntimeManager.GetBus(busPath);
                    RuntimeManager.StudioSystem.flushCommands();
                    ///Check if has groupChannel
                    FMOD.ChannelGroup channelGroup;
                    busses[currentBus].getChannelGroup(out channelGroup);
                    ///Create instance of BusData
                    BusData currentBusData = ScriptableObject.CreateInstance<BusData>();
                    ///Initialize varialbe
                    currentBusData.Init(busPath, busses[currentBus], channelGroup, 1, false);
                    if (Application.isEditor && _path != null)
                    {
                        ///Create uniquepath
                        string _uniquePath = UnityEditor.AssetDatabase.GenerateUniqueAssetPath(_path + "/" + SetBusName(busPath) + ".asset");
                        ///Create data
                        UnityEditor.AssetDatabase.CreateAsset(currentBusData, _uniquePath);
                        UnityEditor.AssetDatabase.SaveAssets();
                        UnityEditor.AssetDatabase.Refresh();
                    }
                    ///Add on list
                    busData.Add(currentBusData);
                }
            }
            #endif

        }

        public string SetBusName(string _busPath)
        {
            string[] asd = _busPath.Split(':', '/');
            Debug.Log(asd[asd.Length - 1]);
            if (asd[asd.Length - 1] == "")
                return "Master";
            else
                return asd[asd.Length - 1];
        }

    }
}