using FMOD.Studio;
using FMODUnity;
using System.Collections.Generic;

namespace CustomFMOD
{
    [System.Serializable]
    public class FMODEventRecord
    {
        [UnityEngine.SerializeField,EventRef]
        private string m_eventPath = null;
        private System.Guid m_guid = new System.Guid();
        private EventDescription m_eventDescription = new EventDescription();
        [UnityEngine.SerializeField, ParamRef]
        private List<string> m_paramters = new List<string>();
        [UnityEngine.SerializeField]
        private int m_instanceCount = 0;

        public int InstanceCount {
            get {
                m_eventDescription.getInstanceCount(out m_instanceCount);
                return m_instanceCount;
            }
        }
        
        public string Path {
            get => m_eventPath;
        }

        public System.Guid EventGuid {
            get => m_guid;
        }

        public EventDescription Description {
            get => m_eventDescription;
        }

        public FMODEventRecord(System.Guid _id)
        {
            this.m_guid = _id;
        }

        public FMODEventRecord(EventDescription _description)
        {
            this.m_eventDescription = _description;
            SetupData();
        }

        private FMOD.RESULT SetupData()
        {
            FMOD.RESULT _currentResult;

            if (!m_eventDescription.isValid())
            {
                PrintError(FMOD.RESULT.ERR_INTERNAL);
                return FMOD.RESULT.ERR_INTERNAL;
            }

            if (!m_eventDescription.hasHandle())
            {
                PrintError(FMOD.RESULT.ERR_INVALID_HANDLE);
                return FMOD.RESULT.ERR_INVALID_HANDLE;
            }
            ///Get all parameter of the current eventdescription
            _currentResult = m_eventDescription.getParameterDescriptionCount(out int _parameterCount);
            for (int i = 0; i < _parameterCount; i++)
            {
                _currentResult = m_eventDescription.getParameterDescriptionByIndex(i, out PARAMETER_DESCRIPTION _desc);
                if (_currentResult != FMOD.RESULT.OK)
                {
                    PrintError(_currentResult);
                    break;
                }
                m_paramters.Add(_desc.name);
            }

            if (_currentResult == FMOD.RESULT.OK)
            {
                ///Get event ID
                _currentResult = m_eventDescription.getID(out m_guid);
                ///Get event path
                _currentResult = m_eventDescription.getPath(out m_eventPath);

                if (_currentResult == FMOD.RESULT.OK)
                {
                    return _currentResult;
                }
                else
                {
                    PrintError(_currentResult);
                }
            }
            else
            {
                PrintError(_currentResult);
            }
            return _currentResult;
        }

        /// <summary>
        /// Function to print on unity console the current FMOD result
        /// </summary>
        /// <param name="_res"></param>
        private void PrintError(FMOD.RESULT _res)
        {
            UnityEngine.Debug.LogError("[Event Description] " + _res);
        }
    }
}