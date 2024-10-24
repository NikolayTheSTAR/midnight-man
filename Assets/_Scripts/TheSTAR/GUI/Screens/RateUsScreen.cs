using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheSTAR.Data;
using Zenject;

namespace TheSTAR.GUI
{
    public class RateUsScreen : GuiScreen
    {
        [SerializeField] private PointerButton closeButton;
        [SerializeField] private PointerButton noButton;
        [SerializeField] private PointerButton yesButton;

        public static string GetRateUsURL => GetGooglePlayURL;
        private static string GetGooglePlayURL => $"https://play.google.com/store/apps/details?id={Application.identifier}";

        private GuiController gui;
        private DataController data;

        [Inject]
        private void Construct(GuiController gui, DataController data)
        {
            this.gui = gui;
            this.data = data;
        }

        public override void Init()
        {
            base.Init();

            closeButton.Init(OnNoClick);
            noButton.Init(OnNoClick);
            yesButton.Init(OnYesClick);
        }

        private void OnNoClick()
        {
            PlanNextRateUs();
            gui.ShowMainScreen();
        }

        private void OnYesClick()
        {
            Application.OpenURL(GetRateUsURL);
            //GameController.Instance.IAR.ShowInAppReview();
            gui.ShowMainScreen();
            data.gameData.commonData.gameRated = true;
            data.Save(DataSectionType.Common);
        }

        private void PlanNextRateUs()
        {
            var planDateTime = System.DateTime.Now.AddDays(3);
            data.gameData.commonData.nextRateUsPlan = planDateTime;
            data.gameData.commonData.rateUsPlanned = true;
            data.Save(DataSectionType.Common);
            Debug.Log("Plan for next rate us: " + planDateTime);
        }
    }
}