using System.Collections;
using Cinemachine;
using UnityEngine;
using DG.Tweening;
using xKhano.StateMachines.Entities;

namespace Skullknight.Enemy.Skeleton
{
    public class SkeletonDeathState : EntityState<Skullknight.Skeleton.ESkeletonState,Skullknight.Skeleton>
    {
        public SkeletonDeathState(Skullknight.Skeleton _controler) : base(_controler)
        { }

        private IEnumerator Death(float duration)
        {
            for (int i = 0; i < 5; i++)
            {
                GameObject.Instantiate(GameManager.Instance.coinPrefab,controller.transform.position,Quaternion.identity);
            }
            controller.SetDamageImmunity(true);
            controller.Animator.Play("Death");
            yield return new WaitForSeconds(duration);
            controller.SpriteRenderer.DOFade(0f, 1f);
            yield return new WaitForSeconds(1f);
            GameObject.Destroy(controller.gameObject);
        }

        public override void EnterState()
        {
            controller.collider.enabled = false;
            coroutines.Add(controller.StartCoroutine(Death(3f)));
        }

        public override void ExitState()
        {
            
        }

        public override void StateUpdate()
        {
            
        }

        public override void StateFixedUpdate()
        {
            
        }

        public override void SubscribeEvents()
        {
            
        }

        public override void UnsubscribeEvents()
        {
            
        }
    }
}