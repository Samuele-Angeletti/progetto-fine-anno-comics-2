using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;
using UnityEngine.UI;
using Commons;
using MainGame;
using System.Linq;
using System;

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
        [SerializeField] Button m_DockingAttemptButton;
        [SerializeField] Image m_DamagedScreen;
        [SerializeField] LogMessageHandler m_LogMessageHandler;
        [SerializeField] Transform m_AnchorIndicator;
        [SerializeField] List<Sprite> m_ImageOnDamage;

        [Header("Modules")]
        [SerializeField] List<Module> m_Modules;

        [Header("Scene References")]
        [SerializeField] ButtonInteraction m_CommandPlat;

        [Header("VFX")]
        [SerializeField] List<ParticleSystem> m_Asteroids;

        public Module CurrentModule => m_CurrentModule;
        private SavableInfos m_CurrentModuleInfos;
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
            PubSub.PubSub.Subscribe(this, typeof(PlayPacManMessage));

            ActiveDockingAttemptButton(false);
        }

        public SavableInfos GetCurrentModuleInfos()
        {
            return m_CurrentModuleInfos;
        }

        public void SetCurrentModuleInfos(SavableInfos infos)
        {
            m_CurrentModuleInfos = infos;

            UpdateBatterySlider(infos.CurrentBattery, infos.MaxBattery);
            UpdateLifeSlider(infos.CurrentLife, infos.MaxLife);
                
        }

        public void OnPublish(IMessage message)
        {
            if(message is StartEngineModuleMessage)
            {
                StartEngineModuleMessage startEngineModule = (StartEngineModuleMessage)message;
                CheckModuleOnStartEngine(m_CurrentModule);
                m_CurrentModule = startEngineModule.Module;
                m_Asteroids.ForEach(x => x.gameObject.SetActive(true));
            }
            else if(message is DockingCompleteMessage)
            {
                m_Asteroids.ForEach(x => x.gameObject.SetActive(false));
            }
            else if(message is ModuleDestroyedMessage || message is NoBatteryMessage)
            {
                m_CurrentModuleInfos = m_CurrentModule.GetSavableInfos();
                m_Asteroids.ForEach(x => x.gameObject.SetActive(false));
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
            //m_MaxSpeedSlider.value = current / maxValue;
            float percentage = current / maxValue;

            m_AnchorIndicator.eulerAngles = new Vector3(0, 0, -360 * Mathf.Abs(percentage));
        }


        public void UpdateBatterySlider(float current, float maxValue)
        {
            m_BatterySlider.value = current / maxValue;

        }

        public void UpdateLifeSlider(float current, float maxValue)
        {
            float percent = current / maxValue;

            if(percent < 0.25f)
            {
                m_DamagedScreen.sprite = m_ImageOnDamage[3];
            }
            else if(percent < 0.5f)
            {
                m_DamagedScreen.sprite = m_ImageOnDamage[2];
            }
            else if(percent < 0.75f)
            {
                m_DamagedScreen.sprite = m_ImageOnDamage[1];
            }
            else
            {
                m_DamagedScreen.sprite = m_ImageOnDamage[0];
            }
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
            PubSub.PubSub.Unsubscribe(this, typeof(PlayPacManMessage));
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

                StartCoroutine(GameManager.Instance.SetTargetForCamera(m_Modules[index].transform));
            }
        }

        public void CheckModuleOnStartEngine(Module module)
        {
            if(m_CurrentModule != null)
            {
                if (module == m_CurrentModule)
                {
                    if (m_CurrentModuleInfos != null)
                    {
                        module.SetInitialParameters(m_CurrentModuleInfos);
                    }
                }
            }
        }

        internal void SendLogMessage(string text, float timeDisplay)
        {
            m_LogMessageHandler.SpawnMessage(text, timeDisplay);
        }
    }
}
