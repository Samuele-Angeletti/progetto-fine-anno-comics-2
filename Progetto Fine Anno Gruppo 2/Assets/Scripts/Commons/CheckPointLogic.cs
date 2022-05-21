using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGame;
namespace Commons
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class CheckPointLogic : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            PlayerMovementManager p = collision.GetComponent<PlayerMovementManager>();
            if(p != null)
            {
                if (p.NextCheckpoint != transform.position)
                {
                    PubSub.PubSub.Publish(new CheckPointMessage(transform.position, this));
                }
            }
        }
    }
}
