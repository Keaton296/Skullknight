using System.Collections;
using Cinemachine;
using Skullknight.State;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.XR;

namespace Skullknight.Enemy.Skeleton
{
    public class SkeletonPatrolState : EntityState<Skullknight.Skeleton.ESkeletonState,Skullknight.Skeleton>
    {
        public SkeletonPatrolState(Skullknight.Skeleton _controler) : base(_controler)
        {
        }

        public override void EnterState()
        {
            controller.SetDamageImmunity(false);
            controller.Animator.Play("Walking");
        }

        public override void ExitState()
        {
            
        }

        public override void StateUpdate()
        {
                if(controller.currentWaypointIndex == 1)
                {
                    controller.FlipX(false);
                    if (controller.splinePosition < 1f)
                    {
                        controller.splinePosition +=
                            Time.deltaTime * controller.evaluationRate * controller.currentWaypointIndex;
                        controller.transform.position =
                            controller.splineContainer.Spline.EvaluatePosition(controller.splinePosition);
                    }
                    else
                    {
                        controller.ChangeState(Skullknight.Skeleton.ESkeletonState.Waiting);
                    }
                }
                else if (controller.currentWaypointIndex == -1)
                {
                    controller.FlipX(true);
                    if (controller.splinePosition > 0f)
                    {
                        controller.splinePosition +=
                            Time.deltaTime * controller.evaluationRate * controller.currentWaypointIndex;
                        controller.transform.position =
                            controller.splineContainer.Spline.EvaluatePosition(controller.splinePosition);
                    }
                    else
                    {
                        controller.ChangeState(Skullknight.Skeleton.ESkeletonState.Waiting);
                    }
                }
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