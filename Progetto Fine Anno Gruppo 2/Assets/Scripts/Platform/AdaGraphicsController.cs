using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;

namespace MainGame
{
    public class AdaGraphicsController : MonoBehaviour, ISubscriber
    {
        [SerializeField] GameObject m_AdaOne;
        [SerializeField] GameObject m_AdaTwo;
        [SerializeField] GameObject m_AdaThree;
        [SerializeField] GameObject m_AdaFour;

        int m_CurrentAda;
        List<GameObject> m_Adas = new List<GameObject>();
        private void Start()
        {
            PubSub.PubSub.Subscribe(this, typeof(AdaChangeMessage));

            m_CurrentAda = 0;

            m_Adas.Add(m_AdaOne);
            m_Adas.Add(m_AdaTwo);
            m_Adas.Add(m_AdaThree);
            m_Adas.Add(m_AdaFour);

            SetNextAda(m_CurrentAda);

        }

        public void OnDisableSubscribe()
        {
            PubSub.PubSub.Unsubscribe(this, typeof(AdaChangeMessage));

        }

        public void OnPublish(IMessage message)
        {
            if(message is AdaChangeMessage)
            {
                m_CurrentAda++;
                SetNextAda(m_CurrentAda);
            }
        }

        private void SetNextAda(int adaActive)
        {
            if (m_CurrentAda < m_Adas.Count)
            {
                m_Adas.ForEach(x => x.SetActive(false));
                m_Adas[adaActive].SetActive(true);
            }
        }

        private void OnDestroy()
        {
            OnDisableSubscribe();
        }
    }
}
