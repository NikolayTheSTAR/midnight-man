using System;
using UnityEngine;
using World;
using DG.Tweening;

public class Player : Creature, ICameraFocusable, IKeyInputHandler
{    
    [SerializeField] private GameObject fire;
    [SerializeField] private EntranceTrigger trigger;
    [SerializeField] private Transform protectHandTran;

    [Space]
    [SerializeField] private Vector3 idleHandPos;
    [SerializeField] private Vector3 idleHandRotation;
    [SerializeField] private Vector3 protectHandPos;
    [SerializeField] private Vector3 protectHandRotation;

    private Tweener slowFireEndTweener;

    private bool enterDrops;
    private bool enterWindow;
    private bool protection;

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

    public void OnStartActionInput()
    {
        protection = true;
        UpdateFireState();
    }

    public void OnEndActionInput()
    {
        protection = false;
        UpdateFireState();
    }

    #endregion

    private void OnEnter(Collider col)
    {
        if (col.CompareTag("Window"))
        {
            enterWindow = true;
            UpdateFireState();
        }
        else if (col.CompareTag("Drops"))
        {
            enterDrops = true;
            UpdateFireState();
        }
    }

    private void OnExit(Collider col)
    {
        if (col.CompareTag("Window"))
        {
            enterWindow = false;
            UpdateFireState();
        }
        else if (col.CompareTag("Drops"))
        {
            enterDrops = false;
            UpdateFireState();
        }
    }

    private void UpdateFireState()
    {
        if (protection)
        {
            protectHandTran.localPosition = protectHandPos;
            protectHandTran.localEulerAngles = protectHandRotation;
            slowFireEndTweener?.Kill();
        }
        else
        {
            protectHandTran.localPosition = idleHandPos;
            protectHandTran.localEulerAngles = idleHandRotation;

            if (enterDrops)
            {
                fire.SetActive(false);
            } 
            else if (enterWindow)
            {
                slowFireEndTweener =
                DOVirtual.Float(0f, 1f, 5f, (temp) => {}).OnComplete(() =>
                {
                    fire.SetActive(false);
                });
            }
        }
    }
}