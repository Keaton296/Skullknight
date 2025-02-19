using System;
using Skullknight.Core;
using Skullknight.State;
using UnityEngine;
using UnityEngine.Events;

namespace Skullknight.Entity
{
    public abstract class EntityController<EState, Controller> : EntityStateManager<EState,EntityState<EState,Controller>,Controller>, IDamageable where EState : Enum where Controller : MonoBehaviour
    {
        [SerializeField] protected Animator animator;
        [SerializeField] protected SpriteRenderer spriteRenderer;
        public Animator Animator => animator;
        public SpriteRenderer SpriteRenderer => spriteRenderer;
        
        protected int health = 100;
        protected int maxHealth = 100;
        protected bool isDamageable = true;
        protected UnityEvent<int> onHealthChanged;
        
        
        public bool IsDamageable => isDamageable;
        public int Health => health;
        public int MaxHealth => maxHealth;
        public UnityEvent<int> OnHealthChanged => onHealthChanged;
        
        public virtual bool TakeDamage(int amount)
        {
            if (isDamageable)
            {
                health = Mathf.Clamp(health - amount, 0, maxHealth);
                onHealthChanged?.Invoke(health);
                return true;
            }
            return false;
        }
        public virtual void FlipX(bool isFlipped)
        {
            spriteRenderer.flipX = isFlipped;
        }
    }
}