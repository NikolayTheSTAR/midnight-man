using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using TheSTAR.Utility;
using Zenject;

namespace TheSTAR.Data
{
    [Serializable]
    public sealed class DataController
    {
        [SerializeField] private bool lockSaves = false; // когда true, перезапись сохранений заблокирована, файлы сохранений не могут быть изменены

        [Header("Test")]
        [SerializeField] private int testCompleteEvents;

        [Inject]
        private void Construct()
        {
            if (clearData) LoadDefault();
            else LoadAll();
        }

        public void Init(bool lockSaves)
        {
            this.lockSaves = lockSaves;
        }

        [ContextMenu("Save")]
        private void ForceSave()
        {
            SaveAll(true);
        }

        public void SaveAll(bool force = false)
        {
            var allSections = EnumUtility.GetValues<DataSectionType>();
            foreach (var section in allSections) Save(section, force);
        }

        public void Save(DataSectionType secionType, bool force = false)
        {
            if (!force && lockSaves) return;

            JsonSerializerSettings settings = new() { TypeNameHandling = TypeNameHandling.Objects };
            var section = gameData.GetSection(secionType);
            string jsonString = JsonConvert.SerializeObject(section, Formatting.Indented, settings);

            PlayerPrefs.SetString(section.DataFileName, jsonString);

            //Debug.Log($"Save {secion}");
        }

        [ContextMenu("Load")]
        private void LoadAll()
        {
            if (PlayerPrefs.HasKey(gameData.GetSection(0).DataFileName))
            {
                var allSections = EnumUtility.GetValues<DataSectionType>();
                foreach (var section in allSections) LoadSection(section);
            }
            else LoadDefault();

            void LoadSection(DataSectionType section)
            {

                string jsonString = PlayerPrefs.GetString(gameData.GetSection(section).DataFileName);
                var loadedData = JsonConvert.DeserializeObject<DataSection>(jsonString, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });
                gameData.SetSection(loadedData);
            }
        }

        [ContextMenu("ClearData")]
        private void LoadDefault()
        {
            gameData = new();
            SaveAll();
        }

        [Header("Data")]
        public GameData gameData = new();

        [SerializeField] private bool clearData = false;

        [Serializable]
        public class GameData
        {
            public CommonData commonData;
            public SettingsData settingsData;
            public CurrencyData currencyData;
            public LevelData levelData;
            public InappsData inappsData;
            public NotificationData notificationData;
            public TutorialData tutorialData;
            public DailyBonusData dailyBonusData;
            public PlayerData playerData;

            public GameData()
            {
                commonData = new();
                settingsData = new();
                currencyData = new();
                levelData = new();
                inappsData = new();
                notificationData = new();
                tutorialData = new();
                dailyBonusData = new();
                playerData = new();
            }

            public DataSection GetSection(DataSectionType sectionType)
            {
                switch (sectionType)
                {
                    case DataSectionType.Common: return commonData;
                    case DataSectionType.Settings: return settingsData;
                    case DataSectionType.Currency: return currencyData;
                    case DataSectionType.Level: return levelData;
                    case DataSectionType.InappsData: return inappsData;
                    case DataSectionType.Notifications: return notificationData;
                    case DataSectionType.Tutorial: return tutorialData;
                    case DataSectionType.DailyBonus: return dailyBonusData;
                    case DataSectionType.Player: return playerData;
                    default:
                        break;
                }

                return null;
            }
            public void SetSection(DataSection sectionData)
            {
                switch (sectionData.SectionType)
                {
                    case DataSectionType.Common:
                        commonData = (CommonData)sectionData;
                        break;

                    case DataSectionType.Settings:
                        settingsData = (SettingsData)sectionData;
                        break;

                    case DataSectionType.Currency:
                        currencyData = (CurrencyData)sectionData;
                        break;

                    case DataSectionType.Level:
                        levelData = (LevelData)sectionData;
                        break;

                    case DataSectionType.InappsData:
                        inappsData = (InappsData)sectionData;
                        break;

                    case DataSectionType.Notifications:
                        notificationData = (NotificationData)sectionData;
                        break;

                    case DataSectionType.Tutorial:
                        tutorialData = (TutorialData)sectionData;
                        break;

                    case DataSectionType.DailyBonus:
                        dailyBonusData = (DailyBonusData)sectionData;
                        break;
                    
                    case DataSectionType.Player:
                        playerData = (PlayerData)sectionData;
                        break;
                }
            }
        }

        [Serializable]
        public abstract class DataSection
        {
            public abstract DataSectionType SectionType { get; }
            public abstract string DataFileName { get; }
        }

        [Serializable]
        public class CommonData : DataSection
        {
            public override DataSectionType SectionType => DataSectionType.Common;
            public override string DataFileName => "common_data";

            public bool gdprAccepted;

            // rate us
            public bool gameRated;
            public bool rateUsPlanned;
            public DateTime nextRateUsPlan;

            public bool gameStarted = false;
        }

        [Serializable]
        public class SettingsData : DataSection
        {
            public override DataSectionType SectionType => DataSectionType.Settings;
            public override string DataFileName => "settings_data";

            public bool isMusicOn = true;
            public bool isSoundsOn = true;
            public bool isVibrationOn = true;
            public bool isNotificationsOn = true;
        }

        [Serializable]
        public class CurrencyData : DataSection
        {
            public override DataSectionType SectionType => DataSectionType.Currency;
            public override string DataFileName => "currency_data";

            public Dictionary<CurrencyType, int> currencyData;

            public CurrencyData() => currencyData = new();

            public void AddCurrency(CurrencyType currencyType, int count, out int result)
            {
                if (currencyData.ContainsKey(currencyType)) currencyData[currencyType] += count;
                else currencyData.Add(currencyType, count);

                result = currencyData[currencyType];
            }

            public int GetCurrencyCount(CurrencyType currencyType)
            {
                if (currencyData.ContainsKey(currencyType)) return currencyData[currencyType];
                else return 0;
            }

            public void ClearAll()
            {
                currencyData = new();
            }
        }

        // Данные по прогрессу игрока в рамках уровня
        [Serializable]
        public class LevelData : DataSection
        {
            public override DataSectionType SectionType => DataSectionType.Level;
            public override string DataFileName => "level_data";

            public Dictionary<int, bool> collectedItems = new();
            public List<DropData> dropData = new();
            public List<EnemyData> enemies = new();

            public void ClearAll()
            {
                collectedItems = new();
                dropData = new();
                enemies = new();
            }
        }

        [Serializable]
        public class InappsData : DataSection
        {
            public override DataSectionType SectionType => DataSectionType.InappsData;
            public override string DataFileName => "inapps_data";
        }

        [Serializable]
        public class NotificationData : DataSection
        {
            public override DataSectionType SectionType => DataSectionType.Notifications;
            public override string DataFileName => "notifications_data";

            /// <summary>
            /// Хранит id зарегестрированных нотификаций. Если id равен -1, значит нотификация неактивна (например, она была отменена)
            /// </summary>
            public Dictionary<NotificationType, int> registredNotifications;

            public void ClearNotification(NotificationType notificationType) => RegisterNotification(notificationType, -1);

            public void RegisterNotification(NotificationType notificationType, int id)
            {
                if (registredNotifications == null) registredNotifications = new Dictionary<NotificationType, int>();

                if (registredNotifications.ContainsKey(notificationType)) registredNotifications[notificationType] = id;
                else registredNotifications.Add(notificationType, id);
            }

            /// <summary>
            /// Возвращает id зарегестрированной нотификации. Если нотификация не зарегестрирована, возвращает -1
            /// </summary>
            public int GetRegistredNotificationID(NotificationType notificationType)
            {
                if (registredNotifications == null)
                {
                    registredNotifications = new Dictionary<NotificationType, int>();
                    return -1;
                }

                if (registredNotifications.ContainsKey(notificationType)) return registredNotifications[notificationType];
                else return -1;
            }
        }

        [Serializable]
        public class TutorialData : DataSection
        {
            public override DataSectionType SectionType => DataSectionType.Tutorial;
            public override string DataFileName => "tutorials_data";

            public List<string> completedTutorials;

            public void CompleteTutorial(string id)
            {
                if (!completedTutorials.Contains(id)) completedTutorials.Add(id);
            }

            public void UncompleteTutorial(string id)
            {
                if (completedTutorials.Contains(id)) completedTutorials.Remove(id);
            }
        }

        [Serializable]
        public class DailyBonusData : DataSection
        {
            public override DataSectionType SectionType => DataSectionType.DailyBonus;
            public override string DataFileName => "daily_bonus_data";

            public DateTime previousDailyBonusTime;
            public int currentDailyBonusIndex = -1;
            public bool bonusIndexWasUpdatedForThisDay;
        }
    
        [Serializable]
        public class PlayerData : DataSection
        {
            public override DataSectionType SectionType => DataSectionType.Player;
            public override string DataFileName => "player_data";

            public SerializedVector3 playerPosition;
            public int playerCurrentHp;
            public int playerMaxHp;
        }
    
        public struct EnemyData
        {
            public int currentHP;
            public int maxHP;
            public SerializedVector3 position;

            public EnemyData(int currentHP, int maxHP, Vector3 position)
            {
                this.currentHP = currentHP;
                this.maxHP = maxHP;
                this.position = position;
            }
        }

        public struct DropData
        {
            public ItemInWorldType itemInWorldType;
            public int value;
            public SerializedVector3 position;

            public DropData(ItemInWorldType itemInWorldType, int value, Vector3 position)
            {
                this.itemInWorldType = itemInWorldType;
                this.value = value;
                this.position = position;
            }
        }
    }

    public enum DataSectionType
    {
        Common,
        Settings,
        Currency,
        Level,
        InappsData,
        Notifications,
        Tutorial,
        DailyBonus,
        Player
    }
}

[Serializable]
public struct SerializedVector3
{
    public float x;
    public float y;
    public float z;

    public static implicit operator Vector3(SerializedVector3 value)
    {
        return new (value.x, value.y, value.z);
    }
    
    public static implicit operator SerializedVector3(Vector3 value)
    {
        return new SerializedVector3 
        {
            x = value.x,
            y = value.y, 
            z = value.z 
        };  
    }
}