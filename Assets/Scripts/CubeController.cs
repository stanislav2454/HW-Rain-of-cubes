using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class CubeController : MonoBehaviour
{
    [SerializeField] float _minLifetime = 2f;
    [SerializeField] float _maxLifetime = 5f;

    private ColorChanger _colorChanger;
    private IObjectPool<CubeController> _pool;
    private Coroutine _lifetimeCoroutine;
    private bool _hasCollided = false;

    private void Awake() =>
        _colorChanger = GetComponent<ColorChanger>();

    public void SetPool(IObjectPool<CubeController> pool) =>
        _pool = pool;

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
        _pool.Release(this);
    }
}