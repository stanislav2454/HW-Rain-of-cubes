using System.Collections;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    [SerializeField] float _minLifetime = 2f;
    [SerializeField] float _maxLifetime = 5f;

    private ColorChanger _colorChanger;
    private Pool _pool;
    private bool _hasCollided = false;

    private Coroutine _lifetimeCoroutine;

    private void Awake()
    {
        _colorChanger = GetComponent<ColorChanger>();
        _pool = GetComponentInParent<Pool>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_hasCollided == false && collision.collider.GetComponent<Platform>())
        {
            _hasCollided = true;
            _colorChanger.SetRandomColor();
            float lifetime = Random.Range(_minLifetime, _maxLifetime);
            _lifetimeCoroutine = StartCoroutine(ReturnAfterLifetime(lifetime));
        }
    }

    public void ResetCube()
    {
        _hasCollided = false;

        if (_lifetimeCoroutine != null)
        {
            StopCoroutine(_lifetimeCoroutine);
            _lifetimeCoroutine = null;
        }
    }

    private IEnumerator ReturnAfterLifetime(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        ReturnToPool();
    }

    private void ReturnToPool() =>
        _pool?.ReturnToPool(gameObject.GetComponent<CubeController>());
}