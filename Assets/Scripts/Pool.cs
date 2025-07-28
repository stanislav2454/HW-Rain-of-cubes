using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    [SerializeField] private int _poolSize = 5;
    [SerializeField] private CubeController _cubePrefab;
    [SerializeField] private Queue<CubeController> _pooledObjects = new();

    private void Start()
    {
        InitializePool();
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

        return CreateNewPooledCube();
    }

    public void ReturnToPool(CubeController obj)
    {
        obj.gameObject.transform.SetParent(transform);
        obj.gameObject.SetActive(false);
        _pooledObjects.Enqueue(obj);
        obj.GetComponent<CubeController>()?.ResetCube();
    }

    private void InitializePool()
    {
        for (int i = 0; i < _poolSize; i++)
            CreateNewPooledCube();
    }

    private CubeController CreateNewPooledCube()
    {
        CubeController cube = Instantiate(_cubePrefab, transform);
        cube.gameObject.SetActive(false);
        _pooledObjects.Enqueue(cube);

        return cube;
    }

    private CubeController CreateNewPooledCubeManually()
    {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obj.SetActive(false);
        var cube = obj.GetComponent<CubeController>();
        _pooledObjects.Enqueue(cube);
        // для примера
        //cube.gameObject.transform.parent = transform;
        //cube.gameObject.tag = "";
        //cube.gameObject.name = "";
        //cube.gameObject.AddComponent <Light>();

        return cube;
    }
}