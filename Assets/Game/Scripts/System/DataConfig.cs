using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DataConfig : Singleton<DataConfig>
{
    public List<CharacterConfig> characterConfigs;
}

[Serializable]
public class CharacterConfig
{
    public Vector3 size;
    public int exp;
    public Vector3 camRootPos;
    public Vector3 camRootRotate;
}