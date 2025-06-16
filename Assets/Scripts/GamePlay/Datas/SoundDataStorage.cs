using System.Collections.Generic;
using UnityEngine;

public class SoundDataStorage : DataStorage
{
    public List<SoundEntry> sounds;

    [System.Serializable]
    public class SoundEntry
    {
        public string key;
        public float volume;
        public float pitch;
        public List<AudioClip> clip;
    }

    public void CheckResources()
    {
        Debug.Log($"SoundDataStorage - \n / sounds.Count = {sounds.Count}");
    }
}

