using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickObject : MonoBehaviour
{
    #region Event
    public Action<Vector3> EventMouseDown; 
    #endregion
    
    #region Unity Methods
    private void OnMouseDown()
    {
        Debug.Log("Clicked pointer");
        EventMouseDown?.Invoke(Input.mousePosition);
    }

    private void OnDestroy()
    {
        EventMouseDown = null;
    }

    #endregion
}

