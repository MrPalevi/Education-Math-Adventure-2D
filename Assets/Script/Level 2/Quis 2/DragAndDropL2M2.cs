using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDropL2M2 : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
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

        DropZoneL2M2 dropZoneL2M2 = eventData.pointerEnter != null ? eventData.pointerEnter.GetComponent<DropZoneL2M2>() : null;

        if (dropZoneL2M2 != null && !dropZoneL2M2.IsOccupied())
        {
            dropZoneL2M2.PlacePuzzlePiece(gameObject);
            transform.SetParent(dropZoneL2M2.transform);
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

