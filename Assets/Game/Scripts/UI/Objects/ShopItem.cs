using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    public BoosterType boosterType;
    public GameObject priceObj;
    public GameObject adsObj;
    public TMP_Text tvPrice;

    int price = 100;

    void Start()
    {
        adsObj.SetActive(boosterType == BoosterType.Size);
        priceObj.SetActive(boosterType != BoosterType.Size);
        tvPrice.text = $"100";
    }

    public void OnClickBuy()
    {
        Datamanager.Instance.UpdateGold(-price);
        switch (boosterType)
        {
            case BoosterType.Size:
                GameManager.Instance.UseBoosterSize();
            break;
            case BoosterType.Time:
                GameManager.Instance.UseBoosterTime();
            break;
            case BoosterType.Speed:
                GameManager.Instance.UseBoosterSpeed();
            break;
        }
        gameObject.SetActive(false);
    }
}
