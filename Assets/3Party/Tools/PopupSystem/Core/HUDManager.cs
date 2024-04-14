using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDManager : SingletonMonoBehaviour<HUDManager>
{
    public Canvas canvas;
    public Toast toast;

    public GameObject userHUD;
    public TMP_Text tvGold;

    private RectTransform mTrans;

    void Start()
    {
        mTrans = GetComponent<RectTransform>();
        GlobalEventManager.evtChangeUserData += InitUI;
    }
    void Update()
    {
        // if (canvas.worldCamera == null) 
        // {
        //     canvas.worldCamera = Camera.main;
        //     canvas.sortingLayerName = "Overlay";
        // }
    }

    public void ActiveUserHUD(bool active)
    {
        userHUD.SetActive(active);
        InitUI();
    }

    void InitUI()
    {
        tvGold.text = $"{Datamanager.Instance.userData.gold}";
    }

    public void ShowToast(string mess, Action callback = null, bool playSound = true)
    {
        if (playSound)
            SoundManager.Instance.PlayUIButtonClick();
        Toast nToast = SimplePool.Spawn(toast.gameObject, Vector3.zero, Quaternion.Euler(Vector3.one)).GetComponent<Toast>();
        if (nToast != null)
        {
            nToast.mTrans.SetParent(mTrans);
            nToast.mTrans.localScale = Vector3.one;
            nToast.SetToast(mess, callback);
        }
    }
}
