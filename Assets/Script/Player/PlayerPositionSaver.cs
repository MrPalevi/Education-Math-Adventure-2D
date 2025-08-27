using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPositionSaver : MonoBehaviour
{
    private void Start()
    {
    
        if (PlayerPrefs.HasKey("PlayerPosX") && SceneManager.GetActiveScene().name == PlayerPrefs.GetString("LastScenePlayed"))
        {
            float x = PlayerPrefs.GetFloat("PlayerPosX");
            float y = PlayerPrefs.GetFloat("PlayerPosY");
            float z = PlayerPrefs.GetFloat("PlayerPosZ");

            transform.position = new Vector3(x, y, z);
        }
    }

    private void OnApplicationQuit()
    {
        SavePlayerPosition();
    }

    public void SavePlayerPosition()
    {
        Vector3 pos = transform.position;
        PlayerPrefs.SetFloat("PlayerPosX", pos.x);
        PlayerPrefs.SetFloat("PlayerPosY", pos.y);
        PlayerPrefs.SetFloat("PlayerPosZ", pos.z);
        PlayerPrefs.SetString("LastScenePlayed", SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();
    }

    public void ClearSavedPosition()
    {
        PlayerPrefs.DeleteKey("PlayerPosX");
        PlayerPrefs.DeleteKey("PlayerPosY");
        PlayerPrefs.DeleteKey("PlayerPosZ");
        PlayerPrefs.Save();
    }
}
