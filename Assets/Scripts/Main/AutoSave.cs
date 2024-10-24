using System;
using System.Collections.Generic;
using TheSTAR.Data;
using UnityEngine;
using Zenject;

public class AutoSave
{
    private DataController data;
    public event Action BeforeAutoSaveGameEvent;

    [Inject]
    private void Construct(DataController data)
    {
        this.data = data;
    }

    public void AutoSaveGame()
    {
        Debug.Log("AutoSave");
        BeforeAutoSaveGameEvent?.Invoke();
        data.SaveAll();
    }
}