using UnityEngine;
using Zenject;

/// <summary>
/// Здесь биндим в контексте всего проекта
/// </summary>
public class ProjectInstaller : MonoInstaller
{
    [SerializeField] private GameLoader gameControllerPrefab;

    public override void InstallBindings()
    {
        var gameLoader = Container.InstantiatePrefabForComponent<GameLoader>(gameControllerPrefab, gameControllerPrefab.transform.position, Quaternion.identity, null);
        Container.Bind<GameLoader>().FromInstance(gameLoader).AsSingle();        
    }
}