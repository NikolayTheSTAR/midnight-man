using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheSTAR.Data;
using TheSTAR.Sound;
using Zenject;

namespace TheSTAR.GUI
{
    public class SettingsScreen : GuiScreen
    {
        [SerializeField] private PointerButton closeButton;
        [SerializeField] private PointerToggle soundsToggle;
        [SerializeField] private PointerToggle musicToggle;
        [SerializeField] private PointerToggle vibrationsToggle;
        [SerializeField] private PointerToggle notificationsToggle;
        [SerializeField] private PointerButton privacyButton;
        [SerializeField] private PointerButton rateUsButton;

        [Header("Cheats")]
        [SerializeField] private GameObject cheatsContainer;
        [SerializeField] private PointerButton cheatAddCurrencyBtn;
        [SerializeField] private PointerButton cheatClearCurrencyBtn;

        private DataController data;
        private SoundController sound;
        private GuiController gui;
        private CurrencyController currency;
        private GameLoader gameLoader;

        [Inject]
        private void Construct(DataController data, GuiController gui, CurrencyController currency, GameLoader game, SoundController sound)
        {
            this.data = data;
            this.gui = gui;
            this.currency = currency;
            this.gameLoader = game;
            this.sound = sound;
        }

        public override void Init()
        {
            base.Init();

            closeButton.Init(gui.ShowMainScreen);

            soundsToggle.Init(data.gameData.settingsData.isSoundsOn, OnToggleSounds);
            musicToggle.Init(data.gameData.settingsData.isMusicOn, OnToggleMusic);
            vibrationsToggle.Init(data.gameData.settingsData.isVibrationOn, OnToggleVibration);
            notificationsToggle.Init(data.gameData.settingsData.isNotificationsOn, OnToggleNotifications);
            rateUsButton.Init(() => gui.Show<RateUsScreen>());

            privacyButton.Init(() =>
            {
                Application.OpenURL(GDPRScreen.PrivatyURL);
            });

            cheatAddCurrencyBtn.Init(() =>
            {
                currency.AddCurrency(CurrencyType.Soft, 1000);
            });

            cheatClearCurrencyBtn.Init(() =>
            {
                currency.ClearCurrency(CurrencyType.Soft);
            });
        }

        protected override void OnShow()
        {
            base.OnShow();
            cheatsContainer.SetActive(gameLoader.GameConfig.UseCheats);
        }

        protected override void OnHide()
        {
            base.OnHide();
            data.Save(DataSectionType.Settings);
        }

        private void OnToggleSounds(bool value)
        {
            data.gameData.settingsData.isSoundsOn = value;
        }

        private void OnToggleMusic(bool value)
        {
            data.gameData.settingsData.isMusicOn = value;

            if (value) sound.Play(MusicType.MainTheme);
            else sound.Stop();
        }

        private void OnToggleVibration(bool value)
        {
            data.gameData.settingsData.isVibrationOn = value;
        }

        private void OnToggleNotifications(bool value)
        {
            data.gameData.settingsData.isNotificationsOn = value;
        }
    }
}