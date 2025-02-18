using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        #region Singleton

        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(gameObject);
            else Instance = this;
            
            Init();
        }

        #endregion

        private Dictionary<string, GameObject> _prefabs = new();

        private void Init()
        {
            _prefabs = Resources.LoadAll<GameObject>("Prefabs").ToDictionary(go => go.name, go => go);
        }
        
        public T GetPrefab<T>(string prefName) where T: class
        {
            var hasValue = _prefabs.TryGetValue(prefName, out var prefab);
            return hasValue ? prefab.GetComponent<T>() : null;
        }
        
        public GameObject GetPrefab(string prefName)
        {
            var hasValue = _prefabs.TryGetValue(prefName, out var prefab);
            return hasValue ? prefab : null;
        }
    }
}