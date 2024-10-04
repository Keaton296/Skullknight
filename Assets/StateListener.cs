using UnityEngine;
using TMPro;
public class StateListener : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    void Update()
    {
        text.text = PlayerController.Instance.PlayerState.ToString();
    }
}
