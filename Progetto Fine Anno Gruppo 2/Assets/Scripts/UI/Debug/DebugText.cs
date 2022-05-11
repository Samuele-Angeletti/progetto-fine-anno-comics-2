using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Commons
{
    public class DebugText : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI m_DebugText;
        [SerializeField] bool m_StringText;

        private bool m_Active;
        private string m_Message;
        private string m_ElapsedTimeText;
        private float m_TimeElapsed;
        public void Active(bool active)
        {
            m_Active = active;
            m_TimeElapsed = 0;
        }

        private void Update()
        {
            if(m_Active)
            {
                if(m_StringText)
                {
                    m_DebugText.text = m_Message;
                }
                else
                {
                    UpdateElapsedTime(m_TimeElapsed += Time.deltaTime);
                    m_DebugText.text = m_Message + m_ElapsedTimeText;
                }
            }
        }

        public void SetMessage(string message)
        {
            m_Message = message;
        }

        public void UpdateElapsedTime(float time)
        {
            TimeSpan timeParsed = TimeSpan.FromSeconds(time);
            m_ElapsedTimeText = timeParsed.ToString(@"mm':'ss':'fff");
        }
    }
}
