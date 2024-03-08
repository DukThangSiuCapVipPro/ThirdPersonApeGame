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