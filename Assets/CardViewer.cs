using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CardViewer : MonoBehaviour
{
    public Text NameText;
    public Text LevelText;
    public Image Image;
    
    void Awake()
    {
        NameText = transform.Find("Name")?.GetComponent<Text>();
        LevelText = transform.Find("Level")?.GetComponent<Text>();
        Image = transform.Find("Image")?.GetComponent<Image>();
    }

    public void Set(string name, int level, string image)
    {
        NameText.text = name;
        LevelText.text = $"Lv {level}";

        Image.sprite = Resources.Load<Sprite>($"Images/{image}");
    }
}
