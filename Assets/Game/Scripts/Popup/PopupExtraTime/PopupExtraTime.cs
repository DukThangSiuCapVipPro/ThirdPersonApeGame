using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PopupSystem;
using System;

public class PopupExtraTime : SingletonPopup<PopupExtraTime>
{
    Action evtOK;
    Action evtNO;

    public void Show(Action ok, Action no){
        evtOK = ok;
        evtNO = no;
        base.Show();
    }

    public void OnOK()
    {
        base.Hide(evtOK);
    }
    
    public void OnNO()
    {
        base.Hide(evtNO);
    }
}
