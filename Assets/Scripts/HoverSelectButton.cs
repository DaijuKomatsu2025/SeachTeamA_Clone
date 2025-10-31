using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

public class HoverSelectButton : MonoBehaviour, IPointerEnterHandler, ISelectHandler
{
    public UnityEvent onSelected;

    public void OnPointerEnter(PointerEventData eventData)
    {
        // このボタンを選択状態にする
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void OnSelect(BaseEventData eventData)
    {
        //Debug.Log($"{gameObject.name} が選択されました");
        onSelected?.Invoke();
    }

    void OnDisable()
    {
        onSelected.RemoveAllListeners();
    }
}

