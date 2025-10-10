using UnityEngine;
using System.Collections.Generic;

public class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour, IPoolable
{
    [System.Serializable]
    public class PoolConfig
    {
        public string poolName;
        public T prefab;
        public int initialSize = 10;
    }

    [SerializeField] private PoolConfig _config;

    private Queue<T> _pool = new Queue<T>();
    private int _totalSpawned = 0;
    private int _activeCount = 0;

    public string PoolName => _config.poolName;
    public int TotalSpawned => _totalSpawned;
    public int ActiveCount => _activeCount;
    public int InactiveCount => _pool.Count;

    public event System.Action<T> ObjectSpawned;
    public event System.Action<T> ObjectDespawned;

    private void Start()
    {
        for (int i = 0; i < _config.initialSize; i++)
            CreateInstance();
    }

    public T Spawn()
    {
        T obj = _pool.Count > 0 ? _pool.Dequeue() : CreateInstance();

        obj.gameObject.SetActive(true);
        obj.OnSpawn();

        _totalSpawned++;
        _activeCount++;
        ObjectSpawned?.Invoke(obj);

        return obj;
    }

    public void Despawn(T obj)
    {
        if (obj != null)
        {
            obj.OnDespawn();
            obj.gameObject.SetActive(false);
            _pool.Enqueue(obj);
            _activeCount--;
            ObjectDespawned?.Invoke(obj);
        }
    }

    private T CreateInstance()
    {
        var obj = Instantiate(_config.prefab, transform);
        obj.gameObject.SetActive(false);
        _pool.Enqueue(obj);
        return obj;
    }
}