
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace PopupSystem
{
    public class PopupManager : MonoBehaviour
    {
        #region Popup Events

        public delegate void PopupEvent(BasePopup popup);
        public event PopupEvent EvtPopupOpen;
        public event PopupEvent EvtPopupClose;
        public Action EventAllPopupClose;
        public Action EventOpenPopup;
        public event PopupEvent EvtTouchOverlay;

        #endregion
        public Canvas canvas;
        public bool usingDefaultTransparent = true;
        public BasePopup[] prefabs;
        public Image transparent;
        private Transform mTransparentTrans;
        public Stack<BasePopup> popupStacks = new Stack<BasePopup>();
        public Transform parent;
        private int defaultSortingOrder;
        private static PopupManager mInstance;
        private Queue<BasePopup> popupQueue = new Queue<BasePopup>();
        public bool hasPopupShowing;
        public static PopupManager Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = LoadResource<PopupManager>("PopupManager");
                }

                return mInstance;
            }
        }

        public static bool Exits => mInstance;
        public string lastPopupID { get; private set; } = "Scene_Main";
        void Awake()
        {
            mInstance = this;
            mTransparentTrans = transparent.transform;
            defaultSortingOrder = canvas.sortingOrder;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            EvtPopupClose += HandlePopupClose;
            hasPopupShowing = false;
        }

        private void Update()
        {
            // if(!canvas.worldCamera)
            // {
            //     canvas.worldCamera = Camera.main;
            //     canvas.sortingLayerName = "Overlay";
            // }
        }

        private void OnDestroy()
        {
            EvtPopupClose -= HandlePopupClose;
        }

        public static T CreateNewInstance<T>()
        {
            T result = Instance.CheckInstancePopupPrebab<T>();
            return result;
        }

        public T CheckInstancePopupPrebab<T>()
        {
            GameObject go = null;
            for (int i = 0; i < prefabs.Length; i++)
            {
                if (IsOfType<T>(prefabs[i]))
                {
                    go = (GameObject) Instantiate(prefabs[i].gameObject, parent);
                    break;
                }
            }

            T result = go.GetComponent<T>();
            return result;
        }

        private bool IsOfType<T>(object value)
        {
            return value is T;
        }

        public void ChangeTransparentOrder(Transform topPopupTransform, bool active)
        {
            if (active)
            {
                mTransparentTrans.SetSiblingIndex(topPopupTransform.GetSiblingIndex() - 1);
                if (usingDefaultTransparent)
                {
                    ShowFade();
                }
                else
                {
                    Debug.Log("Clear all");
                    HideFade();
                }

                /*transparent.gameObject.SetActive(true && usingDefaultTransparent);*/
                hasPopupShowing = true;
            }
            else
            {
                if (parent.childCount >= 2)
                {
                    mTransparentTrans.SetSiblingIndex(parent.childCount - 2);
                    hasPopupShowing = true;
                }
                else
                {
                    HideFade();
                    hasPopupShowing = false;
                }
            }
            //Debug.Log("hasPopupShowing: "+ hasPopupShowing);
        }

        public PopupManager Preload()
        {
            return mInstance;
        }

        public bool SequenceHidePopup()
        {
            if (popupStacks.Count > 0)
                popupStacks.Peek().Hide();
            else
            {
                HideFade();
                /*transparent.gameObject.SetActive(false);*/
                hasPopupShowing = false;
            }

            return (popupStacks.Count > 0);
        }

        public void CloseAllPopup()
        {
            //Debug.Log($"Close : {popupStacks.Count} popups");
            while (popupStacks.Count>0)
            {
                BasePopup popup = popupStacks.Pop();
                if (popup != null)
                    popup.ForceHide();
            }
            ResetOrder();
            HideFade();
        }

        public static T LoadResource<T>(string name)
        {
            GameObject go = (GameObject) GameObject.Instantiate(Resources.Load(name));
            go.name = $"[{name}]";
            DontDestroyOnLoad(go);
            return go.GetComponent<T>();
        }

        public void SetSortingOrder(int order)
        {
            canvas.sortingOrder = order;
        }

        public void ResetOrder()
        {
            canvas.sortingOrder = defaultSortingOrder;
        }

        public void OderPopup(BasePopup popup)
        {
            if (!hasPopupShowing)
            {
                popup.ActivePopup();
            }
            else
            {
                popup.gameObject.SetActive(false);
                popupQueue.Enqueue(popup);
            }
        }

        public void OnClickOverlay()
        {
            EvtTouchOverlay?.Invoke(popupStacks.Peek());
        }

        public bool GetHasPopUp()
        {
            return hasPopupShowing;
        }

        public int TopPopupIndex()
        {
            return popupStacks.Count + 1;
        }
        
        #region Event Methods

        public void OnPopupOpen(BasePopup popup)
        {
            if (popupStacks.Count == 1)
            {
                //GlobalSetting.Instance.isShowPopup = true;
                //UserHUD.Instance.Block(true);
            }
            lastPopupID = popup.popupID;
            EvtPopupOpen?.Invoke(popup);
            EventOpenPopup?.Invoke();
        }

        public void OnPopupClose(BasePopup popup)
        {
            if (popupStacks.Count == 0)
            {
                //GlobalSetting.Instance.isShowPopup = false;
                //UserHUD.Instance.Block(false);
                hasPopupShowing = false;
                EventAllPopupClose?.Invoke();
                //if(GlobalSetting.Instance.SCENE == SCENE_NAME.MAIN)
                    //lastPopupID = "main_menu";
            }
            EvtPopupClose?.Invoke(popup);
        }

        #endregion

        #region Handle Events

        private void HandlePopupClose(BasePopup popup)
        {
            if (popupStacks.Count == 0 && popupQueue.Count > 0)
            {
                BasePopup nextPopup = popupQueue.Dequeue();
                nextPopup.gameObject.SetActive(true);
                nextPopup.ActivePopup();
            }
        }

        #endregion

        #region Tween

        public void ShowFade()
        {
            transparent.gameObject.SetActive(true);
            //transparent.DOFade(transparentAmount, fadeTweenTime).SetEase(fadeInTweenType);
        }

        public void HideFade()
        {
            //transparent.DOFade(0, fadeTweenTime).SetEase(fadeOutTweenType).OnComplete(() =>
            //{
            //    transparent.gameObject.SetActive(false);
            //});
            transparent.gameObject.SetActive(false);
        }

        public void DisableFadeBackground()
        {
            transparent.gameObject.SetActive(false);
        }

        public void EnableFadeBackground()
        {
            transparent.gameObject.SetActive(true);
        }

        #endregion
    }
}