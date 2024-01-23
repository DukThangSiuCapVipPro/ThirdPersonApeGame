using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestItem : MonoBehaviour
{
   [SerializeField]private Image imgBg;
   [SerializeField]private Text tvTitle;
   [SerializeField]private Button btnItem;

   private Action<TestItem> action;
   public void SetData(int id,string title, Color color,Action<TestItem> _action)
   {
      imgBg.color = color;
      tvTitle.text = $"#{id}\n{title}";
      action = _action;
   }

   public string GetData()
   {
      return tvTitle.text;
   }

   public void OnClickButton()
   {
      action?.Invoke(this);
   }
}
