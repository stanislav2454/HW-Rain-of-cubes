using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Pool<>))]
public class GenericSpawner<T> : MonoBehaviour where T : MonoBehaviour, IPoolable
{
    [SerializeField] private float _spawnInterval = 1f;
    [SerializeField] private float _spawnAreaRadius = 5f;
    [SerializeField] private float _spawnHeight = 15f;

    private Pool<T> _objectPool;
    private Coroutine _spawningCoroutine;

    private void OnEnable()
    {
        if (TryGetComponent(out _objectPool))
        {
            StartSpawning();
        }
        else
        {
            Debug.LogError($"Pool<{typeof(T).Name}> component not found!");
            enabled = false;
        }
    }

    private void OnDisable() => 
        StopSpawning();

    private void StartSpawning()
    {
        StopSpawning();
        _spawningCoroutine = StartCoroutine(SpawnObjects());
    }

    private void StopSpawning()
    {
        if (_spawningCoroutine != null)
        {
            StopCoroutine(_spawningCoroutine);
            _spawningCoroutine = null;
        }
    }

    private IEnumerator SpawnObjects()
    {
        while (enabled)
        {
            SpawnSingleObject();
            yield return new WaitForSeconds(_spawnInterval);
        }
    }

    private void SpawnSingleObject()
    {
        if (_objectPool == null)
        {
            enabled = false;
            return;
        }

        var obj = _objectPool.GetPooledObject();

        if (obj == null)
            return;

        obj.transform.position = GetRandomPosition();
    }

    private Vector3 GetRandomPosition()
    {
        return new Vector3(
            Random.Range(-_spawnAreaRadius, _spawnAreaRadius),
            _spawnHeight,
            Random.Range(-_spawnAreaRadius, _spawnAreaRadius)
        );
    }
}