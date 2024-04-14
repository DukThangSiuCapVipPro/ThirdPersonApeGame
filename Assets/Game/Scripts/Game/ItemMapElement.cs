using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemMapElement : MonoBehaviour
{
    public TMP_Text tvStage;
    public Image imgStage;

    public void Init(int stage, int current)
    {
        tvStage.text = $"{stage + 1}";
        if (current < stage)
            imgStage.color = Color.gray;
        else if (current > stage)
            imgStage.color = Color.white;
        else
            imgStage.color = Color.green;
    }
}
