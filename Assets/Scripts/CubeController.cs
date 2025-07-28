using UnityEngine;

public class CubeController : MonoBehaviour
{
    [SerializeField] private Color _collisionColor = Color.green;
    [SerializeField] float _minLifetime = 2f;
    [SerializeField] float _maxLifetime = 5f;

    private ColorChanger _colorChanger;
    private Pool _pool;
    private bool _hasCollided = false;

    private void Awake()
    {
        _colorChanger = GetComponent<ColorChanger>();
        _pool = GetComponentInParent<Pool>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.GetComponent<Platform>() && _hasCollided == false)
        {
            _hasCollided = true;
            _colorChanger.SetRandomColor();
            float lifetime = Random.Range(_minLifetime, _maxLifetime);
            Invoke(nameof(ReturnToPool), lifetime);
        }
    }

    private void ReturnToPool() =>
        _pool?.ReturnToPool(gameObject.GetComponent<CubeController>());

    public void ResetCube()
    {
        _hasCollided = false;
        //_colorChanger?.SetRandomColor();

        CancelInvoke(nameof(ReturnToPool));
    }
}