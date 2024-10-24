using UnityEngine;
using Zenject;

namespace TheSTAR.GUI
{
    public class TopCountersContainer : GuiUniversalElement
    {
        [SerializeField] private UnityDictionary<CurrencyType, CurrencyCounter> counters;

        public RectTransform FlyUiTran(CurrencyType currencyType) => counters.Get(currencyType).IconTran;

        public void IncomeMessage(CurrencyType currencyType, int messageValue)
        {
            counters.Get(currencyType).IncomeMessage(messageValue);
        }

        [Inject]
        private void Construct(CurrencyController currency)
        {
            currency.Subscribe((currencyType, value) =>
            {
                if (counters.ContainsKey(currencyType)) counters.Get(currencyType).SetValue(value);
            });
        }

        public void InitBeforeShow(bool useSoftCounter, bool useHardCounter)
        {
            var softCounter = counters.Get(CurrencyType.Soft);
            if (softCounter != null) softCounter.gameObject.SetActive(useSoftCounter);

            var hardCounter = counters.Get(CurrencyType.Hard);
            if (hardCounter != null) hardCounter.gameObject.SetActive(useHardCounter);
        }
    }
}