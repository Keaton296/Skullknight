using UnityEngine;

namespace Skullknight.Core
{
    public class HangPoint : MonoBehaviour,IHangPoint
    {
        [SerializeField] private Transform m_holdPoint;
        [SerializeField] private Transform m_climbPoint;
        [SerializeField] private BoxCollider2D m_collider;
        [SerializeField] private bool m_spriteFlip = false;
        public Collider2D HandCollider => m_collider;
        public bool SpriteFlip => m_spriteFlip;
        public Transform HoldPoint => m_holdPoint;
        public Transform ClimbPoint => m_climbPoint;
    }
}