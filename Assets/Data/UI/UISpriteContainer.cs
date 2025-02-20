using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skullknight
{
    [CreateAssetMenu(fileName = "UISpriteContainer", menuName = "Skullknight/UISpriteContainer")]
    public class UISpriteContainer : ScriptableObject
    {
        [SerializeField] private Sprite[] playerHearts;
        public Sprite[] PlayerHearts => playerHearts;
    }
}
