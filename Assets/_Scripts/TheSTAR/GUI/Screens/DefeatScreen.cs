using TheSTAR.GUI;
using UnityEngine;
using Zenject;

public class DefeatScreen : GuiScreen
{
    [SerializeField] private PointerButton restartButton;

    private GameController gameController;

    [Inject]
    private void Construct(GameController gameController)
    {
        this.gameController = gameController;
    }

    public override void Init()
    {
        base.Init();
        restartButton.Init(gameController.RestartGame);
    }
}