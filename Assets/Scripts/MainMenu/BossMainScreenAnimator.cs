using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Skullknight
{
    public class BossMainScreenAnimator : MonoBehaviour
    {
        [SerializeField] private Transform[] points;
        private readonly float maxWaitingLength = 5f;
        private readonly float travelLength = 9f;
        private int _currentPositionIndex = 0;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        private Tween movementTween;
        
        void Start()
        {
            StartCoroutine(Animation());
        }

        public void Terminate()
        {
            movementTween.Kill();
            StopAllCoroutines();
        }

        IEnumerator Animation()
        {
            while (true)
            {
                if(_currentPositionIndex == 1)
                {
                    _spriteRenderer.flipX = true;
                    _currentPositionIndex = 0;
                }
                else
                {
                    _spriteRenderer.flipX = false;
                    _currentPositionIndex = 1;
                }
                movementTween = transform.DOMove(points[_currentPositionIndex].position, travelLength).SetEase(Ease.Linear);
                yield return new WaitUntil(() => !movementTween.IsActive());
                yield return new WaitForSeconds(Random.Range(3f, maxWaitingLength));

            }
        }
    }
}
