using System;
using UnityEngine;
using World;
using DG.Tweening;

public class Player : Creature, ICameraFocusable, IKeyInputHandler
{    
    [SerializeField] private GameObject fire;
    [SerializeField] private EntranceTrigger trigger;

    private Tweener slowFireEndTweener;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        trigger.Init(OnEnter, OnExit);
    }

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

    private void OnEnter(Collider col)
    {
        if (col.CompareTag("Window"))
        {
            slowFireEndTweener =
            DOVirtual.Float(0f, 1f, 5f, (temp) => {}).OnComplete(() =>
            {
                fire.SetActive(false);
            });
        }
        else if (col.CompareTag("Drops"))
        {
            fire.SetActive(false);
        }
    }

    private void OnExit(Collider col)
    {
        if (col.CompareTag("Window"))
        {
            slowFireEndTweener?.Kill();
        }
    }
}