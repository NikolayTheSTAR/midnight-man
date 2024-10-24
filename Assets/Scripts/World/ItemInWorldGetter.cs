using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using World;

public class ItemInWorldGetter : MonoBehaviour
{
    [SerializeField] private EntranceTrigger trigger;

    public event Action<int, ItemInWorldType, int> OnGetItemEvent;

    public void Init()
    {
        trigger.Init(OnEnter, null);
    }

    private void OnEnter(Collider col)
    {
        if (col.CompareTag("Item"))
        {
            GetItemFromWorld(col.gameObject.GetComponent<ItemInWorld>());
        }
    }

    private void GetItemFromWorld(ItemInWorld item)
    {
        item.OnGet();
        OnGetItemEvent?.Invoke(item.Index, item.ItemType, item.Value);
    }
}
