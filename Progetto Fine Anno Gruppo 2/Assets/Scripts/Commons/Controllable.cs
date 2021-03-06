using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Commons;
using MicroGame;
using MainGame;
using System;
using PubSub;
namespace Commons
{
    public class Controllable : MonoBehaviour
    {
        [Header("Main Settings")]
        [SerializeField] Vector2 m_StoppingDistance;
        [Tooltip("Se questa funzione ? attiva, il movimento del personaggio sar? continuo fino a quando non incontrer? un ostacolo")]
        [SerializeField] bool m_ContinousMovement;


        internal Vector2 m_Direction;

        public bool ContinousMovement
        {
            get => m_ContinousMovement;
            set
            {
                m_ContinousMovement = value;
                PubSub.PubSub.Publish(new ChangeContinousMovementMessage(value));
            }
        }

        public virtual void MoveDirection(Vector2 newDirection)
        {

        }

        public virtual void MoveRotation(Vector2 newDirection)
        {

        }

        public virtual void DebugZeroG()
        {

        }

        public virtual void Interact()
        {

        }

        public virtual void Jump(bool jumping)
        {

        }

        /// <summary>
        /// Returns true if it is possible to go in that direction. False instead
        /// </summary>
        /// <param name="direction">Direction to go</param>
        /// <returns></returns>
        public bool ForwardCheckOfWall(Vector2 direction)
        {
            RaycastHit2D[] raycastHits = new RaycastHit2D[10];
            int hits = Physics2D.BoxCastNonAlloc(transform.position, m_StoppingDistance, GetAngle(direction), direction, raycastHits, 0.6f);

            if (hits > 0)
            {
                for (int i = 0; i < hits; i++)
                {
                    if (raycastHits[i].collider.GetComponent<Wall>() != null)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        
        public bool ForwardCheckOfWall(Vector2 direction, float distance)
        {
            RaycastHit2D[] raycastHits = new RaycastHit2D[10];
            int hits = Physics2D.BoxCastNonAlloc(transform.position, m_StoppingDistance, GetAngle(direction), direction, raycastHits, distance);

            if (hits > 0)
            {
                for (int i = 0; i < hits; i++)
                {
                    if (raycastHits[i].collider.GetComponent<Wall>() != null)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public bool ForwardCheckOfWall(Vector2 direction, float distance, Vector2 startPos)
        {
            RaycastHit2D[] raycastHits = new RaycastHit2D[10];
            int hits = Physics2D.BoxCastNonAlloc(startPos, m_StoppingDistance, GetAngle(direction), direction, raycastHits, distance);

            if (hits > 0)
            {
                for (int i = 0; i < hits; i++)
                {
                    if (raycastHits[i].collider.GetComponent<Wall>() != null)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// this one is different, returns true if the layermask is matched, false instead
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="distance"></param>
        /// <param name="startPos"></param>
        /// <param name="mask"></param>
        /// <returns></returns>
        public bool ForwardCheckOfWall(Vector2 direction, float distance, Vector2 startPos, LayerMask mask)
        {
            RaycastHit2D[] raycastHits = new RaycastHit2D[10];
            int hits = Physics2D.BoxCastNonAlloc(startPos, m_StoppingDistance, GetAngle(direction), direction, raycastHits, distance);

            if (hits > 0)
            {
                for (int i = 0; i < hits; i++)
                {
                    if (raycastHits[i] && mask.Contains(raycastHits[i].collider.gameObject.layer))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// this one is different, returns true if the layermask is matched, false instead
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="distance"></param>
        /// <param name="startPos"></param>
        /// <param name="mask"></param>
        /// <returns></returns>
        public bool ForwardCheckOfWall(Vector2 direction, float distance, Vector2 startPos, LayerMask mask, Vector3 stoppingDistance)
        {
            RaycastHit2D[] raycastHits = new RaycastHit2D[10];
            int hits = Physics2D.BoxCastNonAlloc(startPos, stoppingDistance, GetAngle(direction), direction, raycastHits, distance);

            if (hits > 0)
            {
                for (int i = 0; i < hits; i++)
                {
                    if (raycastHits[i] && mask.Contains(raycastHits[i].collider.gameObject.layer))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        

        private float GetAngle(Vector2 direction)
        {
            if (direction.x > 0)
            {
                return 90;
            }
            else if (direction.y > 0)
            {
                return 180;
            }
            else if (direction.x < 0)
            {
                return -90;
            }
            else if (direction.y < 0)
            {
                return -180;
            }
            else
            {
                return 0;
            }
        }
    }

}