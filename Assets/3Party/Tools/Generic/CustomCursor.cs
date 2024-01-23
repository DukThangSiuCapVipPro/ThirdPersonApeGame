using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    #region Inspector

    [SerializeField]private SpriteRenderer mCursor;
    [SerializeField]private Sprite normalSpr;
    [SerializeField]private Sprite clickSpr;
    [SerializeField]private Sprite holdSpr;
    #endregion

    #region Variables
    private bool isActive = false;
    private Transform mTrans;
    [SerializeField]private MouseState State = MouseState.Free;
    private float touchTime = 0;
    #endregion

    #region Unity Methods

    private void Awake()
    {
        mTrans = GetComponent<Transform>();
        State = MouseState.Free;
        mCursor.sprite = normalSpr;
        Cursor.visible = false;
        isActive = true;
    }

    private void Update()
    {
        if(!isActive)
            return;
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mTrans.position = cursorPos;
        if (State == MouseState.Free)
        {
            if (Input.GetMouseButtonDown(0))
            {
                State = MouseState.Click;
                touchTime = Time.realtimeSinceStartup;
                mCursor.sprite = clickSpr;
            }
        }
        else
        {
            if (Input.GetMouseButtonUp(0))
            {
                State = MouseState.Free;
                mCursor.sprite = normalSpr;
                touchTime = 0;
            }
            else
            {
                if (State == MouseState.Click && Time.realtimeSinceStartup - touchTime>0.2f)
                {
                    State = MouseState.Hold;
                    mCursor.sprite = holdSpr;
                    touchTime = 0;
                }
            }
        }
        
    }

    #endregion

    #region Public Methods

    #endregion

    #region Private Methods

    #endregion
    
    private enum MouseState
    {
        Free,
        Click,
        Hold
    }
}
