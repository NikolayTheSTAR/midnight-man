using UnityEngine;
using Zenject;

namespace TheSTAR.GUI
{
    public class GameScreen : GuiScreen
    {
        [SerializeField] private PointerButton settingsBtn;
        [SerializeField] private PointerButton dailyBonusBtn;
        [SerializeField] private HpBar hpBar;

        private DailyBonusService dailyBonus;
        private GuiController gui;
        private CurrencyController currency;
        private FlyUIContainer flyUI;

        [Inject]
        private void Construct(
            DailyBonusService dailyBonus, 
            GuiController gui, 
            CurrencyController currency, 
            FlyUIContainer flyUI, 
            Player player)
        {
            this.dailyBonus = dailyBonus;
            this.gui = gui;
            this.currency = currency;
            this.flyUI = flyUI;

            player.HpSystem.OnChangeHpEvent += hpBar.Set;
        }

        public override void Init()
        {
            base.Init();

            settingsBtn.Init(() => gui.Show<SettingsScreen>());
            dailyBonusBtn.Init(() => gui.Show<DailyBonusScreen>());
        }

        protected override void OnShow()
        {
            base.OnShow();

            dailyBonusBtn.gameObject.SetActive(dailyBonus.NeedShowDailyBonus);
        }

        [ContextMenu("Cheat Add Currency")]
        private void CheatAddCurrency()
        {
            currency.AddCurrency(CurrencyType.Hard, 10);
        }

        [ContextMenu("Test Handful Soft")]
        private void CheatHandfulSoft()
        {
            flyUI.FlyFromUI(transform, CurrencyType.Soft, 200, 20);
        }

        [ContextMenu("Test Handful Hard")]
        private void CheatHandfulHard()
        {
            flyUI.FlyFromUI(transform, CurrencyType.Hard, 10, 10);
        }
    }
}