using UnityEngine;

    [System.Serializable]
    public class AudioData
    {
        public string Name;
        public AudioClip audioClip;

        public bool isLooping;

        [Range(0f, 1f)]
        public float volume;

        [Range(0.3f, 3f)]
        public float pitch;

        [HideInInspector]
        public GameObject go;

        [HideInInspector]
        public AudioSource audioSource;
    }
