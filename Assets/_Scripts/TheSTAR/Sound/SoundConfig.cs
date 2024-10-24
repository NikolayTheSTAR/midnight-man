using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheSTAR.Sound
{
    [CreateAssetMenu(menuName = "Data/Sound", fileName = "SoundConfig")]
    public class SoundConfig : ScriptableObject
    {
        [SerializeField] private List<SoundData> soundDatas = new List<SoundData>();
        [SerializeField] private List<MusicData> musicDatas = new List<MusicData>();

        [Space]
        [SerializeField] private float timeClear = 1; 
        [SerializeField] private float timeMusicChange = 1;

        public float TimeClear => timeClear;
        public float TimeMusicChange => timeMusicChange;

        public MusicData GetData(MusicType type)
        {
            return musicDatas.Find(info => info.Type == type);
        }

        public SoundData GetData(SoundType type)
        {
            return soundDatas.Find(info => info.Type == type);
        }
        
        [Serializable]
        public class SoundData
        {
            [SerializeField] private AudioClip clip;
            [SerializeField] private SoundType type;
            [SerializeField] [Range(0, 1)] private float volume = 1;
            /// <summary>
            /// Может ли одновременно воспроизводиться больше 1 экземпляра звука
            /// </summary>
            [SerializeField] private bool canMultiply = false;
            [SerializeField] private bool loop = false;
            
            public AudioClip Clip => clip;
            public float Volume => volume;
            public bool CanMultiply => canMultiply;
            public bool Loop => loop;
            public SoundType Type => type;
        }

        [Serializable]
        public class MusicData
        {
            [SerializeField] private AudioClip clip;
            [SerializeField] private MusicType type;
            [SerializeField] [Range(0, 1)] private float volume = 1;
            [SerializeField] private bool loop = true;
            [SerializeField] private string name;

            public AudioClip Clip => clip;
            public float Volume => volume;
            public bool Loop => loop;
            public MusicType Type => type;
            public string Name => name;
        }
    }

    public enum SoundType
    {
        Click
        // здесь добавить необходимые типы звуков
    }

    public enum MusicType
    {
        MainTheme
        // здесь добавить необходимые типы музыки
    }
}