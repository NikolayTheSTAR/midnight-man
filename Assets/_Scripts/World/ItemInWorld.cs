using System;
using UnityEngine;

public class ItemInWorld : MonoBehaviour
{
    [SerializeField] private ItemInWorldType itemType;
    [SerializeField] private int value = 1;
    
    private int index;

    public ItemInWorldType ItemType => itemType;
    public int Value => value;
    public int Index => index;

    public event Action<ItemInWorld> OnGetItemEvent;

    public void Init(int index)
    {
        this.index = index;
    }

    public void Init(ItemInWorldType itemType, int value)
    {
        this.itemType = itemType;
        this.value = value;
    }

    public void OnGet()
    {
        gameObject.SetActive(false);
        OnGetItemEvent?.Invoke(this);
    }
}

public enum ItemInWorldType
{
    Coin,
    HP
}