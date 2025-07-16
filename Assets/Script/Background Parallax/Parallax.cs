using UnityEngine;

public class Parallax : MonoBehaviour
{
    [Header("Referensi Kamera dan Background")]
    public Transform mainCam;
    public Transform middleBG;
    public Transform sideBG;

    [Header("Ukuran Background")]
    public float length = 28.72f; // Panjang sprite background secara horizontal

    [Header("Efek Parallax (Multiplier)")]
    public Vector2 parallaxMultiplier = new Vector2(0.5f, 0.5f); // X dan Y

    private Vector3 lastCamPosition;

    void Start()
    {
        if (mainCam == null)
            mainCam = Camera.main.transform;

        lastCamPosition = mainCam.position;
    }

    void Update()
    {
        // Hitung delta gerak kamera
        Vector3 deltaMovement = mainCam.position - lastCamPosition;

        // Gerakkan middle dan side background dengan multiplier parallax
        middleBG.position += new Vector3(deltaMovement.x * parallaxMultiplier.x, deltaMovement.y * parallaxMultiplier.y, 0);
        sideBG.position += new Vector3(deltaMovement.x * parallaxMultiplier.x, deltaMovement.y * parallaxMultiplier.y, 0);

        // Lakukan swap posisi background jika kamera melewati batas length
        if (mainCam.position.x > middleBG.position.x)
            sideBG.position = middleBG.position + Vector3.right * length;

        if (mainCam.position.x < middleBG.position.x)
            sideBG.position = middleBG.position + Vector3.left * length;

        // Tukar middle dan side jika kamera melewati posisi side
        if (mainCam.position.x > sideBG.position.x || mainCam.position.x < sideBG.position.x)
        {
            Transform temp = middleBG;
            middleBG = sideBG;
            sideBG = temp;
        }

        // Simpan posisi kamera untuk frame berikutnya
        lastCamPosition = mainCam.position;
    }
}
