
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Skullknight.Core
{
    public class PlayerCollisionChecker : MonoBehaviour
    {
        [SerializeField] LayerMask checkLayerMask;
        public bool IsColliding;
        public Action<Collider2D> onCollisionEnter2D;
        public Action<Collider2D> onTriggerEnter2D;
        public UnityEvent onTriggerEnter;
        public UnityEvent onTriggerExit;
        
        void OnTriggerEnter2D(Collider2D other)
        {
            if(((checkLayerMask & (1 << other.gameObject.layer)) != 0) && other.isTrigger)
            {
                onTriggerEnter2D?.Invoke(other);
                onTriggerEnter?.Invoke();
                IsColliding = true;
            }
        }
        void OnTriggerExit2D(Collider2D other)
        {
            if (((checkLayerMask & (1 << other.gameObject.layer)) != 0) && other.isTrigger)
            {
                onTriggerExit?.Invoke();
                IsColliding = false;

            }
        }
    }
}