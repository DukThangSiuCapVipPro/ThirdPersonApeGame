using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using TruongNT;
using UnityEngine;
using UnityEngine.UI;

public class TutorialHelper : MonoBehaviour
{
    #region Instance

    private static TutorialHelper instance;

    public static TutorialHelper Instance
    {
        get
        {
            if (instance)
                return instance;
            instance = FindObjectOfType<TutorialHelper>();
            return instance;
        }
    }

    public static bool Exist => instance;

    #endregion

    #region Inspector
    public Sprite defautPoiter;
    public Image imgBackground;
    public GameObject npcDialog;
    public TMP_Text ncpMessage;
    public Image uiPointer;
    public Image imgMaskPointer;
    public RectTransform maskRect;
    public GameObject handObj;
    #endregion

    #region Variables
    public bool IsActive;
    private RectTransform mPointer;
    private RectTransform mDialog;
    private Button btnPointer;
    private Button btnMask;
    private Action OnPointerClick;
    private Action OnMaskClick;
    #endregion

    #region Unity Methods

    private void Awake()
    {
        instance = this;
        mPointer = uiPointer.GetComponent<RectTransform>();
        mDialog = npcDialog.GetComponent<RectTransform>();
        btnPointer = uiPointer.GetComponent<Button>();
        btnMask = imgMaskPointer.GetComponent<Button>();
        btnPointer.onClick.AddListener(HandlePointerClick);
        btnMask.onClick.AddListener(HandleMaskClick);
    }

    private void OnDestroy()
    {
        
    }

    #endregion

    #region Public Methods
    Coroutine cDialog = null;
    Tween tDialog = null;
    public void ShowNPCDiaglog(string message, float duration, Action compleateAction)
    {
        ShowText();

        void ShowText()
        {
            npcDialog.SetActive(true);
            if (cDialog != null)
                StopCoroutine(cDialog);
            if (tDialog != null)
            {
                tDialog.Kill();
                mDialog.DOAnchorPosX(30, 0f).SetEase(Ease.Linear);
            }
            else
            {
                tDialog = mDialog.DOAnchorPosX(30, 0.5f).SetEase(Ease.Linear);
            }
            cDialog = StartCoroutine(Tools.TextRunEffect(message, duration,
                delegate (string text)
                {
                    ncpMessage.text = text;
                },
                delegate
                {
                    cDialog = null;
                    tDialog = null;
                    compleateAction?.Invoke();
                }));
        }
    }

    public void HideNPCDialog()
    {
        tDialog = mDialog.DOAnchorPosX(-500, 0.5f).SetEase(Ease.Linear).OnComplete(delegate
        {
            tDialog = null;
        });
        npcDialog.gameObject.SetActive(false);
        ncpMessage.text = "";
    }

    public void ActiveUIPointer(Vector2 size, Vector2 pos, Action completeAction, Action pointerClick = null, Action maskClick = null, Image imgTarget = null)
    {
        Debug.Log("Active UI Pointer");
        if (pointerClick != null)
        {
            uiPointer.raycastTarget = true;
            OnPointerClick = pointerClick;
        }
        else
        {
            uiPointer.raycastTarget = false;
            OnPointerClick = null;
        }

        if (maskClick != null)
        {
            imgMaskPointer.raycastTarget = true;
            OnMaskClick = maskClick;
        }
        else
        {
            imgMaskPointer.raycastTarget = false;
            OnMaskClick = null;
        }

        if (imgTarget != null)
        {
            uiPointer.sprite = imgTarget.sprite;
            uiPointer.pixelsPerUnitMultiplier = imgTarget.pixelsPerUnitMultiplier;
        }
        else
        {
            uiPointer.sprite = defautPoiter;
            uiPointer.pixelsPerUnitMultiplier = 1;
        }
        mPointer.sizeDelta = Vector2.zero;
        mPointer.position = pos;
        mPointer.gameObject.SetActive(true);
        btnPointer.interactable = false;
        handObj.SetActive(size != Vector2.zero);
        //Debug.Log($"mPointer: {mPointer.position}");
        mPointer.DOSizeDelta(size, 0.3f).OnComplete(delegate
        {
            completeAction?.Invoke();
            btnPointer.interactable = true;
        });
    }

    public void DeactiveUIPointer()
    {
        uiPointer.gameObject.SetActive(false);
    }

    public void Active()
    {
        IsActive = true;
        imgBackground.gameObject.SetActive(true);
    }
    public void Deactive()
    {
        IsActive = false;
        imgBackground.gameObject.SetActive(false);
    }
    #endregion

    #region Private Methods

    private void HandlePointerClick()
    {
        OnPointerClick?.Invoke();
    }
    private void HandleMaskClick()
    {
        OnMaskClick?.Invoke();
    }
    #endregion
}