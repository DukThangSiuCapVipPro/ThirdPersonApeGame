using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Your.Namespace.Here.UniqueStringHereToAvoidNamespaceConflicts.Grids;

public class TestScript : MonoBehaviour
{
    [SerializeField]private Button btnInit;
    [SerializeField]private Button btnChangeState;
    [SerializeField]private Text tvStatus;
    [SerializeField] private TestGridAdapter adapter;

    public void Awake()
    {
        btnInit.onClick.AddListener(ClickInit);
        btnChangeState.onClick.AddListener(ClickChangeState);
    }

    public void ClickInit()
    {
        adapter.Init(this);
    }

    public void ClickChangeState()
    {
        adapter.ReloadData();
    }

    public void OnClickItem(TestItem item)
    {
        tvStatus.text = $"{item.GetData()}";
    }
}
