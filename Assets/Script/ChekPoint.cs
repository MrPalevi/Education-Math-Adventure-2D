using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPoint : MonoBehaviour
{
    public string checkpointID;          // ID unik checkpoint ini
    public string sceneName;             // Nama scene tempat checkpoint ini
    public Sprite spriteOff;             // Sebelum disentuh
    public Sprite spriteOn;              // Setelah disentuh

    private SpriteRenderer spriteRenderer;
    private bool activated = false;

    private const string prefsKey = "PlayerPrefsCheckPoint";
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null && spriteOff != null)
            spriteRenderer.sprite = spriteOff;

        // Aktifkan checkpoint jika ini adalah yang terakhir disimpan
        if (PlayerPrefs.GetString(prefsKey, "") == checkpointID)
        {
            Vector3 pos = transform.position;
            if (pos.x < 0) pos.x = 0f; // koreksi posisi X

            PlayerManager.lastCheckPointPos = pos;
            activated = true;

            if (spriteRenderer != null && spriteOn != null)
                spriteRenderer.sprite = spriteOn;
                
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!activated && collision.CompareTag("Player"))
        {
            Debug.Log("Player menyentuh checkpoint");

            // Simpan scene dan ID checkpoint ke PlayerPrefs
            PlayerPrefs.SetString("LastScenePlayed", sceneName);
            PlayerPrefs.SetString(prefsKey, checkpointID);

            // Simpan posisi checkpoint (koreksi X jika negatif)
            Vector3 pos = transform.position;
            if (pos.x < 0) pos.x = 0f;
            PlayerManager.lastCheckPointPos = pos;

            PlayerPrefs.Save();

            activated = true;

            if (spriteRenderer != null && spriteOn != null)
                spriteRenderer.sprite = spriteOn;
                audioManager.PlaySFX(audioManager.Cekpoint);
        }
    }

    public static void ResetCheckpointInCurrentScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        // Cari semua checkpoint di scene ini
        CheckPoint[] allCheckpoints = FindObjectsOfType<CheckPoint>();

        foreach (CheckPoint cp in allCheckpoints)
        {
            if (cp.sceneName == currentScene)
            {
                // Jika checkpoint ini yang sedang disimpan, hapus PlayerPrefs-nya
                if (PlayerPrefs.GetString(prefsKey, "") == cp.checkpointID)
                {
                    PlayerPrefs.DeleteKey(prefsKey);
                    PlayerPrefs.DeleteKey("LastScenePlayed");
                    PlayerPrefs.Save();
                    Debug.Log($"Checkpoint {cp.checkpointID} di scene {currentScene} direset.");
                }

                // Reset sprite dan statusnya
                cp.activated = false;
                if (cp.spriteRenderer != null && cp.spriteOff != null)
                    cp.spriteRenderer.sprite = cp.spriteOff;
            }
        }
    }
}
