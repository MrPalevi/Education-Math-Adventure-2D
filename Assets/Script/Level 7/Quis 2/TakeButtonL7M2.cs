using UnityEngine;
using UnityEngine.UI;

public class TakeButtonL7M2 : MonoBehaviour
{
    public static TakeButtonL7M2 instance;

    public Button takeButton;
    private SoalItemL7M2 currentItem;

    void Awake()
    {
        instance = this;
        takeButton.gameObject.SetActive(false);
        takeButton.onClick.AddListener(AmbilItem);
    }

    public void RegisterItem(SoalItemL7M2 item)
    {
        currentItem = item;
        takeButton.gameObject.SetActive(true);
    }

    public void UnregisterItem(SoalItemL7M2 item)
    {
        if (currentItem == item)
        {
            currentItem = null;
            takeButton.gameObject.SetActive(false);
        }
    }

    void AmbilItem()
    {
        if (currentItem != null)
        {
            currentItem.AmbilItem();
            currentItem = null;
            takeButton.gameObject.SetActive(false);
        }
    }
}
