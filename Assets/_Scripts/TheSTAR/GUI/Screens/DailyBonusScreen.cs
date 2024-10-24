using UnityEngine;
using Zenject;
using System;

namespace TheSTAR.GUI
{
    public class DailyBonusScreen : GuiScreen
    {
        [SerializeField] private DailyUIElement[] dailyElements = new DailyUIElement[0];
        [SerializeField] private Transform lightTran;
        [SerializeField] private Transform nextDayUI;
        [SerializeField] private DailyBonusConfig dailyBonusConfig;
        [SerializeField] private IconsConfig iconsConfig;
        [SerializeField] private PointerButton claimButton;
        [SerializeField] private PointerButton closeButton;

        private DailyBonusConfig.DailyBonusData _currentDailyBonusData;

        private GuiController gui;
        private DailyBonusService dailyBonus;
        private CurrencyController currency;
        private TutorialController tutor;
        private NotificationController notifications;

        [Inject]
        private void Construct(GuiController gui, DailyBonusService dailyBonus, CurrencyController currency, TutorialController tutor, NotificationController notifications)
        {
            this.gui = gui;
            this.dailyBonus = dailyBonus;
            this.currency = currency;
            this.tutor = tutor;
            this.notifications = notifications;
        }

        public override void Init()
        {
            claimButton.Init(OnClaimClick);
            closeButton.Init(gui.ShowMainScreen);
        }

        private int currentBonusIndex;

        protected override void OnShow()
        {
            base.OnShow();

            currentBonusIndex = dailyBonus.GetCurrentBonusIndex();
            _currentDailyBonusData = dailyBonusConfig.dailyBonuses[currentBonusIndex];

            DailyUIElement element;
            for (int i = 0; i < dailyElements.Length; i++)
            {
                element = dailyElements[i];
                var reward = dailyBonusConfig.dailyBonuses[i].rewards[0];

                element.Init(i, i <= currentBonusIndex, iconsConfig.GetCurrencyIcon((CurrencyType)dailyBonus.ConvertToCurrencyType(reward.rewardType)), reward.rewardValue);
            }

            Invoke(nameof(DelayActivateLight), 0.1f);
        }

        private void DelayActivateLight()
        {
            // current
            lightTran.position = dailyElements[currentBonusIndex].transform.position;
            lightTran.gameObject.SetActive(true);

            // next
            if (currentBonusIndex < dailyElements.Length - 1)
            {
                nextDayUI.position = dailyElements[currentBonusIndex + 1].transform.position;
                nextDayUI.gameObject.SetActive(true);
            }
        }

        public void OnClaimClick()
        {
            if (_currentDailyBonusData != null)
            {
                foreach (var reward in _currentDailyBonusData.rewards)
                {
                    switch (reward.rewardType)
                    {
                        case DailyRewardType.Soft:
                            currency.AddCurrency(CurrencyType.Soft, reward.rewardValue);
                            break;

                        case DailyRewardType.Hard:
                            currency.AddCurrency(CurrencyType.Hard, reward.rewardValue);
                            break;
                    }
                }
            }

            dailyBonus.OnGetDailyReward();
            gui.ShowMainScreen();

            DateTime nextDailyBonusNotificationDateTime = DateTime.Today.AddDays(1).AddHours(12);
            notifications.RegisterNotification(NotificationType.DailyBonus, nextDailyBonusNotificationDateTime);
        }
    }
}