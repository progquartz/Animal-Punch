using System.Collections.Generic;
using UnityEngine;

public class SoundDataStorage : MonoBehaviour
{
    public List<SoundEntry> sounds;

    [System.Serializable]
    public class SoundEntry
    {
        public string key;
        public float volume;
        public AudioClip clip;
    }
}

