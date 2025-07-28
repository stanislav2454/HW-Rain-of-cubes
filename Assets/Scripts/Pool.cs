using UnityEngine;
using UnityEngine.Pool;

public class Pool : MonoBehaviour
{
    [SerializeField] private int _defaultCapacity = 10;
    [SerializeField] private int _maxSize = 20;
    [SerializeField] private bool _collectionCheck = true;
    [SerializeField] private CubeController _cubePrefab;

    private IObjectPool<CubeController> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<CubeController>(
            CreatePooledObject,
            GetFromPool,
            OnReturnedToPool,
            OnDestroyPooledObject,
            _collectionCheck,
            _defaultCapacity,
            _maxSize
        );
    }

    public CubeController GetPooledObject()
    {
        if (_pool == null)
            return null;

        return _pool.Get();
    }

    public void ReturnToPool(CubeController cube)
    {
        if (_pool != null)
            _pool.Release(cube);
    }

    private void OnDestroyPooledObject(CubeController cube) =>
            Destroy(cube.gameObject);

    private CubeController CreatePooledObject()
    {
        CubeController cube = Instantiate(_cubePrefab, transform);
        cube.gameObject.SetActive(false);
        cube.SetPool(_pool);

        return cube;
    }

    private void GetFromPool(CubeController cube)
    {
        cube.gameObject.SetActive(true);
        cube.ResetCube();
    }

    private void OnReturnedToPool(CubeController cube) =>
            cube.gameObject.SetActive(false);
}