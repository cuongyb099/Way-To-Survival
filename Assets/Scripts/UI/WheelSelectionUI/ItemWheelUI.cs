using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ItemWheelUI : MonoBehaviour
{
    public TextMeshProUGUI Name;
    public Image Image;
    [field:SerializeField] public Button ItemButton { get; private set; }
    public int ID { get;private set; }
    
    public void Initialize(int id, string name, Sprite image)
    {
        ID = id;
        Name.text = name;
        if (image != null)
        {
            Image.sprite = image;
            Image.color = new Color(1, 1, 1, 1);
        }
        else
        {
            Image.color = new Color(0, 0, 0, 0);
        }
    }
}
