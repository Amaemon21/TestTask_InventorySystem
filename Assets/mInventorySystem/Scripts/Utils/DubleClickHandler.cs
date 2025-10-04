using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DubleClickHandler : MonoBehaviour , IPointerClickHandler
{
    private const float DoubleClickThreshold = 0.3f;
    private float _lastClickTime;
    
    public event Action OnDubleClick;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        float timeSinceLastClick = Time.time - _lastClickTime;

        if (timeSinceLastClick <= DoubleClickThreshold)
        {
            OnDoubleClick(eventData);
            _lastClickTime = 0f; // сброс
        }
        else
        {
            OnSingleClick(eventData);
            _lastClickTime = Time.time;
        }
    }
    
    private void OnSingleClick(PointerEventData eventData)
    {
    }

    private void OnDoubleClick(PointerEventData eventData)
    {
        OnDubleClick?.Invoke();
    }
}
