using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDropL5M1 : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 _startPosition;
    private Transform _startParent;
    private CanvasGroup _canvasGroup;

    void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _startPosition = transform.position;
        _startParent = transform.parent;
        _canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = true;

        DropZoneL5M1 dropZoneL5M1 = eventData.pointerEnter != null ? eventData.pointerEnter.GetComponent<DropZoneL5M1>() : null;

        if (dropZoneL5M1 != null && !dropZoneL5M1.IsOccupied())
        {
            dropZoneL5M1.PlacePuzzlePiece(gameObject);
            transform.SetParent(dropZoneL5M1.transform);
            transform.localPosition = Vector3.zero;
        }
        else
        {
            ResetPosition();
        }
    }

    private void ResetPosition()
    {
        transform.position = _startPosition;
        transform.SetParent(_startParent);
    }
}
