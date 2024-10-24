using System;
using UnityEngine;
using Zenject;
using TheSTAR.GUI;
using TheSTAR.Utility;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private GuiController gui;
    private Player player;

    private readonly ResourceHelper<GameConfig> gameConfig = new("Configs/GameConfig");

    [Inject]
    private void Construct(
        GuiController gui,
        Player player)
    {
        this.gui = gui;
        this.player = player;
    }

    private void Start()
    {
        var loadScreen = gui.FindScreen<LoadScreen>();

        loadScreen.Init(() =>
        {
            StartGame();
        });

        gui.Show(loadScreen);
    }

    private void StartGame()
    {
        gui.ShowMainScreen();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }
}