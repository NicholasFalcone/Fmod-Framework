using UnityEngine;
using FMOD.Studio;
using FMODUnity;
using System.Collections.Generic;
using System.Collections;
using System;

namespace CustomFMOD
{
    [System.Serializable]
    public class FMODEventRequest
    {
        public FMODEventInstance m_event = null;

        public Action m_onRequestDone = null;

        public FMODEventRequest(FMODEventInstance _eventInstance, Action _onRequestDone)
        {
            this.m_event = _eventInstance;
            this.m_onRequestDone = _onRequestDone;
        }
    }

    public class FMODDatabase : Singleton<FMODDatabase>
    {
        private FMOD.Studio.System m_system = new FMOD.Studio.System();
        private const string assetsPath = "/Audio/Desktop/";

        private const string bankExtension = ".bank";
        [SerializeField]
        private List<FMODEventRecord> m_eventRecords = null;
        private List<Bank> m_banks = new List<Bank>();
        private PARAMETER_DESCRIPTION[] m_globalParameters = null;
        private Queue<FMODEventRequest> m_eventRequests = new Queue<FMODEventRequest>();
        [SerializeField]
        private int m_eventRequestCount = 0;
        [SerializeField, ParamRef]
        private string[] m_globalParametersPath = null;
        [SerializeField, BankRef]
        private string[] m_banksPath = null;
        private bool m_eventCollectionReady = false;

        public int m_EventRequestCount {
            get => m_eventRequestCount;
        }

        private bool hasFinishLoadingBanks {
            get => m_banks.Count == m_banksPath.Length;
        }


        public FMOD.Studio.System System {
            get => m_system;
        }

        public List<FMODEventRecord> FMODEventRecord {
            get => m_eventRecords;
        }

        #region Unity-Method
        protected override void Awake()
        {
            base.Awake();
            InitStudio();
            LoadBank();
            GetGlobalParameters();
        }


        private void Update()
        {
            m_system.update();

            ManagerRequest();
        }


        private void OnApplicationQuit()
        {
            ShutDown();
        }
        #endregion

        #region Private-Method
        private void ManagerRequest()
        {
            if (!m_eventCollectionReady)
                return;

            if (m_eventRequests.Count > 0)
            {
                FMODEventRequest request = m_eventRequests.Dequeue();
                if (!FindAssociation(request.m_event))
                {
                    m_eventRequests.Enqueue(request);
                }
                else
                {
                    if(request.m_onRequestDone != null)
                    {
                        request.m_onRequestDone.Invoke();
                    }
                }
                m_eventRequestCount = m_eventRequests.Count;
            }
        }

        private void GetGlobalParameters()
        {
            ///initialize global parameters
            m_globalParameters = new PARAMETER_DESCRIPTION[m_globalParametersPath.Length];
            m_globalParameters.Initialize();
            for (int i = 0; i < m_globalParametersPath.Length; i++)
            {
                RuntimeManager.StudioSystem.getParameterDescriptionByName(m_globalParametersPath[i], out m_globalParameters[i]);
            }
        }

        private void InitStudio()
        {
            FMOD.RESULT res = FMOD.RESULT.OK;
            if (!m_system.isValid())
            {
                res = FMOD.Studio.System.create(out m_system);
                if (res != FMOD.RESULT.OK)
                {
                    Debug.LogError("Fail to create Studio System: " + res);
                    return;
                }

                res = m_system.initialize(256, INITFLAGS.ALLOW_MISSING_PLUGINS | INITFLAGS.SYNCHRONOUS_UPDATE, FMOD.INITFLAGS.NORMAL, IntPtr.Zero);
                if (res != FMOD.RESULT.OK)
                {
                    Debug.LogError(res);
                    return;
                }
            }
        }

        private void LoadBank()
        {
            FMOD.RESULT res = FMOD.RESULT.OK;
            foreach (string bankPath in m_banksPath)
            {
                if(m_banksPath == null)
                {
                    Debug.LogError("[FMODDataBase] missing bank path");
                    continue;
                }

                Bank _currentBank;
                string _finalPath = Application.dataPath + assetsPath + bankPath + bankExtension;
                res = m_system.loadBankFile(_finalPath, LOAD_BANK_FLAGS.NONBLOCKING, out _currentBank);
                if (res != FMOD.RESULT.OK)
                {
                    Debug.LogError("Fail to recive banks " + res);
                }
                StartCoroutine(WaitLoadingBank(_currentBank, () =>
                {
                    if (hasFinishLoadingBanks)
                        GetAllEvents();
                }));
            }
        }

        private void GetAllEvents()
        {
            FMOD.RESULT result = FMOD.RESULT.ERR_INVALID_HANDLE;

            ///iterate throw the current banks
            foreach (Bank b in m_banks)
            {
                ///Load meta data of the current bank
                b.loadSampleData();
                ///get the loading state of the current bank
                b.getLoadingState(out LOADING_STATE _state);
                if (_state == LOADING_STATE.LOADED)
                {
                    ///Get all event from the current bank
                    result = b.getEventList(out EventDescription[] _des);
                    ///Fail to get events
                    if (result != FMOD.RESULT.OK)
                    {
                        ///print current error
                        Debug.LogError(result);
                        return;
                    }
                    ///iterate throw the events
                    foreach (EventDescription d in _des)
                    {
                        ///check the event validation
                        if (!d.isValid() || !d.hasHandle())
                        {
                            Debug.LogError("Event Description not valid");
                            continue;
                        }
                        ///load event description meta
                        d.loadSampleData();
                        ///wait for the loading response
                        StartCoroutine(LoadingSample(d, () =>
                        {
                            ///add it on the the event collection
                            m_eventRecords.Add(AddEventTemplate(d));
                        }));
                    }
                    m_eventCollectionReady = true;
                    return;
                }
                else
                {
                    Debug.LogError("Bank not loaded");
                    return;
                }
            }
        }

        /// <summary>
        /// Use this method to release all event instance, unload all banks and system
        /// </summary>
        private void ShutDown()
        {
            ///release and clear all events
            foreach (FMODEventRecord events in m_eventRecords)
            {
                events.Description.releaseAllInstances();
                events.Description.unloadSampleData();
            }
            m_eventRecords.Clear();
            ///unload and clear all banks
            foreach (Bank bank in m_banks)
            {
                bank.unload();
            }
            m_banks.Clear();
            ///release system
            if (m_system.isValid())
            {
                m_system.release();
            }
        }

        private FMODEventRecord AddEventTemplate(EventDescription _description)
        {
            return new FMODEventRecord(_description);
        }

        private bool FindAssociation(FMODEventInstance _eventInstance)
        {
            FMODEventRecord _record = m_eventRecords.Find((x) => x.Path == _eventInstance.Path);
            if (_record != null)
            {
                _eventInstance.FMODRecord = _record;
                return true;
            }
            return false;
        }

        IEnumerator WaitLoadingBank(Bank _current, System.Action _onLoadedAction = null)
        {
            LOADING_STATE _loadingState = LOADING_STATE.ERROR;
            while (_loadingState != LOADING_STATE.LOADED)
            {
                FMOD.RESULT res = _current.getLoadingState(out _loadingState);
                yield return null;
            }
            m_banks.Add(_current);
            _onLoadedAction.Invoke();
            yield return null;
        }

        IEnumerator LoadingSample(EventDescription _des, System.Action _onEndAction)
        {
            LOADING_STATE _state = LOADING_STATE.ERROR;
            do
            {
                _des.getSampleLoadingState(out _state);
                yield return new UnityEngine.WaitForEndOfFrame();
            }
            while (_state != LOADING_STATE.LOADED);

            _onEndAction.Invoke();
            yield return null;
        }
        #endregion

        #region Public-Method

        public void ForceInitization()
        {
            InitStudio();
            LoadBank();
            GetGlobalParameters();
        }

        public bool GetFmodEvent(string _path, out FMODEventRecord _event)
        {
            _event = null;
            if (_path == null)
                return false;
            _event = m_eventRecords.Find((x) => x.Path == _path);
            if (_event != null)
                return true;
            return false;
        }

        public bool GetFmodEvent(FMODEventInstance _eventInstance, Action _onRequestDone = null)
        {
            if (m_eventRecords == null || m_eventRecords.Count == 0)
            {
                m_eventRequests.Enqueue(new FMODEventRequest(_eventInstance, _onRequestDone));
                m_eventRequestCount = m_eventRequests.Count;
                return false;
            }
            return FindAssociation(_eventInstance);
        }

        public bool GetFmodEvent(System.Guid _id, out FMODEventRecord _event)
        {
            _event = null;
            if (_id == null)
                return false;
            _event = m_eventRecords.Find((x) => x.EventGuid == _id);
            if (_event != null)
                return true;
            return false;
        }
        #endregion
    }
}

