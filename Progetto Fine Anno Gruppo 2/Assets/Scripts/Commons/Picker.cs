using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Commons
{
    public class Picker : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Pickable p = collision.gameObject.GetComponent<Pickable>();
            if(p != null)
            {
                p.Picked();
            }
        }
    }
}

