using UnityEngine;

public class ExplosiveObject : MonoBehaviour
{
    [SerializeField] private float _explosionRadius = 5f;
    [SerializeField] private float _explosionForce = 10f;
    [SerializeField] private float _upwardsModifier = 1f;
    [SerializeField] private LayerMask _explosionLayerMask = Physics.AllLayers;

    public event System.Action<ExplosiveObject> OnExplosion;
    public static event System.Action OnAnyExplosion;

    public float ExplosionRadius => _explosionRadius;
    public float ExplosionForce => _explosionForce;
    public static int TotalExplosions { get; private set; }

    public void Explode(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, _explosionRadius, _explosionLayerMask);

        foreach (Collider collider in colliders)
            if (collider.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
                rigidbody.AddExplosionForce(_explosionForce, position, _explosionRadius, _upwardsModifier, ForceMode.Impulse);

        TotalExplosions++;
        OnExplosion?.Invoke(this);
        OnAnyExplosion?.Invoke();
    }

    public void Explode() =>
        Explode(transform.position);

    public void SetExplosionParameters(float radius, float force, float upwardsModifier)
    {
        _explosionRadius = radius;
        _explosionForce = force;
        _upwardsModifier = upwardsModifier;
    }

    public static void ResetExplosionCounter() =>
        TotalExplosions = 0;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }
}