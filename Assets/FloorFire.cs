using System;
using System.Collections;
using System.Collections.Generic;
using Skullknight.Core;
using UnityEngine;

namespace Skullknight
{
    public class FloorFire : MonoBehaviour
    {
        [SerializeField] private LayerMask hitMask;

        private void OnTriggerStay2D(Collider2D other)
        {
            if(this.enabled) TryHitting(other);
        }

        void Awake()
        {
            this.enabled = false;
        }
        private void TryHitting(Collider2D other)
        {
            if ((hitMask & (1 << other.gameObject.layer)) != 0 && !other.isTrigger)
            {
                var damageable = other.GetComponent<IDamageable>();
                if (damageable != null && damageable.IsDamageable)
                {
                    damageable.TakeDamage(10);
                }
            }
        }
    }
}
