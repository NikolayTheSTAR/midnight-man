using System;
using UnityEngine;

public class HpSystem : MonoBehaviour
{
    private int currentHp;
    private int maxHp;

    public int CurrentHP => currentHp;
    public int MaxHP => maxHp;

    public event Action<int, int> OnChangeHpEvent;
    public event Action OnDamageEvent;
    public event Action<HpSystem> OnDieEvent;

    public void Init(int currentHp, int maxHp)
    {
        this.currentHp = currentHp;
        this.maxHp = maxHp;

        OnChangeHpEvent?.Invoke(currentHp, maxHp);
    }

    public void Damage(int value)
    {
        if (currentHp <= 0) return;

        currentHp -= value;

        bool die;
        if (currentHp <= 0)
        {
            currentHp = 0;
            die = true;
        }
        else die = false;

        OnChangeHpEvent?.Invoke(currentHp, maxHp);
        OnDamageEvent?.Invoke();
        if (die) OnDieEvent?.Invoke(this);
    }

    public void Heal(int value)
    {
        currentHp += value;
        if (currentHp > maxHp) currentHp = maxHp;

        OnChangeHpEvent?.Invoke(currentHp, maxHp);
    }

    [ContextMenu("TestDamage")]
    private void TestDamage()
    {
        Damage(1);
    }
}