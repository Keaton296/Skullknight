using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Skullknight
{
    public class UnfocusableUI : MonoBehaviour, ICancelHandler
    {
        [SerializeField] private Selectable ReturnedSelectable;
        public void OnCancel(BaseEventData eventData)
        {
            EventSystem.current.SetSelectedGameObject(ReturnedSelectable.gameObject);
        }
    }
}
