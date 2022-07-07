using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;
using Commons;
using TMPro;
using UnityEngine.UI;
using ArchimedesMiniGame;
using MainGame;

namespace MicroGame
{
    public class GameManagerPM : MonoBehaviour, ISubscriber
    {
        #region SINGLETON PATTERN
        public static GameManagerPM m_instance;
        public static GameManagerPM Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = FindObjectOfType<GameManagerPM>();

                    if (m_instance == null)
                    {
                        GameObject container = new GameObject("GameManager");
                        m_instance = container.AddComponent<GameManagerPM>();
                    }
                }

                return m_instance;
            }
        }
        #endregion

        [Header("Player Settings")]
        [SerializeField] Controllable m_Controllable;

        [Header("Enemies Settings")]
        [SerializeField] Enemy m_EnemyPrefab;
        [SerializeField] Transform[] m_EnemiesSpawnPoints;
        [SerializeField] List<Sprite> m_EnemiesSprites;

        [Header("Scene references")]
        [SerializeField] List<GameObject> m_AllSceneObjects;

        [Header("Prefabs")]
        [SerializeField] GameObject m_AdaLifeSprite;

        [HideInInspector]
        public UIPacManInterface UIPacManInterface;

        private List<Pickable> m_PickableList = new List<Pickable>();
        private List<Enemy> m_Enemies = new List<Enemy>();
        private int m_PickablesInScene;
        private int m_PickablePicked = 0;
        private bool[] m_SpawnPointUsed;
        private SavableInfos m_CurrentModuleInfos;
        private int m_EnemiesQuantity;
        Dictionary<string, SavableInfos> databaseLoaded;
        Queue<GameObject> m_AdaLifeSpritesSpawned = new Queue<GameObject>();
        private void Awake()
        {
            if (m_instance == null)
            {
                m_instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);

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
            UIPacManInterface = UIManager.Instance.PacManInterface;
            GameManager.Instance.SetNewControllable(m_Controllable);

            StartSettings();

        }

        private void StartSettings()
        {
            m_CurrentModuleInfos = GameManagerES.Instance.GetCurrentModuleInfos();

            float damagePercentage = m_CurrentModuleInfos.DamagePercentage();

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
                Enemy e = Instantiate(m_EnemyPrefab, ChooseFreeSpawnPoint(), Quaternion.identity);
                m_Enemies.Add(e);
                e.SpriteRenderer.sprite = m_EnemiesSprites[UnityEngine.Random.Range(0, m_EnemiesSprites.Count)];
            }
            m_EnemiesQuantity = enemiesQuantity;
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

        internal void LoseLife()
        {
            GameObject g = m_AdaLifeSpritesSpawned.Dequeue();
            Destroy(g);
        }

        public void UISpawnInitialLifes(int initialLifes)
        {
            for (int i = 0; i < initialLifes; i++)
            {
                GameObject g = Instantiate(m_AdaLifeSprite, UIPacManInterface.LifePanelContainer.transform);
                m_AdaLifeSpritesSpawned.Enqueue(g);
            }
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
                GameOverMicroGameMessage gameOverMicroGame = (GameOverMicroGameMessage)message;
                if (gameOverMicroGame.Win)
                {
                    SaveAndChangeScene();
                }
                else
                {
                    ChangeScene();
                }
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

            UIPacManInterface.BatterySlider.value = (float)m_PickablePicked / (float)m_PickablesInScene;

            if (m_PickablePicked >= m_PickablesInScene)
            {
                PubSub.PubSub.Publish(new GameOverMicroGameMessage(true));

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

        public void RemoveFromList(Enemy enemy)
        {
            m_Enemies.Remove(enemy);
        }

        public void SaveAndChangeScene()
        {

            float lifeGained = (float)((float)m_CurrentModuleInfos.MaxLife - (float)m_CurrentModuleInfos.CurrentLife) * (float)(((float)m_EnemiesQuantity - (float)m_Enemies.Count) / (float)m_EnemiesQuantity);

            m_CurrentModuleInfos.SetCurrentLife(m_CurrentModuleInfos.CurrentLife + lifeGained);

            m_CurrentModuleInfos.CurrentBattery = m_CurrentModuleInfos.MaxBattery;

            GameManagerES.Instance.SetCurrentModuleInfos(m_CurrentModuleInfos);
            ChangeScene();

        }

        private void ChangeScene()
        {
            GameManager.Instance.ForcePlayerAsControllable();
            DestroyAllObjects();
            UIManager.Instance.OpenPacManInterface(false);
        }

        private void DestroyAllObjects()
        {
            foreach(GameObject gameObject in m_AllSceneObjects)
            {
                gameObject.SetActive(false);
                Destroy(gameObject, 3f);
            }
            foreach(GameObject g in m_AdaLifeSpritesSpawned)
            {
                Destroy(g, 3);
            }
            m_Enemies.ForEach(x => x.gameObject.SetActive(false));
            m_Enemies.ForEach(x => Destroy(x.gameObject, 3f));
            m_AdaLifeSpritesSpawned.Clear();
            Destroy(gameObject, 3f);
        }

    }

}
