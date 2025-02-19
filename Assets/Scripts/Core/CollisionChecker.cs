using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Skullknight.Core
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CollisionChecker : MonoBehaviour
    {
        [SerializeField] LayerMask checkLayerMask;
        [FormerlySerializedAs("inTrigger")] public bool IsColliding;
        public Action<Collider2D> onCollisionEnter2D;
        public Action<Collider2D> onTriggerEnter2D;
        public UnityEvent onTriggerEnter;
        public UnityEvent onTriggerExit;
        void OnTriggerEnter2D(Collider2D other)
        {
            if((checkLayerMask & (1 << other.gameObject.layer)) != 0)
            {
                onTriggerEnter2D?.Invoke(other);
                //Debug.Log("deneme from " + gameObject.name);
                onTriggerEnter?.Invoke();
                IsColliding = true;
            }
        }
        void OnTriggerExit2D(Collider2D other)
        {
            if ((checkLayerMask & (1 << other.gameObject.layer)) != 0)
            {
                onTriggerExit?.Invoke();
                IsColliding = false;

            }
        }

    }
}
