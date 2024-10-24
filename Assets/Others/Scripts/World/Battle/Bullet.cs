using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private BulletType bulletType;
    [SerializeField] private Transform visual;

    public event Action<Bullet> OnCompleteFlyEvent;
    
    private float speed;

    private int force;
    private DateTime endLifeTime;
    public DateTime EndLifeTime => endLifeTime;
    public BulletType BulletType => bulletType;

    public void Init(float speed, int force, int maxLifetimeSeconds)
    {
        this.speed = speed;
        this.force = force;
        endLifeTime = DateTime.Now.AddSeconds(maxLifetimeSeconds);
    }

    public void Fly()
    {
        transform.Translate(new Vector3(0, 0, speed * Time.deltaTime));
    }

    public void ForceDespawn()
    {
        CompleteFly();
    }

    private void CompleteFly()
    {
        gameObject.SetActive(false);
        OnCompleteFlyEvent?.Invoke(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Creature")) other.GetComponent<HpSystem>().Damage(force);
        CompleteFly();
    }
}