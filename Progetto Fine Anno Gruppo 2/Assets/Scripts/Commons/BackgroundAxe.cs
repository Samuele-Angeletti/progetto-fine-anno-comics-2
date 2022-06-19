using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Commons
{
    public class BackgroundAxe : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float m_TransitionSpeedIncrease;
        [SerializeField] float m_TransitionSpeedDecrease;

        Coroutine m_ChangeScaleCoroutine;
        float m_StartingScale;

        private void Awake()
        {
            m_StartingScale = transform.localScale.x;
        }

        public void ChangeScale(float originalCameraOrthoSize, float newCameraOrthoSize)
        {
            float destinationScale = newCameraOrthoSize * m_StartingScale / originalCameraOrthoSize;
            if(m_ChangeScaleCoroutine == null)
            {
                m_ChangeScaleCoroutine = StartCoroutine(ScaleCoroutine(destinationScale));
            }
            else
            {
                StopCoroutine(m_ChangeScaleCoroutine);
                m_ChangeScaleCoroutine = StartCoroutine(ScaleCoroutine(destinationScale));
            }
        }

        private IEnumerator ScaleCoroutine(float destinationScale)
        {
            EDirection direction;
            float transition = 0;
            Vector3 destination = new Vector3(destinationScale, destinationScale);
            if (destinationScale < transform.localScale.x)
            {
                transition = m_TransitionSpeedDecrease / 2;
                direction = EDirection.Down;
            }
            else
            {
                transition =  m_TransitionSpeedIncrease;
                direction = EDirection.Up;
            }
            
            while(true)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, destination, transition * Time.deltaTime);

                if (direction == EDirection.Down)
                {
                    transition += m_TransitionSpeedDecrease / 2 * Time.deltaTime;
                    if(transition >= m_TransitionSpeedDecrease)
                    {
                        transition = m_TransitionSpeedDecrease;
                    }
                }

                yield return new WaitForEndOfFrame();

                if(direction == EDirection.Up)
                {
                    if(transform.localScale.x >= destinationScale)
                    {
                        transform.localScale = destination;
                        break;
                    }
                }
                else
                {
                    if (transform.localScale.x <= destinationScale)
                    {
                        transform.localScale = destination;
                        break;
                    }
                }
            }

            m_ChangeScaleCoroutine = null;
        }
    }
}
