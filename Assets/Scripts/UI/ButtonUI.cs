using Unity.VisualScripting;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Skullknight
{
    public class ButtonUI : Button
    {
        
        public UnityEvent m_onDeselectEvent;
        public UnityEvent m_onSelectEvent;
        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);
            m_onDeselectEvent?.Invoke();
        }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            m_onSelectEvent?.Invoke();
        }
        
    }
}
