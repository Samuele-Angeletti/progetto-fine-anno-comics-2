using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;
using UnityEngine.UI;
using Commons;
using MainGame;
using System.Linq;

namespace ArchimedesMiniGame
{
    public class GameManagerES : MonoBehaviour, ISubscriber
    {
        #region SINGLETON PATTERN
        public static GameManagerES m_instance;
        public static GameManagerES Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = FindObjectOfType<GameManagerES>();

                    if (m_instance == null)
                    {
                        GameObject container = new GameObject("GameManager");
                        m_instance = container.AddComponent<GameManagerES>();
                    }
                }

                return m_instance;
            }
        }
        #endregion

        private Module m_CurrentModule;

        [Header("UI References")]
        [SerializeField] Slider m_BatterySlider;
        [SerializeField] Slider m_DamageSlider;
        [SerializeField] Slider m_MaxSpeedSlider;
        [SerializeField] Slider m_AccelerationSlider;
        [SerializeField] Button m_DockingAttemptButton;


        [Header("Modules")]
        [SerializeField] List<Module> m_Modules;

        [Header("Scene References")]
        [SerializeField] ButtonInteraction m_CommandPlat;

        private string m_FileName;
        public Module CurrentModule => m_CurrentModule;
        private string m_CurrentModuleId = "";
        private void Awake()
        {
            if (m_instance == null)
            {
                m_instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            PubSub.PubSub.Subscribe(this, typeof(DockingCompleteMessage));
            PubSub.PubSub.Subscribe(this, typeof(ModuleDestroyedMessage));
            PubSub.PubSub.Subscribe(this, typeof(NoBatteryMessage));
            PubSub.PubSub.Subscribe(this, typeof(PauseGameMessage));
            PubSub.PubSub.Subscribe(this, typeof(ResumeGameMessage));
            PubSub.PubSub.Subscribe(this, typeof(StartEngineModuleMessage));

            ActiveDockingAttemptButton(false);
        }

        public void SaveData()
        {
            m_FileName = "ModuleInfo" + m_Modules.IndexOf(m_CurrentModule);
            SaveAndLoadSystem.Save(m_CurrentModule.GetSavableInfos(), m_FileName);
        }

        public string GetCurrentModuleID()
        {
            return m_CurrentModuleId;
        }

        public void OnPublish(IMessage message)
        {
            if(message is StartEngineModuleMessage)
            {
                StartEngineModuleMessage startEngineModule = (StartEngineModuleMessage)message;
                m_CurrentModule = startEngineModule.Module;
                m_CurrentModuleId = m_CurrentModule.SavableEntity.Id;
                Debug.Log(m_CurrentModule.name);
            }
            else if(message is DockingCompleteMessage || message is NoBatteryMessage || message is ModuleDestroyedMessage)
            {
                //SaveData();
                SetNextModuleToCommandPlat();
            }
            else if(message is PauseGameMessage)
            {
                Time.timeScale = 0;
            }
            else if(message is ResumeGameMessage)
            {
                Time.timeScale = 1;
            }
        }

        public void UpdateSpeed(float current, float maxValue)
        {
            m_MaxSpeedSlider.value = current / maxValue;
        }

        internal void UpdateAcceleration(float magnitude, float acceleration)
        {
            m_AccelerationSlider.value = magnitude / acceleration;
        }

        public void UpdateBatterySlider(float current, float maxValue)
        {
            m_BatterySlider.value = current / maxValue;
        }

        public void UpdateLifeSlider(float current, float maxValue)
        {
            m_DamageSlider.value = current / maxValue;
        }

        public void ActiveDockingAttemptButton(bool active)
        {
            m_DockingAttemptButton.interactable = active;
        }

        public void OnDisableSubscribe()
        {
            PubSub.PubSub.Unsubscribe(this, typeof(DockingCompleteMessage));
            PubSub.PubSub.Unsubscribe(this, typeof(ModuleDestroyedMessage));
            PubSub.PubSub.Unsubscribe(this, typeof(NoBatteryMessage));
            PubSub.PubSub.Unsubscribe(this, typeof(PauseGameMessage));
            PubSub.PubSub.Unsubscribe(this, typeof(ResumeGameMessage));
        }

        private void OnDestroy()
        {
            OnDisableSubscribe();
        }

        public void SetNextModuleToCommandPlat()
        {
            int index = m_Modules.IndexOf(m_CurrentModule) + 1;
            if (index < m_Modules.Count)
            {
                m_CommandPlat.SetInterestedObject(m_Modules[index].gameObject);

                GameManager.Instance.SetTargetForCamera(m_Modules[index].transform);
            }
        }
    }
}
