using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDropL3M1 : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
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

        DropZoneL3M1 dropZoneL3M1 = eventData.pointerEnter != null ? eventData.pointerEnter.GetComponent<DropZoneL3M1>() : null;

        if (dropZoneL3M1 != null && !dropZoneL3M1.IsOccupied())
        {
            dropZoneL3M1.PlacePuzzlePiece(gameObject);
            transform.SetParent(dropZoneL3M1.transform);
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

