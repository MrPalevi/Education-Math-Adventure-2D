using UnityEngine;
using TMPro;

public class PencilCollector : MonoBehaviour
{
    public static PencilCollector instance;

    public int totalPencilsToCollect = 5;
    private int currentCollected = 0;

    public TMP_Text countText;
    public GameObject collectButton;

    public GameObject returnToNPCText;
    public GameObject chatBoxComplete;
    
    private GameObject currentPencil;
    private bool canCollect = false;
    private bool missionStarted = false;

    private void Awake()
    {
        instance = this;
    }

    public void StartCollecting()
    {
        missionStarted = true;
        currentCollected = 0;
        UpdateUI();
    }

    void Update()
    {
        if (canCollect && Input.GetKeyDown(KeyCode.E))
        {
            TakePencil();
        }
    }

    public void ShowCollectButton(GameObject pencil)
    {
        currentPencil = pencil;
        canCollect = true;
        collectButton.SetActive(true);
    }

    public void HideCollectButton()
    {
        currentPencil = null;
        canCollect = false;
        collectButton.SetActive(false);
    }

    public void TakePencil()
    {
        if (currentPencil != null)
        {
            Destroy(currentPencil);
            currentCollected++;
            UpdateUI();
            HideCollectButton();

            if (currentCollected >= totalPencilsToCollect)
            {
                returnToNPCText.SetActive(true);
            }
        }
    }
    
    public bool HasCompletedMission()
    {
        return currentCollected >= totalPencilsToCollect;
    }

    void UpdateUI()
    {
        countText.text = currentCollected + " / " + totalPencilsToCollect;
    }

    public bool IsMissionStarted()
    {
        return missionStarted;
    }

    public void CompleteMission()
    {
        chatBoxComplete.SetActive(true);
    }
}
