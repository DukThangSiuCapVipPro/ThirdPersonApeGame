using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;

public class Datamanager : Singleton<Datamanager>
{
    public UserData userData;

    public bool IsLoadDone { get; private set; } = false;

    void Start()
    {
        LoadUserData();
    }

    public void UpdateGold(int amt)
    {
        userData.gold += amt;
        SaveUserData();
    }
    public void UpdateMapStage(int m, int stg)
    {
        
    }

    void LoadUserData()
    {
        if (!PlayerPrefs.HasKey("USER_DATA"))
        {
            userData = new UserData();
        }
        else
        {
            string data = PlayerPrefs.GetString("USER_DATA");
            userData = JsonConvert.DeserializeObject<UserData>(data);
        }
        IsLoadDone = true;
    }
    void SaveUserData()
    {
        GlobalEventManager.ChangeUserData();
        string data = JsonConvert.SerializeObject(userData);
        PlayerPrefs.SetString("USER_DATA", data);
    }
}

[Serializable]
public class UserData
{
    public int map;
    public int stage = 2;
    public int gold;
    public string name;
}
