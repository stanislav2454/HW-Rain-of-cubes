using UnityEngine;

public class BombPool : Pool<Bomb>
{
    protected override void Awake()
    {
        base.Awake();

        Cube.OnCubeCreated += OnCubeCreated;
    }

    private void OnDestroy() =>
        Cube.OnCubeCreated -= OnCubeCreated;

    private void OnEnable() =>
        Cube.OnCubeCreated += OnCubeCreated;

    private void OnDisable() =>
        Cube.OnCubeCreated -= OnCubeCreated;

    public new Bomb GetPooledObject()
    {
        Bomb bomb = base.GetPooledObject();

        if (bomb != null)
            bomb.Initialize();

        return bomb;
    }

    private void OnCubeCreated(Cube cube) =>
        cube.SetBombPool(this);

#if UNITY_EDITOR
    [ContextMenu("ContextMenu /ExplodeAllBombs ( )")]
    public void ExplodeAllBombs()
    {
        var activeBombs = FindObjectsOfType<Bomb>();

        foreach (var bomb in activeBombs)
            if (bomb.gameObject.activeInHierarchy)
                bomb.Invoke("Explode", 0.1f);
    }
#endif
}