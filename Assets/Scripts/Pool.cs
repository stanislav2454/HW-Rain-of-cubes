using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    [SerializeField] private CubeController _cubePrefab;
    [SerializeField] private int _poolSize = 5;
    [SerializeField] private Queue<CubeController> _pooledObjects = new();

    private void Start()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < _poolSize; i++)
            CreateNewPooledObject();
    }

    private CubeController CreateNewPooledObject()
    {
        CubeController obj = Instantiate(_cubePrefab, transform);
        obj.gameObject.SetActive(false);
        _pooledObjects.Enqueue(obj);

        return obj;
    }

    public CubeController GetPooledObject()
    {
        foreach (var obj in _pooledObjects)
        {
            if (obj.gameObject.activeInHierarchy == false)
            {
                obj.gameObject.SetActive(true);
                return obj;
            }
        }

        return CreateNewPooledObject();
    }

    public void ReturnToPool(CubeController obj)
    {
        obj.gameObject.transform.SetParent(transform);

        obj.GetComponent<CubeController>()?.ResetCube();
    }
}