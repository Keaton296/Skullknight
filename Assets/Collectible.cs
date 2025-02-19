using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Skullknight
{
    public class Collectible : MonoBehaviour
    {
        public CollectibleSO collectibleData;
        public static float randomForceAmount = 2f;
        private bool isRbSleepingBuffer = false;
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private AudioSource audioSource;

        public void Collect()
        {
            collectibleData.OnCollect();
            Destroy(gameObject);
        }
        void Start()
        {
            rb.AddForce(new Vector2(Random.Range(-1,1f) * collectibleData.SpawnForce,Random.Range(-1,1f) * collectibleData.SpawnForce),ForceMode2D.Impulse);
            audioSource.clip = collectibleData.SpawnSound;
            audioSource.Play();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if ((collectibleData.CollectableLayer & (1 << other.gameObject.layer)) != 0)
            {
                Collect();
            }
            else
            {
                audioSource.clip = collectibleData.CollisionSound; 
                audioSource.Play();
            }
        }
        
    }
}
