using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DataConfig : Singleton<DataConfig>
{
    public List<CharacterConfig> characterConfigs;

    public CharacterConfig GetCharacterConfig(int exp)
    {
        for (int i = characterConfigs.Count - 1; i >= 0 ; i--)
        {
            if (exp >= characterConfigs[i].exp)
            {
                return characterConfigs[i];
            }
        }
        return characterConfigs[0];
    }
}

[Serializable]
public class CharacterConfig
{
    public Vector3 size;
    public int exp;
    public Vector3 camRootPos;
    public Vector3 camRootRotate;
}

[Serializable]
public enum RewardType
{
    Gold = 0,
}

[Serializable]
public class GameReward
{
    public RewardType type;
    public int id;
    public int amount = 50;
}

[Serializable]
public enum BoosterType
{
    Size = 0,
    Time = 1,
    Speed = 2
}