using System.Collections.Generic;
using UnityEngine;
using TheSTAR.Utility;
using TheSTAR.Sound;
using Zenject;
using DG.Tweening;
using System;

public class BulletsContainer : MonoBehaviour
{
    [SerializeField] private UnityDictionary<BulletType, Bullet> bulletPrefabs;

    private SoundController sounds;

    private List<Bullet> activeBullets = new();
    private Dictionary<BulletType, Queue<Bullet>> inactiveBulletsPool;

    private const float BulletsSpeed = 10;
    private const int MaxLifeTime = 5;

    private Tweener autoDespawnTweener;

    [Inject]
    private void Construct(SoundController sounds)
    {
        this.sounds = sounds;
    }

    private void Start()
    {
        Init();
        WaitForAutoDespawn();
    }

    public void Init()
    {
        var allBulletTypes = EnumUtility.GetValues<BulletType>();
        inactiveBulletsPool = new();
        foreach (var bulletType in allBulletTypes) inactiveBulletsPool.Add(bulletType, new());
    }

    public void Shoot(Shooter shooter, BulletType bulletType, int force, Vector3 direction)
    {
        var inactiveBulletsOfType = inactiveBulletsPool[bulletType];

        Bullet bullet;
        if (inactiveBulletsOfType.Count > 0)
        {
            bullet = inactiveBulletsOfType.Dequeue();
            bullet.transform.position = shooter.ShootingPos.position;
            bullet.gameObject.SetActive(true);
        }
        else bullet = CreateNewBullet(shooter.ShootingPos.position);

        bullet.transform.LookAt(shooter.ShootingPos.position + direction);
        bullet.Init(BulletsSpeed, force, MaxLifeTime);
        activeBullets.Add(bullet);

        if (activeBullets.Count == 1) WaitForAutoDespawn();

        Bullet CreateNewBullet(Vector3 pos)
        {
            var bullet = Instantiate(bulletPrefabs.Get(bulletType), shooter.ShootingPos.position, Quaternion.identity, transform);
            bullet.OnCompleteFlyEvent += OnBulletCompleteFly;
            return bullet;
        }
    }

    private void WaitForAutoDespawn()
    {
        autoDespawnTweener?.Kill();

        autoDespawnTweener =
        DOVirtual.Float(0f, 1f, MaxLifeTime, (value) => {}).OnComplete(() =>
        {
            for (int i = activeBullets.Count - 1; i >= 0; i--)
            {
                Bullet b = activeBullets[i];
                if (DateTime.Now >= b.EndLifeTime) b.ForceDespawn();
            }

            if (activeBullets.Count > 0) WaitForAutoDespawn();
        }).SetEase(Ease.Linear);
    }

    #region Simulation

    public void StopSimulate()
    {
        for (int i = activeBullets.Count - 1; i >= 0; i--)
        {
            Bullet b = activeBullets[i];
            b.ForceDespawn();
        }
    }

    private void Update()
    {
        for (int i = activeBullets.Count - 1; i >= 0; i--)
        {
            Bullet b = activeBullets[i];
            b.Fly();
        }
    }

    #endregion

    private void OnBulletCompleteFly(Bullet b)
    {
        activeBullets.Remove(b);
        inactiveBulletsPool[b.BulletType].Enqueue(b);
    }
}

public enum BulletType
{
    Default
}