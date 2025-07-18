using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDropL4M2 : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 _startPosition;
    private Transform _startParent;
    private CanvasGroup _canvasGroup;

    public int puzzleID;

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

        DropZoneL4M2 dropZoneL4M2 = eventData.pointerEnter != null ? eventData.pointerEnter.GetComponent<DropZoneL4M2>() : null;

        if (dropZoneL4M2 != null && !dropZoneL4M2.IsOccupied())
        {
            dropZoneL4M2.PlacePuzzlePiece(gameObject);
            transform.SetParent(dropZoneL4M2.transform);
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

