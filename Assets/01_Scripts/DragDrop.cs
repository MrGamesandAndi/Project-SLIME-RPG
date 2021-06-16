using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [Header("Element Configuration")]
    [Range(0, 1)]
    [SerializeField] float _alpha = 0.6f; // Alpha value of the element.

    // Parent where the element will return to in case the element is dropped in an invalid area.
    Transform _parentToReturnTo = null;

    public Transform parentToReturnTo
    {
        set
        {
            _parentToReturnTo = value;
        }

        get
        {
            return _parentToReturnTo;
        }
    }

    // Prevents a dragging element from parenting to a Player Drop Area if said element wasn't dropped. 
    Transform _placeholderParent = null; 

    public Transform placeholderParent
    {
        set
        {
            _placeholderParent = value;
        }
        get
        {
            return _placeholderParent;
        }
    }

    RectTransform _rectTransform; // Used for grabbing an element.
    CanvasGroup _canvasGroup; // Used to edit the element's alpha.
    GameObject _placeholderElement; // Phantom element to highlight the position of a dragged element.
    Canvas _canvas; // Current canvas.

    private void Awake()
    {
        _rectTransform = GetComponentInChildren<RectTransform>();
        _canvasGroup = GetComponentInChildren<CanvasGroup>();
        _canvas = FindObjectOfType<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _placeholderElement = new GameObject();
        _placeholderElement.transform.SetParent(transform.parent);
        LayoutElement layoutElement = _placeholderElement.AddComponent<LayoutElement>();
        layoutElement.preferredWidth = GetComponentInChildren<LayoutElement>().preferredWidth;
        layoutElement.preferredHeight = GetComponentInChildren<LayoutElement>().preferredHeight;
        layoutElement.flexibleWidth = 0f;
        layoutElement.flexibleHeight = 0f;
        _placeholderElement.transform.SetSiblingIndex(transform.GetSiblingIndex());
        _parentToReturnTo = transform.parent;
        _placeholderParent = _parentToReturnTo;
        transform.SetParent(transform.parent.parent);
        _canvasGroup.alpha = _alpha;
        _canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;

        if (_placeholderElement.transform.parent != _placeholderParent)
        {
            _placeholderElement.transform.SetParent(_placeholderParent);
        }

        int newSiblingIndex = _placeholderParent.childCount;

        for (int i = 0; i < _placeholderParent.childCount; i++)
        {
            if (transform.position.x < _placeholderParent.GetChild(i).position.x)
            {
                newSiblingIndex = i;

                if (_placeholderElement.transform.GetSiblingIndex() < newSiblingIndex)
                {
                    newSiblingIndex--;
                }

                break;
            }
        }

        _placeholderElement.transform.SetSiblingIndex(newSiblingIndex);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.alpha = 1f;
        _canvasGroup.blocksRaycasts = true;
        transform.SetParent(_parentToReturnTo);
        transform.SetSiblingIndex(_placeholderElement.transform.GetSiblingIndex());
        Destroy(_placeholderElement);
    }
}
