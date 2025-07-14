using UnityEngine;
using TMPro;
using System.Collections;

public class ItemManager : MonoBehaviour
{
    [Header("UI Settings")]
    public GameObject panelInfoItem;
    public TextMeshProUGUI infoItemText;
    public float infoDisplayTime = 3f;

    public IEnumerator HidePanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (panelInfoItem != null)
            panelInfoItem.SetActive(false);
    }

    public void ShowItemInfo(string itemName, string itemDescription)
    {
        if (panelInfoItem != null && infoItemText != null)
        {
            panelInfoItem.SetActive(true);
            infoItemText.text = $"ini adalah {itemName}!\nBukan {itemDescription}";
            StartCoroutine(HidePanelAfterDelay(infoDisplayTime));
        }
    }
}
