using UnityEngine;
using System;

    public class EffectsStorage : MonoBehaviour
    {
        #region Singleton
        public static EffectsStorage Instance { private set; get; }
        #endregion

        public Effect[] effects;

        void Awake()
        {
            Instance = this;
        }
    }

    public static class EffectsEmitter
    {
        public static void Emit(string effectName, Vector3 position, Quaternion rotation)
        {
            Effect effect = Array.Find(EffectsStorage.Instance.effects, x => x.name == effectName);
            GameObject.Instantiate(effect.prefab, position, rotation);
        }
    }

    [Serializable]
    public class Effect
    {
        public string name;
        public GameObject prefab;
    }