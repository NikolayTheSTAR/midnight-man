using System;
using UnityEngine;
using Zenject;
using TheSTAR.Data;
using TheSTAR.Sound;
using TheSTAR.GUI;
using TheSTAR.Utility;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private DataController data;
    private GuiController gui;
    private SoundController sounds;
    private Player player;
    private AutoSave autoSave;

    private readonly ResourceHelper<GameConfig> gameConfig = new("Configs/GameConfig");

    [Inject]
    private void Construct(
        DataController data,
        SoundController sounds,
        GuiController gui,
        Player player,
        AutoSave autoSave)
    {
        this.data = data;
        this.sounds = sounds;
        this.gui = gui;
        this.player = player;
        this.autoSave = autoSave;
    }

    private void Start()
    {
        sounds.Play(MusicType.MainTheme);

        var loadScreen = gui.FindScreen<LoadScreen>();

        loadScreen.Init(() =>
        {
            if (gameConfig.Get.UseGDPR && !data.gameData.commonData.gdprAccepted)
            {
                var gdprScreen = gui.FindScreen<GDPRScreen>();
                gdprScreen.Init(() =>
                {
                    Debug.Log("On GDPR Accepted");
                    // MaxSdk.SetHasUserConsent(true);
                    data.gameData.commonData.gdprAccepted = true;
                    data.Save(DataSectionType.Common);

                    StartGame();
                });
                gui.Show(gdprScreen);
                return;
            }
            else StartGame();
        });

        gui.Show(loadScreen);
    }

    private void StartGame()
    {
        if (!data.gameData.commonData.gameStarted)
        {
            player.Init(gameConfig.Get.PlayerMaxHP, gameConfig.Get.PlayerMaxHP);
            data.gameData.commonData.gameStarted = true;
        }
        else
        {
            player.Init(data.gameData.playerData.playerCurrentHp, data.gameData.playerData.playerMaxHp);
            player.transform.position = data.gameData.playerData.playerPosition;
        }

        player.HpSystem.OnDieEvent += (_) => Defeat();

        gui.ShowMainScreen();
    }

    private void OnApplicationPause()
    {
        autoSave.AutoSaveGame();
    }

    private void OnApplicationQuit()
    {
        autoSave.AutoSaveGame();
    }

    private void Defeat()
    {
        data.gameData.currencyData.ClearAll();
        data.gameData.levelData.ClearAll();
        data.gameData.commonData.gameStarted = false;
        data.SaveAll();
        gui.Show<DefeatScreen>();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }
}