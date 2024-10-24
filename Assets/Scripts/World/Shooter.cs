using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Shooter : MonoBehaviour
{
    [SerializeField] private Transform shootingPos;
    public Transform ShootingPos => shootingPos;

    private BulletType bulletType;
    private int force;

    private BulletsContainer bullets;

    public void Init(BulletsContainer bullets, BulletType bulletType, int force)
    {
        this.bullets = bullets;
        this.bulletType = bulletType;
        this.force = force;
    }

    public void Shoot(Vector3 direction)
    {
        bullets.Shoot(this, bulletType, force, direction);
    }
}