using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
   
    [SerializeField] [Range(0, 1)] private float alpha = 0.6f;
    [HideInInspector] public Transform parentToReturnTo = null;
    [HideInInspector] public Transform placeholderParent;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private GameObject placeholderElement;
    private Canvas canvas;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = FindObjectOfType<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        placeholderElement = new GameObject();
        placeholderElement.transform.SetParent(transform.parent);
        LayoutElement layoutElement = placeholderElement.AddComponent<LayoutElement>();
        layoutElement.preferredWidth = GetComponent<LayoutElement>().preferredWidth;
        layoutElement.preferredHeight = GetComponent<LayoutElement>().preferredHeight;
        layoutElement.flexibleWidth = 0f;
        layoutElement.flexibleHeight = 0f;
        placeholderElement.transform.SetSiblingIndex(transform.GetSiblingIndex());
        parentToReturnTo = transform.parent;
        placeholderParent = parentToReturnTo;
        transform.SetParent(transform.parent.parent);
        canvasGroup.alpha = alpha;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

        if (placeholderElement.transform.parent != placeholderParent)
        {
            placeholderElement.transform.SetParent(placeholderParent);
        }

        int newSiblingIndex = placeholderParent.childCount;

        for (int i = 0; i < placeholderParent.childCount; i++)
        {
            if (transform.position.x < placeholderParent.GetChild(i).position.x)
            {
                newSiblingIndex = i;

                if (placeholderElement.transform.GetSiblingIndex() < newSiblingIndex)
                {
                    newSiblingIndex--;
                }

                break;
            }
        }

        placeholderElement.transform.SetSiblingIndex(newSiblingIndex);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        transform.SetParent(parentToReturnTo);
        transform.SetSiblingIndex(placeholderElement.transform.GetSiblingIndex());
        Destroy(placeholderElement);
    }
}
