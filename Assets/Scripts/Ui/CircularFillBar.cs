using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircularFillBar : MonoBehaviour
{

    private Image img;

    void Start()
    {
        GameObject circularFillBar = transform.Find("CircularFillBar").Find("Canvas").Find("Image").gameObject;
        transform.Rotate(new Vector3(0,180,0));
        img = circularFillBar.GetComponent<Image>();
    }

    public void UpdateFillValue(double fillValue)
    {
        int spriteNum = Math.Clamp((int) (360 * (1 - fillValue) / 6), 0, 58);
        string spriteName = "tile0" + (spriteNum + 1);
        if (spriteNum == 0) {
            spriteName = "trsp";
        }
        img.sprite = Resources.Load<Sprite>("Sprites/CircleLoadingSprite/" + spriteName);
    }
}
