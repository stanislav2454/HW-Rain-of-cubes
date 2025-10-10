using UnityEngine;
using System.Collections;

public class Cube : MonoBehaviour, IPoolable
{
    [SerializeField] private float _minLifetime = 2f;
    [SerializeField] private float _maxLifetime = 5f;

    private Rigidbody _rigidbody;
    private ColorChanger _colorChanger;
    private bool _hasCollided;
    private Coroutine _lifetimeCoroutine;

    private System.Action<Cube> Destroyed;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _colorChanger = GetComponent<ColorChanger>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_hasCollided == false && collision.collider.TryGetComponent<Platform>(out _))
        {
            _hasCollided = true;
            _colorChanger.SetRandomColor();
            _lifetimeCoroutine = StartCoroutine(DestroyAfterLifetime());
        }
    }

    public void OnSpawn()
    {
        _hasCollided = false;
        _colorChanger.SetColor(Color.white);
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }

    public void OnDespawn()
    {
        if (_lifetimeCoroutine != null)
        {
            StopCoroutine(_lifetimeCoroutine);
            _lifetimeCoroutine = null;
        }

        Destroyed = null;
    }

    public void SetDestroyCallback(System.Action<Cube> onDestroyed) =>
        Destroyed = onDestroyed;

    private IEnumerator DestroyAfterLifetime()
    {
        yield return new WaitForSeconds(Random.Range(_minLifetime, _maxLifetime));
        Destroyed?.Invoke(this);
    }
}