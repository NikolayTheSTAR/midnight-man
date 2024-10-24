using System;
using UnityEngine;
using TheSTAR.Data;
using TheSTAR.Utility;
//using Sirenix.OdinInspector;
using Random = UnityEngine.Random;
using Zenject;

public class CurrencyController
{
    private DataController _data;
    private CurrencyType[] allCurrencyTypes;

    [Inject]
    private void Construct(DataController data)
    {
        _data = data;
        allCurrencyTypes = EnumUtility.GetValues<CurrencyType>();
    }

    public int Coins => GetCurrencyValue(CurrencyType.Soft);

    private event Action<CurrencyType, int> onTransactionEvent;

    public void Subscribe(Action<CurrencyType, int> onTransactionAction)
    {
        onTransactionEvent += onTransactionAction;

        foreach (var currencyType in allCurrencyTypes)
        {
            onTransactionAction(currencyType, GetCurrencyValue(currencyType));
        }
    }

    public void Unsubscribe(Action<CurrencyType, int> onTransactionAction)
    {
        onTransactionEvent -= onTransactionAction;
    }

    public void AddCurrency(CurrencyType currencyType, int value = 1, bool autoSave = true)
    {
        if (value < 0)
        {
            ReduceCurrency(currencyType, -value, autoSave);
            return;
        }

        _data.gameData.currencyData.AddCurrency(currencyType, value, out var result);
        if (autoSave) _data.Save(DataSectionType.Currency);

        OnTransactionReaction(currencyType, result);
    }

    public void ReduceCurrency(CurrencyType currencyType, int count = 1, bool autoSave = false, Action completeAction = null, Action failAction = null)
    {
        if (_data.gameData.currencyData.GetCurrencyCount(currencyType) >= count)
        {
            _data.gameData.currencyData.AddCurrency(currencyType, -count, out var result);
            if (autoSave) _data.Save(DataSectionType.Currency);

            completeAction?.Invoke();

            OnTransactionReaction(currencyType, result);
        }
        else failAction?.Invoke();
    }

    public void ClearCurrency(CurrencyType currencyType)
    {
        var count = GetCurrencyValue(currencyType);
        _data.gameData.currencyData.AddCurrency(currencyType, -count, out var result);

        OnTransactionReaction(currencyType, result);
    }

    public int GetCurrencyValue(CurrencyType currencyType)
    {
        return _data.gameData.currencyData.GetCurrencyCount(currencyType);
    }

    public void GiveReward(CurrencyValue reward)
    {
        int value;

        if (reward.useRange) value = Random.Range((int)reward.valueRange.min, (int)reward.valueRange.max + 1);
        else value = reward.value;

        AddCurrency(reward.currencyType, value);
    }

    public void OnTransactionReaction(CurrencyType currencyType, int finalValue)
    {
        onTransactionEvent?.Invoke(currencyType, finalValue);
    }
}

[Serializable]
public struct CurrencyValue
{
    public CurrencyType currencyType;
    //[HideIf("useRange")] 
    public int value;
    public bool useRange;
    //[ShowIf("useRange")] 
    public IntRange valueRange;
}

public enum CurrencyType
{
    Soft,
    Hard
}