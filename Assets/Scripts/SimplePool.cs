using UnityEngine;
using System.Collections.Generic;

public class SimplePool : MonoBehaviour, IPoolInfo
{
    [System.Serializable]
    public class PoolItem
    {
        public string name;
        public GameObject prefab;
        public int initialSize = 10;
    }

    [SerializeField] private PoolItem[] _poolItems;

    private Dictionary<string, Queue<GameObject>> _poolDictionary = new Dictionary<string, Queue<GameObject>>();
    private Dictionary<string, int> _totalSpawned = new Dictionary<string, int>();
    private Dictionary<string, int> _activeCount = new Dictionary<string, int>();

    public int SpawnedCount => 
        GetTotalSpawnedForFirstPool();

    public int ActiveCount => 
        GetActiveCountForFirstPool();

    public int InactiveCount => 
        GetPoolCountForFirstPool();

    public string PoolName => 
        GetPoolName();

    public event System.Action PoolCountChanged;
    //public event System.Action<string> PoolCountChanged;

    private void Start()
    {
        foreach (var item in _poolItems)
        {
            if (string.IsNullOrEmpty(item.name) == false && item.prefab != null)
            {
                var queue = new Queue<GameObject>();// todo to interface

                for (int i = 0; i < item.initialSize; i++)
                {
                    var obj = CreateInstance(item.prefab);
                    queue.Enqueue(obj);
                }
                _poolDictionary[item.name] = queue;
                _totalSpawned[item.name] = 0;
                _activeCount[item.name] = 0;
            }
        }
    }

    public T Get<T>(string poolName) where T : MonoBehaviour
    {
        if (_poolDictionary.TryGetValue(poolName, out var queue))
        {
            GameObject obj;// todo to interface

            if (queue.Count == 0)
            {
                var prefab = FindPrefab(poolName);

                if (prefab != null)
                {
                    obj = CreateInstance(prefab);
                    obj.SetActive(true);
                    _totalSpawned[poolName]++;
                    _activeCount[poolName]++;
                    PoolCountChanged?.Invoke();
                    return obj.GetComponent<T>();
                }
                return null;
            }

            obj = queue.Dequeue();
            obj.SetActive(true);
            _totalSpawned[poolName]++;
            _activeCount[poolName]++;
            PoolCountChanged?.Invoke();
            return obj.GetComponent<T>();
        }
        return null;
    }

    public void Return(string poolName, GameObject obj)// todo to interface
    {
        if (obj != null && _poolDictionary.TryGetValue(poolName, out var queue))
        {
            obj.SetActive(false);
            queue.Enqueue(obj);
            _activeCount[poolName]--;
            PoolCountChanged?.Invoke();
        }
    }

    public int GetPoolCount(string poolName) =>
         _poolDictionary.TryGetValue(poolName, out var queue) ? queue.Count : 0;

    public int GetTotalSpawned(string poolName) =>
         _totalSpawned.TryGetValue(poolName, out var count) ? count : 0;

    public int GetActiveCount(string poolName) =>
         _activeCount.TryGetValue(poolName, out var count) ? count : 0;

    private int GetTotalSpawnedForFirstPool()
    {
        foreach (var item in _totalSpawned) return item.Value;
        return 0;
    }

    private int GetActiveCountForFirstPool()
    {
        foreach (var item in _activeCount) return item.Value;
        return 0;
    }

    private int GetPoolCountForFirstPool()
    {
        foreach (var queue in _poolDictionary.Values) return queue.Count;
        return 0;
    }

    private string GetPoolName()
    {// public  ???
        foreach (var item in _poolItems) 
            return item.name;
            
        return "Unknown";
    }

    private GameObject CreateInstance(GameObject prefab)
    {
        var obj = Instantiate(prefab, transform);
        obj.SetActive(false);
        return obj;
    }

    private GameObject FindPrefab(string poolName)
    {
        foreach (var item in _poolItems)
            if (item.name == poolName)
                return item.prefab;

        return null;
    }
}