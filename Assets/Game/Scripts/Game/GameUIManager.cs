using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using TMPro;
using TruongNT;
using System;

public class GameUIManager : SingletonMonoBehaviour<GameUIManager>
{
    public GameObject mainPanel;
    public ItemMap itmCurrentMap, itmNextMap;
    public List<ItemMapElement> itmMapElements;
    public TMP_Text tvTime;

    UserData userData;

    void Start()
    {
        InitUI();
    }

    public void UpdateTime(float time)
    {
        tvTime.text = new TimeSpan(0, 0, (int)time).ToStringValue();
    }

    public void OnClickPlayGame()
    {
        mainPanel.SetActive(false);
        GameManager.Instance.PlayGame();
    }

    void InitUI()
    {
        userData = Datamanager.Instance.userData;
        itmCurrentMap.Init(userData.map);
        itmNextMap.Init(userData.map + 1);
        for (int i = 0; i < itmMapElements.Count; i++)
        {
            itmMapElements[i].Init(i, userData.stage);
        }
    }
}
