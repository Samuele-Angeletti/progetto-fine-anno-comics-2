using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Commons
{
    public class SpriteTransition : MonoBehaviour
    {
        [Header("Sprite Transition")]
        [SerializeField] float m_SpriteTransitionSpeed;
        private Coroutine m_ActiveExternalGraphicsCoroutine;

        public void ActiveSpriteTransparencyTransition(SpriteRenderer spRenderer, EDirection direction)
        {
            if(direction == EDirection.Up)
            {
                m_SpriteTransitionSpeed = Mathf.Abs(m_SpriteTransitionSpeed);
            }
            else
            {
                m_SpriteTransitionSpeed = -m_SpriteTransitionSpeed;
            }

            if (m_ActiveExternalGraphicsCoroutine == null)
            {
                m_ActiveExternalGraphicsCoroutine = StartCoroutine(ChangeSpriteTransparency(m_SpriteTransitionSpeed, spRenderer, direction));
            }
            else
            {
                StopCoroutine(m_ActiveExternalGraphicsCoroutine);
                m_ActiveExternalGraphicsCoroutine = null;
                m_ActiveExternalGraphicsCoroutine = StartCoroutine(ChangeSpriteTransparency(m_SpriteTransitionSpeed, spRenderer, direction));
            }
        }

        private IEnumerator ChangeSpriteTransparency(float modifier, SpriteRenderer spriteRenderer, EDirection direction)
        {
            Color original = spriteRenderer.color;

            while (true)
            {
                original.a += modifier * Time.deltaTime;
                spriteRenderer.color = original;

                yield return null;

                if (direction == EDirection.Up)
                {
                    if (original.a >= 1) break;
                }
                else
                {
                    if (original.a <= 0) break;
                }
            }
            Debug.Log($"{gameObject.name} exit coroutine");
            m_ActiveExternalGraphicsCoroutine = null;
        }
    }
}
