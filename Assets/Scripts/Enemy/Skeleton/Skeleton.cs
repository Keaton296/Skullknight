using System.Collections;
using System.Collections.Generic;
using Skullknight.Core;
using Skullknight.Enemy.Skeleton;
using Skullknight.Entity;
using Skullknight.Player.Statemachine;
using Skullknight.State;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.Splines;

namespace Skullknight
{
    public class Skeleton : EntityController<Skeleton.ESkeletonState,Skeleton>,IDamageable
    {
        public BoxCollider2D collider;
        [SerializeField] private Collider2D atkCollider;
        [SerializeField] private LayerMask atkLayer;
        [SerializeField] private Rigidbody2D rb;
        public SplineContainer splineContainer;
        public int currentWaypointIndex = -1;
        public float evaluationRate;
        public float splinePosition;
        public float attackingDistance = .1f;
        public bool chasing = false;
        public enum ESkeletonState
        {
            Patrol,
            Waiting,
            Attacking,
            Hurt,
            Chasing,
            Death
        }

        public void SetChasing(bool val)
        {
            if(stateEnum != ESkeletonState.Death)
            {
                this.chasing = val;
                if (val)
                {
                    ChangeState(ESkeletonState.Chasing);
                }
                else
                {
                    ChangeState(ESkeletonState.Patrol);
                }
            }
        }
        private void Awake()
        {
            states.Add(ESkeletonState.Waiting, new SkeletonWaitingState(this));
            states.Add(ESkeletonState.Patrol, new SkeletonPatrolState(this));
            states.Add(ESkeletonState.Hurt, new SkeletonHurtState(this));
            states.Add(ESkeletonState.Death, new SkeletonDeathState(this));
            states.Add(ESkeletonState.Chasing, new SkeletonChasingState(this));
            states.Add(ESkeletonState.Attacking, new SkeletonAttackingState(this));
        }

        public override bool TakeDamage(int amount)
        {
            if (isDamageable)
            {
                health = Mathf.Clamp(health - amount, 0, maxHealth);
                onHealthChanged?.Invoke(health);
                if (health == 0) ChangeState(ESkeletonState.Death);
                else ChangeState(ESkeletonState.Hurt);
                return true;
            }
            return false;
        }
        
        private void Start()
        {
            ChangeState(ESkeletonState.Patrol);
        }

        public void Attack()
        {
            RaycastHit2D hit = Physics2D.BoxCast(atkCollider.bounds.center, atkCollider.bounds.size, 0f, Vector3.right, .1f,
                atkLayer);
            if (hit.collider == null) return;
            var player = hit.collider.GetComponent<IDamageable>();
            if (player != null)
            {
                player.TakeDamage(1);
            }
        }

        public void AttackPrecise()
        {
            RaycastHit2D[] hits = new RaycastHit2D[1];
            rb.Cast(spriteRenderer.flipX ? -Vector2.right : Vector2.right, hits );
            if (hits[0].collider == null) return;
            var player = hits[0].collider.GetComponent<IDamageable>();
            if (player != null)
            {
                player.TakeDamage(1);   
            }
        }

        public override void FlipX(bool flip)
        {
            spriteRenderer.flipX = flip;
            atkCollider.transform.localScale = new Vector3(flip ? -1 : 1, atkCollider.transform.localScale.y, atkCollider.transform.localScale.z);
        }

        public void SetDamageImmunity(bool isImmune)
        {
            isDamageable = !isImmune;
        }
        public void ChangeWaypoint()
        {
            switch (currentWaypointIndex)
            {
                case -1:
                    currentWaypointIndex = 1;
                    break;
                case 1:
                    currentWaypointIndex = -1;
                    break;
            }
        }
    }
}
