using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;
using Commons;
using TMPro;
using UnityEngine.UI;

namespace MicroGame
{
    public class GameManagerPM : MonoBehaviour, ISubscriber
    {
        #region SINGLETON PATTERN
        public static GameManagerPM _instance;
        public static GameManagerPM Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<GameManagerPM>();

                    if (_instance == null)
                    {
                        GameObject container = new GameObject("GameManager");
                        _instance = container.AddComponent<GameManagerPM>();
                    }
                }

                return _instance;
            }
        }
        #endregion

        [Header("Enemies Settings")]
        [SerializeField] Enemy m_EnemyPrefab;
        [SerializeField] Transform[] m_EnemiesSpawnPoints;

        [Header("UI Stuff")]
        [SerializeField] TextMeshProUGUI m_LifeText;
        [SerializeField] Slider m_BatterySlider;

        [Header("Load File")]
        [SerializeField] string m_FileName;

        private List<Pickable> m_PickableList = new List<Pickable>();
        private int m_PickablesInScene;
        private int m_PickablePicked = 0;
        private bool[] m_SpawnPointUsed;

        private void Awake()
        {
            Pickable[] pickables = FindObjectsOfType<Pickable>();
            for (int i = 0; i < pickables.Length; i++)
            {
                m_PickableList.Add(pickables[i]);
            }
            m_PickablesInScene = m_PickableList.Count;
            m_SpawnPointUsed = new bool[m_EnemiesSpawnPoints.Length];
        }

        private void Start()
        {
            PubSub.PubSub.Subscribe(this, typeof(PickablePickedMessage));
            PubSub.PubSub.Subscribe(this, typeof(GameOverMicroGameMessage));
            PubSub.PubSub.Subscribe(this, typeof(PauseGameMessage));
            PubSub.PubSub.Subscribe(this, typeof(ResumeGameMessage));

            StartSettings();
        }

        private void StartSettings()
        {
            DamageableInfos d = SaveAndLoadSystem.Load<DamageableInfos>(m_FileName);
            float damagePercentage = d.DamagePercentage();

            if (damagePercentage == 0)
            {
                SpawnEnemies(1);
            }
            else if (damagePercentage <= 0.2f)
            {
                SpawnEnemies(2);
            }
            else if (damagePercentage <= 0.4f)
            {
                SpawnEnemies(3);
            }
            else if (damagePercentage <= 0.6f)
            {
                SpawnEnemies(4);
            }
            else if (damagePercentage <= 0.8f)
            {
                SpawnEnemies(5);
            }
            else if (damagePercentage < 1f)
            {
                SpawnEnemies(6);
            }
            else
            {
                SpawnEnemies(7);
            }
        }

        private void SpawnEnemies(int enemiesQuantity)
        {
            for (int i = 0; i < enemiesQuantity; i++)
            {
                Instantiate(m_EnemyPrefab, ChooseFreeSpawnPoint(), Quaternion.identity);
            }
        }

        private Vector3 ChooseFreeSpawnPoint()
        {
            int attemptPerform = 0;
            while(true)
            {
                int rndIndex = UnityEngine.Random.Range(0, m_EnemiesSpawnPoints.Length);
                if(!m_SpawnPointUsed[rndIndex])
                {
                    m_SpawnPointUsed[rndIndex] = true;
                    return m_EnemiesSpawnPoints[rndIndex].position;
                }
                else if(attemptPerform >= 10)
                {
                    for (int i = 0; i < m_SpawnPointUsed.Length; i++)
                    {
                        if(!m_SpawnPointUsed[i])
                        {
                            attemptPerform = 0;
                        }
                    }
                    if(attemptPerform > 0)
                    {
                        return m_EnemiesSpawnPoints[0].position; // if all the spawn points have been used, the enemies get spawned at the first spawn point (this shouldn't happen, each enemy should have its own spawn point)
                    }
                }
                attemptPerform++;
            }
        }

        internal void UIUpdateLife(int m_CurrentLifes)
        {
            m_LifeText.text = "VITE: " + m_CurrentLifes;
        }

        public void OnPublish(IMessage message)
        {
            if(message is PickablePickedMessage)
            {
                PickablePickedMessage pickablePicked = (PickablePickedMessage)message;
                m_PickableList.Remove(pickablePicked.Pickable);
                CheckPickableLeft();
            }
            else if(message is GameOverMicroGameMessage)
            {
                Debug.Log("Gioco finito");
            }
            else if (message is PauseGameMessage)
            {
                Time.timeScale = 0;
            }
            else if (message is ResumeGameMessage)
            {
                Time.timeScale = 1;
            }
        }

        private void CheckPickableLeft()
        {
            m_PickablePicked++;

            m_BatterySlider.value = (float)m_PickablePicked / (float)m_PickablesInScene;

            if (m_PickablePicked >= m_PickablesInScene)
            {
                Debug.Log("Gioco finito");
            }
        }

        public void OnDisableSubscribe()
        {
            PubSub.PubSub.Unsubscribe(this, typeof(PickablePickedMessage));
            PubSub.PubSub.Unsubscribe(this, typeof(GameOverMicroGameMessage));
            PubSub.PubSub.Unsubscribe(this, typeof(PauseGameMessage));
            PubSub.PubSub.Unsubscribe(this, typeof(ResumeGameMessage));
        }

        private void OnDestroy()
        {
            OnDisableSubscribe();
        }
    }

}
