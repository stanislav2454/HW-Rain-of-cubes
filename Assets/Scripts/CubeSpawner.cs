using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Pool))]
public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private float _spawnInterval = 1f;
    [SerializeField] private float _spawnAreaRadius = 5f;
    [SerializeField] private float _spawnHeight = 15f;
    [SerializeField] private Pool _cubePool;

    private Coroutine _spawningCoroutine;

    private void OnEnable()
    {
        if (_cubePool == null)
        {
            enabled = false;
            return;
        }

        StartSpawning();
    }

    private void OnDisable() =>
        StopSpawning();

    private void Awake() =>
        _cubePool = GetComponent<Pool>();

    private void StartSpawning()
    {
        StopSpawning();
        _spawningCoroutine = StartCoroutine(SpawnCubes());
    }

    private void StopSpawning()
    {
        if (_spawningCoroutine != null)
        {
            StopCoroutine(_spawningCoroutine);
            _spawningCoroutine = null;
        }
    }

    private IEnumerator SpawnCubes()
    {
        while (enabled)
        {
            SpawnSingleCube();
            yield return new WaitForSeconds(_spawnInterval);
        }
    }

    private void SpawnSingleCube()
    {
        if (_cubePool == null)
        {
            enabled = false;
            return;
        }

        var cube = _cubePool.GetPooledObject();

        if (cube == null)
            return;

        cube.transform.position = GetRandomPosition();
    }

    private Vector3 GetRandomPosition()
    {
        return new Vector3(
            Random.Range(-_spawnAreaRadius, _spawnAreaRadius),
            _spawnHeight,
            Random.Range(-_spawnAreaRadius, _spawnAreaRadius)
        );
    }

    //public void SetSpawnInterval(float newInterval)
    //{
    //    _spawnInterval = Mathf.Max(0.1f, newInterval);
    //    StartSpawning();
    //}
}