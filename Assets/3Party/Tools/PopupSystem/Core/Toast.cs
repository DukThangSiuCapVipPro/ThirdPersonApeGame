using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Toast : MonoBehaviour
{
    #region Inspector
    public RectTransform mTrans;
    public TMP_Text tvMess;
    public RectTransform background;
    #endregion

    #region Variables
    #endregion

    #region Unity Methods

    #endregion

    #region Public Methods

    public void SetToast(string mess, Action callback = null)
    {
        mTrans.anchoredPosition = new Vector2(0, -100f);
        tvMess.text = mess;
        mTrans.DOAnchorPos(new Vector2(0, -100), 0.1f).OnComplete(delegate
        {
            gameObject.SetActive(true);
            Vector2 size = tvMess.rectTransform.sizeDelta;
            background.sizeDelta = new Vector2(size.x + 20, size.y + 10);
            mTrans.DOAnchorPosY(100, 2f).SetEase(Ease.OutCirc).OnComplete(delegate
            {
                callback?.Invoke();
                gameObject.SetActive(false);
                SimplePool.Despawn(this.gameObject);
            });
            mTrans.DOPunchScale(new Vector3(0.1f, 0.1f, 0f), 1.2f, 0);
        });

    }
    #endregion
}
