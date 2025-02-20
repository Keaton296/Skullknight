using System.Collections;
using System.Collections.Generic;
using Skullknight.Core;
using Skullknight.Player.Statemachine;
using UnityEngine;

namespace Skullknight
{
    public class FireBreath : MonoBehaviour
    {
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private BoxCollider2D collider;
        public void GiveDamage(int amount)
        {
            RaycastHit2D hit = Physics2D.BoxCast(collider.bounds.center, collider.bounds.size, 0f, Vector3.zero, .1f,
                layerMask);
            if (hit.collider == null) return;
            var player = hit.collider.GetComponent<IDamageable>();
            if (player != null)
            {
                player.TakeDamage(amount);
            }
        }
    }
}
