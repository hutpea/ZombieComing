using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour
{
    public ShopSkinUI shopSkinUI;
    public RawImage skinRawImage;
    public Image btnImg;
    public int skinIndex;

    //public TextMeshPro costTxt;

    public TextMeshProUGUI buttonTxt;

    public Sprite greenBtnSprite;
    public Sprite grayBtnSprite;
    private void Awake()
    {
        if (skinIndex == GameData.SelectZombieSkin)
        {
            //OnClick();
        }
    }

    /*public void OnClick()
    {
        buttonTxt.SetText("Equipped");
        btnImg.sprite = grayBtnSprite;
        shopSkinUI.previewImage.texture = skinRawImage.texture;
        foreach (var sButton in shopSkinUI.shopButtons)
        {
            if(sButton.skinIndex == this.skinIndex) continue;
            sButton.buttonTxt.SetText("Use");
            Debug.Log("Set green button " + sButton.transform.parent.gameObject.name);
            btnImg.sprite = greenBtnSprite;
        }
        shopSkinUI.OnChangeSkin(skinIndex);
    }*/
}
