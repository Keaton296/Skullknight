using UnityEngine;

namespace Skullknight.Core
{
    public interface IHangPoint //could be called IInteractable
    {
        public Collider2D HandCollider { get;}
        public bool SpriteFlip { get; }
        public Transform HoldPoint { get; }
        public Transform ClimbPoint { get; }
    }
}