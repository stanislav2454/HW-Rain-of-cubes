using UnityEngine;

public class BombSpawner : MonoBehaviour
{
    [SerializeField] private ObjectPool<Bomb> _bombPool;
    [SerializeField] private int _maxBombs = 20;

    private int _activeBombsCount = 0;

    public event System.Action BombSpawned;
    public event System.Action BombExploded;

    private void Start() =>
        ValidateReferences();

    private void ValidateReferences()
    {
        if (_bombPool == null)
            Debug.LogError("BombPool not set in BombSpawner", this);
    }

    public void SpawnBomb(Vector3 position)
    {
        if (_activeBombsCount >= _maxBombs)
            return;

        try
        {
            var bomb = _bombPool.Spawn();
            if (bomb != null)
            {
                bomb.transform.position = position;
                bomb.SetExplodeCallback(OnBombExploded);
                _activeBombsCount++;
                BombSpawned?.Invoke();
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error spawning bomb: {ex.Message}");
        }
    }

    private void OnBombExploded(Bomb bomb)
    {
        _bombPool.Despawn(bomb);
        _activeBombsCount--;
        _activeBombsCount = Mathf.Max(0, _activeBombsCount);
        BombExploded?.Invoke();
    }
}