using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

namespace Skullknight
{
    [CreateAssetMenu(fileName = "CollectibleSO", menuName = "ScriptableObjects/CollectibleSO")]
    public class CollectibleSO : ScriptableObject
    {
        [SerializeField] protected float spawnForce = 2f;
        [SerializeField] protected int collectibleValue = 1;
        [SerializeField] protected LayerMask collectableLayer;

        [SerializeField] protected AnimatorController animatorControllerAsset; 
        
        [SerializeField] protected AudioClip collectingSound;
        [SerializeField] protected AudioClip spawnSound;
        [SerializeField] protected AudioClip collisionSound;
        
        public float SpawnForce => spawnForce;  
        public LayerMask CollectableLayer => collectableLayer;
        
        public AnimatorController AnimatorControllerAsset => animatorControllerAsset;
        
        public AudioClip CollectingSound => collectingSound;
        public AudioClip SpawnSound => spawnSound;
        public AudioClip CollisionSound => collisionSound;

        public virtual void OnCollect()
        {
            GameManager.Instance?.SetLevelCoins(GameManager.Instance.LevelCoins + collectibleValue);
        }
        public virtual void OnRigidbodySleep(Collectible collectible){}
        public virtual void RigidbodySleep(Collectible collectible){}
        public virtual void RigidbodyWake(Collectible collectible){}
        public virtual void OnRigidbodyWake(Collectible collectible){}
    }
}
