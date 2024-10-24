using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Game", fileName = "GameConfig")]
public class GameConfig : ScriptableObject
{
    [SerializeField] private GameAbVersionType versionType;
    [SerializeField] private bool useCheats;
    [SerializeField] private bool lockData;

    [Space]
    [SerializeField] private bool useGDPR = true;

    [Space]
    [SerializeField] private int playerMaxHP = 10;
    [SerializeField] private int enemyMaxHP = 10;

    public GameAbVersionType VersionType => versionType;
    public bool UseCheats => useCheats;
    public bool LockData => lockData;
    public bool UseGDPR => useGDPR;
    public int PlayerMaxHP => playerMaxHP;
    public int EnemyMaxHP => enemyMaxHP;

    [ContextMenu("Test")]
    private void ConfigurateForTest()
    {
        useCheats = true;
    }

    [ContextMenu("Release")]
    private void ConfigurateForRelease()
    {
        useCheats = false;
    }
}