using UnityEngine;
using TMPro;

public class ProgresManager : MonoBehaviour
{
    [System.Serializable]
    public class LevelProgressData
    {
        public string sceneName;                      // Nama scene
        public string[] checkpointIDs;                // Daftar checkpoint ID
        public TMP_Text textPersen;                   // Menampilkan progres persen

        public string[] playerPrefsScoreFinalKeys;    // Daftar key PlayerPrefs untuk skor
        public TMP_Text[] textScores;                 // TMP untuk tiap skor (urutan sesuai key)
    }

    public LevelProgressData[] levelProgressList;

    private const string prefsKey = "PlayerPrefsCheckPoint";
    private const string sceneKey = "LastScenePlayed";

    void Start()
    {
        InvokeRepeating(nameof(UpdateProgressUI), 0f, 0.5f);
    }

    void UpdateProgressUI()
    {
        string currentScene = PlayerPrefs.GetString(sceneKey, "");
        string lastCheckpointID = PlayerPrefs.GetString(prefsKey, "");

        foreach (var data in levelProgressList)
        {
            // Update Persentase Progress
            if (data.textPersen != null && data.checkpointIDs != null && data.checkpointIDs.Length > 0)
            {
                if (data.sceneName == currentScene)
                    {
                        int index = GetCheckpointIndex(data.checkpointIDs, lastCheckpointID);
                        if (index >= 0)
                        {
                            float progress = ((index + 1f) / data.checkpointIDs.Length) * 100f;
                            data.textPersen.text = $"{progress:0}%";
                        }
                        else
                        {
                            data.textPersen.text = "0%";
                        }
                    }
                    else
                    {
                        data.textPersen.text = "0%";
                    }
            }

            // Update Score dari Array Key
            if (data.playerPrefsScoreFinalKeys != null && data.textScores != null)
            {
                int count = Mathf.Min(data.playerPrefsScoreFinalKeys.Length, data.textScores.Length);
                for (int i = 0; i < count; i++)
                {
                    string key = data.playerPrefsScoreFinalKeys[i];
                    TMP_Text scoreText = data.textScores[i];

                    if (!string.IsNullOrEmpty(key) && scoreText != null)
                    {
                        int score = PlayerPrefs.GetInt(key, 0);
                        scoreText.text = $"Score: {score}";
                    }
                }
            }
        }
    }

    private int GetCheckpointIndex(string[] checkpoints, string currentID)
    {
        for (int i = 0; i < checkpoints.Length; i++)
        {
            if (checkpoints[i] == currentID)
                return i;
        }
        return -1;
    }
}
