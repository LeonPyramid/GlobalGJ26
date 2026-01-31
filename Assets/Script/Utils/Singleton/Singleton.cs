using System;
using UnityEngine;

namespace Utils.Singleton
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance => _instance != null ? _instance : null;

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
            }
            else if (_instance != this)
            {
                throw new InvalidOperationException($"Violation du singleton : Plus d'une instance de {typeof(T).Name} trouv�e.");
            }
        }
    }
}