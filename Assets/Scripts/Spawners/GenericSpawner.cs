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

    public System.Action<bool> OnSpawnerStateChanged;

    public bool IsSpawning { get; private set; }

    private void OnEnable()
    {
        if (TryGetComponent(out _objectPool))
            StartSpawning();
        else
            enabled = false;
    }

    private void OnDisable()
    {
        StopSpawning();
    }

    private void StartSpawning()
    {
        if (IsSpawning)
            return;

        IsSpawning = true;
        _spawningCoroutine = StartCoroutine(SpawnRoutine());
        OnSpawnerStateChanged?.Invoke(true);
    }

    private void StopSpawning()
    {
        if (IsSpawning == false)
            return;

        IsSpawning = false;

        if (_spawningCoroutine != null)
        {
            StopCoroutine(_spawningCoroutine);
            _spawningCoroutine = null;
        }

        OnSpawnerStateChanged?.Invoke(false);
    }

    private IEnumerator SpawnRoutine()
    {
        while (enabled)
        {
            SpawnObject();
            yield return new WaitForSeconds(_spawnInterval);
        }
    }

    private void SpawnObject()
    {
        var obj = _objectPool?.GetPooledObject();

        if (obj != null)
            obj.transform.position = GetRandomPosition();
    }

    private Vector3 GetRandomPosition() =>
         new Vector3(Random.Range(-_spawnAreaRadius, _spawnAreaRadius),
                     _spawnHeight,
                     Random.Range(-_spawnAreaRadius, _spawnAreaRadius));
}