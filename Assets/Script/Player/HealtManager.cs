using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealtManager : MonoBehaviour
{
    public static int health = 3;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite empetyHeart;

    void Awake()
    {
        health = 3;
    }
    // Update is called once per frame
    void Update()
    {
        foreach (Image img in hearts)
        {
            img.sprite = empetyHeart;
        }
        
        for (int i = 0; i < health; i++)
        {
            hearts[i].sprite = fullHeart;
        }
    }
}
