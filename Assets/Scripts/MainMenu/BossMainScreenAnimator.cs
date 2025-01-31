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
        private SpriteRenderer _spriteRenderer;
        
        void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            StartCoroutine(Animation());
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
                var anim = transform.DOMove(points[_currentPositionIndex].position, travelLength).SetEase(Ease.Linear);
                yield return new WaitUntil(() => !anim.IsActive());
                yield return new WaitForSeconds(Random.Range(3f, maxWaitingLength));

            }
        }
    }
}
