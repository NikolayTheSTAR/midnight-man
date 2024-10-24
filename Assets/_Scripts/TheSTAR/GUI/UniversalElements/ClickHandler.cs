using System.Collections;
using System.Collections.Generic;
using TheSTAR.GUI;
using UnityEngine;
using Zenject;

public class ClickHandler : GuiUniversalElement
{
    [SerializeField] private Pointer pointer;

    private IClickInputHandler clickInputHandler;

    [Inject]
    private void Construct(IClickInputHandler clickInputHandler)
    {
        this.clickInputHandler = clickInputHandler;
    }

    public override void Init()
    {
        base.Init();
        pointer.InitPointer((eventData) => OnClick());
    }

    private void OnClick()
    {
        clickInputHandler.OnClick();
    }
}

public interface IClickInputHandler
{
    void OnClick();
}