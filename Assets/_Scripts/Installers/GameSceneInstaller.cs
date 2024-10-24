using System;
using UnityEngine;
using Zenject;
using TheSTAR.GUI;

/// <summary>
/// Биндим в контексте сцены Game
/// </summary>
public class GameSceneInstaller : MonoInstaller
{
    [SerializeField] private CameraController worldCameraPrefab;
    [SerializeField] private GameController gameControllerPrefab;
    [SerializeField] private Player playerPrefab;
    [SerializeField] private KeyInput keyInputPrefab;

    [Header("GUI")]
    [SerializeField] private GuiController guiControllerPrefab;
    [SerializeField] private GuiScreen[] screenPrefabs;
    [SerializeField] private GuiUniversalElement[] universalElementPrefabs;

    private GuiController gui;

    public override void InstallBindings()
    {
        InstallGuiContainers();

        var player = Container.InstantiatePrefabForComponent<Player>(playerPrefab);
        Container.Bind<Player>().FromInstance(player).AsSingle();
        Container.Bind<ICameraFocusable>().FromInstance(player).AsSingle();
        Container.Bind<IKeyInputHandler>().FromInstance(player).AsSingle();

        var keyInput = Container.InstantiatePrefabForComponent<KeyInput>(keyInputPrefab);
        Container.Bind<KeyInput>().FromInstance(keyInput).AsSingle();

        var camera = Container.InstantiatePrefabForComponent<CameraController>(worldCameraPrefab);
        Container.Bind<CameraController>().FromInstance(camera).AsSingle();

        var gameController = Container.InstantiatePrefabForComponent<GameController>(gameControllerPrefab);
        Container.Bind<GameController>().FromInstance(gameController).AsSingle();

        // gui
        InstallGuiScreens();
    }

    private void InstallGuiContainers()
    {
        gui = Container.InstantiatePrefabForComponent<GuiController>(guiControllerPrefab, guiControllerPrefab.transform.position, Quaternion.identity, null);
        Container.Bind<GuiController>().FromInstance(gui).AsSingle();
    }

    private void InstallGuiScreens()
    {
        GuiScreen[] createdScreens = new GuiScreen[screenPrefabs.Length];
        GuiScreen screen;
        for (int i = 0; i < screenPrefabs.Length; i++)
        {
            screen = Container.InstantiatePrefabForComponent<GuiScreen>(screenPrefabs[i], gui.ScreensContainer.position, Quaternion.identity, gui.ScreensContainer);
            createdScreens[i] = screen;
        }

        GuiUniversalElement[] createdUniversalElements = new GuiUniversalElement[universalElementPrefabs.Length];
        GuiUniversalElement ue;
        for (int i = 0; i < universalElementPrefabs.Length; i++)
        {
            var uePrefab = universalElementPrefabs[i];
            ue = Container.InstantiatePrefabForComponent<GuiUniversalElement>(
                uePrefab, 
                gui.UniversalElementsContainer(uePrefab.Placement).position, 
                Quaternion.identity, 
                gui.UniversalElementsContainer(uePrefab.Placement));
            createdUniversalElements[i] = ue;
        }

        gui.Set(createdScreens, createdUniversalElements);
    }

    [ContextMenu("Sort")]
    private void Sort()
    {
        Array.Sort(screenPrefabs);
        Array.Sort(universalElementPrefabs);
    }
}