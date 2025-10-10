using UnityEngine;

public class BombSpawner : MonoBehaviour
{
    private const string PoolName = "Bomb";

    [SerializeField] private SimplePool _bombPool;
    [SerializeField] private int _maxBombs = 20;

    private int _activeBombsCount = 0;

    // Событие для статистики
    public event System.Action BombSpawned;
    public event System.Action BombExploded;

    public void SpawnBomb(Vector3 position)
    {
        if (_activeBombsCount >= _maxBombs) 
            return;

        var bomb = _bombPool.Get<Bomb>(PoolName);

        if (bomb != null)
        {
            bomb.transform.position = position;
            bomb.Initialize(OnBombExploded);
            _activeBombsCount++;

            // Уведомляем статистику о создании бомбы
            BombSpawned?.Invoke();
        }
    }

    private void OnBombExploded(Bomb bomb)
    {
        _bombPool.Return(PoolName, bomb.gameObject);
        _activeBombsCount--;
        _activeBombsCount = Mathf.Max(0, _activeBombsCount);

        // Уведомляем статистику о взрыве бомбы
        BombExploded?.Invoke();
    }
}
//using UnityEngine;

//public class BombSpawner : GenericSpawner<Bomb>
//{
//    //protected override void SpawnObject()
//    //{
//    //    Debug.Log($"🚀 BombSpawner: Attempting to spawn bomb");
//    //    var bomb = _objectPool?.GetPooledObject();
//    //    Debug.Log($"🚀 BombSpawner: Bomb received - {bomb != null}");

//    //    if (bomb != null)
//    //    {
//    //        bomb.transform.position = GetRandomPosition();
//    //        Debug.Log($"🚀 BombSpawner: Bomb spawned at {bomb.transform.position}");

//    //        // ВАЖНО: Подписываем BombPool на события этой бомбы
//    //        if (_objectPool is BombPool bombPool)
//    //        {
//    //            bomb.BombExploded += bombPool.OnBombExploded;
//    //            Debug.Log($"🚀 BombSpawner: Subscribed BombPool to bomb events");
//    //        }

//    //        ObjectSpawned?.Invoke(bomb);
//    //       // ObjectSpawned?.Invoke(bomb);
//    //    }
//    //}
//}