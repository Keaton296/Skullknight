using UnityEngine;

public interface IHangPoint
{
    public Collider2D HandCollider { get;}
    public bool SpriteFlip { get; }
    public Transform HoldPoint { get; }
    public Transform ClimbPoint { get; }
}