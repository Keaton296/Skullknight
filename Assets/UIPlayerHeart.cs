using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Skullknight
{
    public class UIPlayerHeart : MonoBehaviour
    {
        [SerializeField] private UISpriteContainer container;
        [SerializeField] private int hpValue;
        [SerializeField] private Image img;

        public int HealthValue
        {
            get
            {
                return hpValue;
            }
            set
            {
                hpValue = Mathf.Clamp(value, 0, container.PlayerHearts.Length-1);
                img.sprite = container.PlayerHearts[hpValue];
            }
        }
    }
}
