using System;
using UnityEngine;

public class Player : Creature, ICameraFocusable, IKeyInputHandler
{    
    [SerializeField] private GameObject fire;

    #region KeyInput

    public void OnStartKeyInput() 
    {}

    public void KeyInput(Vector2 direction)
    {
        MoveTo(direction);
    }

    public void OnEndKeyInput() 
    {}

    #endregion
}