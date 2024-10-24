using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheSTAR.Data;
using Zenject;

public class ItemsInWorldContainer : MonoBehaviour
{
    [SerializeField] private ItemInWorld[] items;
    [SerializeField] private UnityDictionary<ItemInWorldType, ItemInWorld> itemPrefabs;

    private List<ItemInWorld> activeDrop = new();

    private DataController data;

    [Inject]
    private void Construct(DataController data, AutoSave autoSave)
    {
        this.data = data;
        autoSave.BeforeAutoSaveGameEvent += () =>
        {
            var dropData = data.gameData.levelData.dropData;
            dropData.Clear();
            for (int i = 0; i < activeDrop.Count; i++)
            {
                dropData.Add(new(activeDrop[i].ItemType, activeDrop[i].Value, activeDrop[i].transform.position));
            }
        };
    }

    private void Start()
    {
        LoadItems();
    }

    public void LoadItems()
    {
        var collectedCurrencyItems = data.gameData.levelData.collectedItems;

        for (int i = 0; i < items.Length; i++)
        {
            items[i].Init(i);
            items[i].gameObject.SetActive(!collectedCurrencyItems.ContainsKey(i) || !collectedCurrencyItems[i]);
        }

        var allDropData = data.gameData.levelData.dropData;

        for (int i = 0; i < allDropData.Count; i++)
        {
            var dropData = allDropData[i];
            Drop(dropData.itemInWorldType, dropData.value, dropData.position);
        }
    }

    public void Drop(ItemInWorldType itemType, int value, Vector3 position)
    {
        var item = Instantiate(itemPrefabs.Get(itemType), position, Quaternion.identity, transform);
        item.Init(itemType, value);
        item.OnGetItemEvent += OnGetDropItem;
        activeDrop.Add(item);
    }

    private void OnGetDropItem(ItemInWorld item)
    {
        activeDrop.Remove(item);
        Destroy(item.gameObject);
    }
}