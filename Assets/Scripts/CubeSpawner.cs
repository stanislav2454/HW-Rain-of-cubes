using UnityEngine;
using System.Collections;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private float _spawnInterval = 1f;
    [SerializeField] private float _spawnAreaRadius = 5f;
    [SerializeField] private float _spawnHeight = 15f;
    [SerializeField] private Pool _cubePool;

    private Coroutine _spawningCoroutine;

    private void OnEnable()
    {
        _spawningCoroutine = StartCoroutine(SpawnCubes());
    }

    private void OnDisable()
    {
        if (_spawningCoroutine != null)
            StopCoroutine(_spawningCoroutine);
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
        CubeController cube = _cubePool.GetPooledObject();

        Vector3 spawnPosition = new Vector3(
                Random.Range(-_spawnAreaRadius, _spawnAreaRadius),
                _spawnHeight,
                Random.Range(-_spawnAreaRadius, _spawnAreaRadius));

        cube.gameObject.transform.position = spawnPosition;
    }
}