using System;
using System.Collections.Generic;
using TheSTAR.Utility;
using UnityEngine;
using DG.Tweening;
using Zenject;

namespace TheSTAR.GUI
{
    public class FlyUIContainer : MonoBehaviour
    {
        [SerializeField] private HandfulFlyUI handfulPrefab;
        [SerializeField] private AnimationCurve speedCurve;
        [SerializeField] private AnimationCurve scaleCurve;
        [SerializeField] private float flyTime = 1;

        private List<FlyUIObject> _flyObjectsPool = new ();

        private GuiController gui;
        private CurrencyController currency;
        private readonly ResourceHelper<IconsConfig> iconsConfig = new ("Configs/IconsConfig");

        private const int DefaultFlyCount = 10;

        [Inject]
        private void Construct(GuiController gui, CurrencyController currency)
        {
            this.gui = gui;
            this.currency = currency;
        }

        public void FlyFromWorld(Transform from, CurrencyType currencyType, int value, int flyCount = DefaultFlyCount)
        {
            Vector3 startPos = Camera.main.WorldToScreenPoint(from.position);
            StartFlyTo(startPos, gui.FindUniversalElement<TopCountersContainer>().FlyUiTran(currencyType), currencyType, value, flyCount);
        }

        public void FlyFromUI(Transform from, CurrencyType currencyType, int value, int flyCount = DefaultFlyCount)
        {
            StartFlyTo(from.transform.position, gui.FindUniversalElement<TopCountersContainer>().FlyUiTran(currencyType), currencyType, value, flyCount);
        }

        private void StartFlyTo(Vector3 startPos, RectTransform to, CurrencyType currencyType, int value, int flyCount)
        {
            var distance = to.position - startPos;

            Action endAction = () =>
            {
                currency.AddCurrency(currencyType, value);
                gui.FindUniversalElement<TopCountersContainer>().IncomeMessage(currencyType, value);
            };

            var handful = Instantiate(handfulPrefab, startPos, Quaternion.identity, transform);
            handful.transform.localPosition = new Vector3(handful.transform.localPosition.x, handful.transform.localPosition.y, 0);
            handful.Fly(flyCount, iconsConfig.Get.GetCurrencyIcon(currencyType), to, endAction);
        }
    }
}