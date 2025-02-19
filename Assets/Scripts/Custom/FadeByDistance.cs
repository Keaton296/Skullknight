using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace Skullknight
{
    public class FadeByDistance : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private float fadeDistance = 5f; // Maximum distance to start fading
        [SerializeField] private float minOpacity = 0f;
        [SerializeField] private float maxOpacity = 1f;

        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private TMP_Text text;

        void Update()
        {
            if (player == null) return;
        
            float distance = Vector2.Distance(transform.position, player.position);
            float alpha = Mathf.Lerp(maxOpacity, minOpacity, distance / fadeDistance);
        
            // Clamp alpha between min and max values
            alpha = Mathf.Clamp(alpha, minOpacity, maxOpacity);
        
            if(spriteRenderer != null)
            {
                Color color = spriteRenderer.color;
                color.a = alpha;
                spriteRenderer.color = color;
            }
            if(text != null)
            {
                Color color2 = text.color;
                color2.a = alpha;
                text.color = color2;
            }
        }
    }
}
