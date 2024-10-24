using System;
using TheSTAR.Data;
using UnityEngine;
using Zenject;

public class Player : Creature, ICameraFocusable, IKeyInputHandler, IClickInputHandler
{
    [SerializeField] private ItemInWorldGetter itemGetter;
    [SerializeField] private Shooter shooter;

    private DataController data;
    private CurrencyController currency;
    private BulletsContainer bullets;
    
    [Inject]
    private void Construct(DataController data, CurrencyController currency, BulletsContainer bullets, AutoSave autoSave)
    {
        this.data = data;
        this.currency = currency;
        this.bullets = bullets;
        autoSave.BeforeAutoSaveGameEvent += () =>
        {
            data.gameData.playerData.playerPosition = transform.position;
            data.gameData.playerData.playerCurrentHp = HpSystem.CurrentHP;
            data.gameData.playerData.playerMaxHp = HpSystem.MaxHP;
        };
    }

    public void Init(int currentHp, int maxHp)
    {
        hpSystem.Init(currentHp, maxHp);

        itemGetter.Init();
        itemGetter.OnGetItemEvent += (index, itemType, value) =>
        {
            if (itemType == ItemInWorldType.Coin)
            {
                currency.AddCurrency(CurrencyType.Soft, value);
            }
            else if (itemType == ItemInWorldType.HP)
            {
                hpSystem.Heal(value);
            }
            
            var colledtedItems = data.gameData.levelData.collectedItems;
            if (!colledtedItems.ContainsKey(index)) colledtedItems.Add(index, true);
            else colledtedItems[index] = true;
        };

        shooter.Init(bullets, BulletType.Default, 1);
    }

    #region KeyInput

    public void OnStartKeyInput() 
    {}

    public void KeyInput(Vector2 direction)
    {
        MoveTo(direction);
    }

    public void OnEndKeyInput() 
    {}

    #endregion

    #region ClickInput

    public void OnClick()
    {
        Shoot();
    }

    #endregion

    private void Shoot()
    {
        shooter.Shoot(visualTran.forward);
    }
}