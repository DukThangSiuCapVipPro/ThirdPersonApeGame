using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

namespace PopupSystem
{
	[RequireComponent(typeof(Animator))]
	public class BasePopup : MonoBehaviour {
		/// <summary>
		/// Optional Animator show popup controller
		/// </summary>
		[HideInInspector]
		public Animator animator;
		/// <summary>
		/// Optional animation show
		/// </summary>
		public AnimationClip showAnimationClip;

		/// <summary>
		/// Optional animation hide
		/// </summary>
		public AnimationClip hideAnimationClip;
		public string popupID;
		
		protected bool canCloseWithOverlay = false;
		protected bool isShowed = false;
		private int mSortOrder;
		private Transform mTransform;
		private bool overlay;
		private Stack<BasePopup> refStacks;
		public Action hideCompletedCallback;
		public Action showCompletedCallback;

        private Action InitAction;
        private float timeOpen;
        
        #region Unity Methods
        
        public virtual void Awake () {
	        isShowed = false;
	        animator = GetComponent<Animator>();
	        mTransform = transform;
	        mSortOrder = mTransform.GetSiblingIndex();
	        refStacks = PopupManager.Instance.popupStacks;
	        if (animator == null || showAnimationClip == null || hideAnimationClip == null) {
				
	        }
#if (UNITY_ANDROID || UNITY_IOS)
	        //gameObject.AddComponent<AutoPopupMobile>();
#endif
        }
        public virtual void OnDisable()
        {
	        if (canCloseWithOverlay)
	        {
		        PopupManager.Instance.EvtTouchOverlay -= HandleCloseByOverlay;
	        }
        }

        public virtual void OnDestroy()
        {
	        if (canCloseWithOverlay)
	        {
		        PopupManager.Instance.EvtTouchOverlay -= HandleCloseByOverlay;
	        }
        }

        #endregion

        #region Public Methods

        public void ActivePopup()
        {
	        InitAction?.Invoke();
	        Show();
        }

        public void Reshow () {
	        if (animator != null && showAnimationClip != null)
	        {
		        animator.enabled = true;
		        animator.Play(showAnimationClip.name, -1, 0.0f);
		        float showAnimationDuration = GetAnimationClipDuration(showAnimationClip);
		        StartCoroutine(RunMethod(showAnimationDuration, OnShowFinish));
	        }
	        PopupManager.Instance.ChangeTransparentOrder(mTransform, true);
        }

        public virtual void Hide () {

	        if (!isShowed)
				return;
			isShowed = false;
			AnimateHide();
			if (canCloseWithOverlay)
			{
				PopupManager.Instance.EvtTouchOverlay -= HandleCloseByOverlay;
			}
        }

        public void ForceHide()
        {
	        isShowed = false;
	        if (canCloseWithOverlay)
	        {
		        PopupManager.Instance.EvtTouchOverlay -= HandleCloseByOverlay;
	        }
	        PopupManager.Instance.OnPopupClose(this);
	        Debug.Log($"ForceHide: {gameObject.name}");
	        gameObject.SetActive(false);
	        Destroy(gameObject);
        }
		
        public void Hide (Action hideCompletedCallback) {       
	        this.hideCompletedCallback = hideCompletedCallback;
	        if (!isShowed)
		        return;
	        isShowed = false;
	        AnimateHide();
	        if (canCloseWithOverlay)
	        {
		        PopupManager.Instance.EvtTouchOverlay -= HandleCloseByOverlay;
	        }
        }

        public bool IsShowed {
	        get { return isShowed; }
        }

        public int SortOrder () {
	        return mSortOrder;
        }

        public void ChangeSortOrder (int newSortOrder = -1) {
	        if (newSortOrder != -1) {
		        mTransform.SetSiblingIndex(newSortOrder);
		        mSortOrder = newSortOrder;
	        }
        }

        public bool isTopPopup()
        {
	        return SortOrder() == PopupManager.Instance.TopPopupIndex();
        }

        public virtual void HandleCloseByOverlay(BasePopup pop)
        {
	        if(pop==this)
		        Hide();
        }
        #endregion

        #region Protected Methods
        protected void Show (Action showCompletedCallback = null, bool overlay = true, Action hideCompletedCallback = null) 
		{
			if (isShowed) {
		        Reshow();
		        int topSortOrder = refStacks.Peek().SortOrder();
		        if (refStacks.Count > 1 && SortOrder() != topSortOrder) {
			        MoveElementToTopStack(ref refStacks, SortOrder()); 
		        }
		        return;
	        } else {
		        this.showCompletedCallback = showCompletedCallback;
		        this.hideCompletedCallback = hideCompletedCallback;
		        //if(Analytics.Exist)
			       // Analytics.Instance.OpenPopup(popupID,PopupManager.Instance.lastPopupID);
	        }
			
	        float waitLastPopupHide = 0;
	        this.overlay = overlay;
	        isShowed = true;

	        if (!overlay && refStacks.Count > 0)
		        ForceHideAllCurrent(ref waitLastPopupHide);

	        if (!refStacks.Contains(this))
		        refStacks.Push(this);

	        if (refStacks.Count > 0)
		        ChangeSortOrder(refStacks.Peek().SortOrder() + 1);

	        if (waitLastPopupHide != 0)
		        StartCoroutine(RunMethod(waitLastPopupHide, AnimateShow));
	        else
		        AnimateShow();

	        if (canCloseWithOverlay)
	        {
		        PopupManager.Instance.EvtTouchOverlay += HandleCloseByOverlay;
	        }

	        timeOpen = Time.realtimeSinceStartup;

			TMP_Text[] lstTxt = FindObjectsOfType<TMP_Text>(true);
			//for(int i = 0; i < lstTxt.Length; i++)
   //         {
			//	AutoChangeTextTM autoChangeText = lstTxt[i].gameObject.GetComponent<AutoChangeTextTM>();
			//	if (autoChangeText == null)
			//	{
			//		AutoChangeFontTM autoChangeFont = lstTxt[i].gameObject.GetComponent<AutoChangeFontTM>();
			//		if (autoChangeFont == null)
   //                 {
			//			lstTxt[i].gameObject.AddComponent<AutoChangeFontTM>();
			//		}
   //             }
   //         }
		}

        protected void Queue(Action action)
        {
	        this.InitAction = action;
	        PopupManager.Instance.OderPopup(this);
        }

        #endregion

        #region Private Methods

        IEnumerator RunMethod(float delay,Action action)
        {
	        yield return new WaitForSecondsRealtime(delay);
	        action();

        }

        void AnimateShow ()
        {
            RectTransform rect = GetComponent<RectTransform>();
            rect.position = new Vector3(rect.position.x, rect.position.y, 0);
            if (animator != null && showAnimationClip != null)
	        {
		        animator.enabled = true;
		        float showAnimationDuration = GetAnimationClipDuration(showAnimationClip);
		        StartCoroutine(RunMethod(showAnimationDuration, OnShowFinish));
		        animator.Play(showAnimationClip.name);
	        } else {
		        OnShowFinish();
	        }
	        PopupManager.Instance.ChangeTransparentOrder(mTransform, true);
	        PopupManager.Instance.OnPopupOpen(this);
        }

        void OnShowFinish () {
	        showCompletedCallback?.Invoke();
        }

        private void AnimateHide ()
        {
	        timeOpen = Time.realtimeSinceStartup - timeOpen;
	        //if(Analytics.Exist)
		       // Analytics.Instance.ClosePopup(popupID,(int)(timeOpen*1000));
	        if (animator != null && hideAnimationClip != null)
	        {
		        animator.enabled = true;
		        animator.Play(hideAnimationClip.name);
		        float hideAnimationDuration = GetAnimationClipDuration(hideAnimationClip);
		        StartCoroutine(RunMethod(hideAnimationDuration,Destroy));
	        } else {                
		        Destroy();
	        }
        }
        void Destroy () {
	        if (refStacks.Contains(this))
		        refStacks.Pop();

	        if (gameObject.activeSelf)
		        DestroyImmediate(gameObject);

			PopupManager.Instance.OnPopupClose(this);
			hideCompletedCallback?.Invoke();
	        PopupManager.Instance.ChangeTransparentOrder(mTransform, false);
	        PopupManager.Instance.ResetOrder();
        }

		
        private void ForceHideAllCurrent (ref float waitTime) {
	        while (refStacks.Count > 0) {
		        BasePopup bp = refStacks.Pop();
		        waitTime += bp.GetAnimationClipDuration(hideAnimationClip);
		        bp.Hide();
	        }
        }

        private float GetAnimationClipDuration (AnimationClip clip) {
	        if (animator != null && clip != null)
	        {
		        animator.enabled = true;
		        RuntimeAnimatorController rac = animator.runtimeAnimatorController;
		        for (int i = 0; i < rac.animationClips.Length; i++) {
			        if (rac.animationClips[i].Equals(clip))
				        return rac.animationClips[i].length;
		        }
	        }

	        return 0;
        }

        private void MoveElementToTopStack (ref Stack<BasePopup> stack, int order) {
	        Stack<BasePopup> tempStack = new Stack<BasePopup>();
	        BasePopup foundPopup = null;
	        int minSortOrder = 0;
	        while (refStacks.Count > 0) {
		        BasePopup bp = refStacks.Pop();
		        if (bp.SortOrder() != order) {
			        tempStack.Push(bp);
			        minSortOrder = bp.SortOrder();
		        } else {
			        foundPopup = bp;
		        }
	        }

	        while (tempStack.Count > 0) {
		        BasePopup bp = tempStack.Pop();
		        bp.ChangeSortOrder(minSortOrder++);
		        stack.Push(bp);
	        }

	        if (foundPopup != null) {
		        foundPopup.ChangeSortOrder(minSortOrder);
		        stack.Push(foundPopup);
	        }
        }
	    #endregion
	}
}