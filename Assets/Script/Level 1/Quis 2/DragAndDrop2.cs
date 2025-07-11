using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class DragAndDrop2 : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 _startPosition;
    private Transform _startParent;
    private CanvasGroup _canvasGroup;
    private bool _isLocked = false; // Menandakan apakah puzzle telah dikunci di DropZone

    void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_isLocked) return; // Jangan izinkan drag jika sudah terkunci

        _startPosition = transform.position; // Simpan posisi awal
        _startParent = transform.parent;     // Simpan parent awal
        _canvasGroup.blocksRaycasts = false; // Agar bisa melewati collider dropzone
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_isLocked) return; // Jangan izinkan drag jika sudah terkunci

        // Mengikuti posisi mouse
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = true; // Kembalikan agar dapat dideteksi collider lagi

        if (_isLocked) return; // Jangan lakukan apapun jika sudah terkunci

        // Cek apakah drop zone valid
        DropZone2 dropZone = eventData.pointerEnter != null ? eventData.pointerEnter.GetComponent<DropZone2>() : null;
        if (dropZone != null)
        {
            // Pindahkan puzzle ke posisi tengah DropZone
            transform.SetParent(dropZone.transform);

            // Menyesuaikan posisi puzzle agar berada di tengah DropZone
            RectTransform dropZoneRect = dropZone.GetComponent<RectTransform>();
            Vector2 dropZoneCenter = dropZoneRect.rect.center;

            // Sesuaikan posisi puzzle di tengah DropZone
            transform.localPosition = new Vector3(dropZoneCenter.x, dropZoneCenter.y, 0);

            // Kunci puzzle di tempat
            _isLocked = true; 
        }
        else
        {
            // Jika tidak ada dropzone valid, kembalikan ke posisi awal
            transform.position = _startPosition;
            transform.SetParent(_startParent);
        }
    }

    public void ResetLockState()
    {
        _isLocked = false;
    }

}

