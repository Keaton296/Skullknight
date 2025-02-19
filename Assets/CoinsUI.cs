using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Skullknight
{
    public class CoinsUI : MonoBehaviour
    {
        [SerializeField] TMP_Text coinsText;
        void Start()
        {
            GameManager.Instance?.OnLevelCoinsChanged.AddListener(OnCoinsChanged);
        }

        private void OnCoinsChanged(int arg0)
        {
            coinsText.text = arg0.ToString();
        }
    }
}
