using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDropL7M4 : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
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

        DropZoneL7M4 dropZoneL7M4 = eventData.pointerEnter != null ? eventData.pointerEnter.GetComponent<DropZoneL7M4>() : null;

        if (dropZoneL7M4 != null && !dropZoneL7M4.IsOccupied())
        {
            dropZoneL7M4.PlacePuzzlePiece(gameObject);
            transform.SetParent(dropZoneL7M4.transform);
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


