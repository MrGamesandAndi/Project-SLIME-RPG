using UnityEngine;
using UnityEngine.EventSystems;

public class ElementSlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            DragDrop dragScript = eventData.pointerDrag.GetComponent<DragDrop>();

            if (dragScript != null)
            {
                dragScript.parentToReturnTo = transform;
            }

        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
        {
            return;
        }

        DragDrop dragScript = eventData.pointerDrag.GetComponent<DragDrop>();

        if (dragScript != null)
        {
            dragScript.placeholderParent = transform;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
        {
            return;
        }

        DragDrop dragScript = eventData.pointerDrag.GetComponent<DragDrop>();

        if (dragScript != null && dragScript.placeholderParent == transform)
        {
            dragScript.placeholderParent = dragScript.parentToReturnTo;
        }
    }
}
