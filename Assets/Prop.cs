using System.Collections;
using System.Collections.Generic;
using Skullknight.Core;
using UnityEngine;
using UnityEngine.Events;

namespace Skullknight
{
    public class Prop : MonoBehaviour,IDamageable
    {
        [SerializeField] private int health = 1;
        [SerializeField] private int maxHealth = 1;
        [SerializeField] private bool isDamageable = true;
        public int Health => health;
        public int MaxHealth => maxHealth;
        public bool IsDamageable => isDamageable;
        public bool TakeDamage(int amount)
        {
            health = Mathf.Clamp(health - amount, 0, maxHealth);
            if (health == 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    Instantiate(GameManager.Instance.coinPrefab, transform.position, Quaternion.identity);
                }
                Destroy(gameObject);
                return true;
            }

            return false;
        }
        
        public UnityEvent<int> OnHealthChanged { get; }
    }
}
