using UnityEngine;
using UnityEngine.Pool;

public class Pool<T> : MonoBehaviour, IPoolInfo where T : MonoBehaviour, IPoolable
{
    [SerializeField] [Range(0, 20)] private int _defaultCapacity = 10;
    [SerializeField] [Range(1, 20)] private int _maxSize = 20;
    //[SerializeField] private int _defaultCapacity = 10;
    //[SerializeField] private int _maxSize = 20;
    [SerializeField] private bool _collectionCheck = true;
    [SerializeField] private T _prefab;
    [SerializeField] private string _objectTypeName;

    private IObjectPool<T> _pool;
    private int _spawnedCount;
    private int _createdCount;

    public int SpawnedCount => _spawnedCount;
    public int CreatedCount => _createdCount;
    public int ActiveCount => _pool?.CountInactive ?? 0;
    //public int ActiveCount => _pool?.CountActive ?? 0;
    public string ObjectType => _objectTypeName;

    protected virtual void Awake()
    {
        _pool = new ObjectPool<T>(
            CreatePooledObject,
            GetFromPool,
            OnReturnedToPool,
            OnDestroyPooledObject,
            _collectionCheck,
            _defaultCapacity,
            _maxSize
        );
    }

    private void OnValidate()
    {
        if (_defaultCapacity < _maxSize)
        {
            return;
        }
        _defaultCapacity = _maxSize - 1;
    }

    public T GetPooledObject()
    {
        if (_pool == null)
            return null;

        _spawnedCount++;
        return _pool.Get();
    }

    public void ReturnToPool(T obj)
    {
        if (_pool != null)
            _pool.Release(obj);
    }

    protected virtual void OnDestroyPooledObject(T obj) =>
        Destroy(obj.gameObject);

    protected virtual T CreatePooledObject()
    {
        T obj = Instantiate(_prefab, transform);
        obj.gameObject.SetActive(false);
        obj.SetPool(this);
        _createdCount++;

        return obj;
    }

    private void GetFromPool(T obj) =>
        obj.gameObject.SetActive(true);

    private void OnReturnedToPool(T obj) =>
        obj.gameObject.SetActive(false);
}
