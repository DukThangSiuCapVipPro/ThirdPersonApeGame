using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemMap : MonoBehaviour
{
    public TMP_Text tvMap;

    public void Init(int map){
        tvMap.text = $"{map + 1}";
    }
}
