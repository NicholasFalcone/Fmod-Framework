using UnityEngine;
using FMODUnity;
using FMOD.Studio;

namespace CustomFMOD
{
    [System.Serializable]
    public class FMODEventInstance
    {
        [SerializeField,EventRef]
        private string m_eventPath = null;

        [SerializeField]
        private bool m_ready;

        private FMODEventRecord m_record = null;
        private EventInstance m_eventInstance = new EventInstance();
        [SerializeField, ParamRef]
        private string[] m_paramtersName;

        private PARAMETER_ID[] m_parameters = null;
        private EventDescription m_eventDescription {
            get => m_record != null ? m_record.Description : new EventDescription();
        }

        public PARAMETER_ID[] Parameters {
            get => m_parameters;
        }

        public FMODEventRecord FMODRecord {
            get => m_record;
            set {
                m_record = value;
                m_ready = InitEvent();
            }
        }

        public string Path {
            get => m_eventPath;
        }

        /// <summary>
        /// This function init the fmod event
        /// </summary>
        /// <returns></returns>
        public bool InitEvent()
        {
            if (Path == null)
            {
                Debug.LogError("[FMODEventInstance] Missing Event path");
                return false;
            }
            ///instance the current fmod event
            m_eventInstance = RuntimeManager.CreateInstance(Path);
            FillParameters(FMODRecord.Description);

            bool _isValid = m_eventInstance.isValid() && m_eventInstance.hasHandle();
            if (!_isValid)
            {
                Debug.LogError("Invilid FMOD Event");
            }
            return _isValid;
        }

        /// <summary>
        /// Go to initialize parametes
        /// </summary>
        /// <param name="_description"></param>
        private void FillParameters(EventDescription _description)
        {
            _description.getParameterDescriptionCount(out int _count);
            m_paramtersName = new string[_count];
            m_parameters = new PARAMETER_ID[_count];
            if (_count > 0)
            {
                for (int i = 0; i < _count; i++)
                {
                    _description.getParameterDescriptionByIndex(i, out PARAMETER_DESCRIPTION _currentParameter);
                    m_paramtersName[i] = _currentParameter.name;
                    m_parameters[i] = _currentParameter.id;
                }
            }
        }
        /// <summary>
        /// Attach event to in-game engine position
        /// </summary>
        /// <param name="_transform"></param>
        /// <param name="_rb"></param>
        public void AttachTo(Transform _transform, Rigidbody _rb = null)
        {
            RuntimeManager.AttachInstanceToGameObject(m_eventInstance, _transform, _rb);
        }

        /// <summary>
        /// Use this function to start the fmod event
        /// </summary>
        public FMOD.RESULT Play()
        {
            return m_eventInstance.start();
        }

        /// <summary>
        /// Use this function to stop the fmod event
        /// </summary>
        public void Stop()
        {
            m_eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

        /// <summary>
        /// Use this function to change the event paramter
        /// </summary>
        /// <param name="_id">event parameter id</param>
        /// <param name="_nextValue">next value</param>
        public void ChangeParameter(PARAMETER_ID _id, float _nextValue)
        {
            m_eventInstance.setParameterByID(_id, _nextValue);
        }

        /// <summary>
        /// Use this function to clean the memory resources
        /// </summary>
        private void Destroy()
        {
            m_eventInstance.release();
        }

    }
}
