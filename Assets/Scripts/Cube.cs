using System.Collections;
using UnityEngine;

public class Cube : MonoBehaviour, IPoolable
{
    [SerializeField] float _minLifetime = 2f;
    [SerializeField] float _maxLifetime = 5f;
    [SerializeField] private Bomb _bombPrefab;

    public event System.Action<Cube> OnCubeDestroyed;

    private ColorChanger _colorChanger;
    private Pool<Cube> _pool;
    private Coroutine _lifetimeCoroutine;
    private bool _hasCollided = false;

    private void Awake() =>
        _colorChanger = GetComponent<ColorChanger>();

    public void SetPool<T>(Pool<T> pool) where T : MonoBehaviour, IPoolable =>
        _pool = pool as Pool<Cube>;

    public void ResetCube()
    {
        _hasCollided = false;

        if (_lifetimeCoroutine != null)
        {
            StopCoroutine(_lifetimeCoroutine);
            _lifetimeCoroutine = null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_hasCollided && collision.collider.GetComponent<Platform>())
        {
            _hasCollided = true;
            _colorChanger.SetRandomColor();
            float lifetime = Random.Range(_minLifetime, _maxLifetime);
            _lifetimeCoroutine = StartCoroutine(ReturnAfterLifetime(lifetime));
        }
    }

    private IEnumerator ReturnAfterLifetime(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        OnCubeDestroyed?.Invoke(this);
        SpawnBomb();
        _pool.ReturnToPool(this);
    }

    private void SpawnBomb()
    {
        if (_bombPrefab != null)
        {
            Bomb bomb = Instantiate(_bombPrefab, transform.position, Quaternion.identity);
            bomb.Initialize();
        }
    }
}