using UnityEngine;

public class ExplosiveObject : MonoBehaviour
{
    [SerializeField] private float _explosionRadius = 5f;
    [SerializeField] private float _explosionForce = 10f;
    [SerializeField] private float _upwardsModifier = 1f;
    [SerializeField] private LayerMask _explosionLayerMask = Physics.AllLayers;

    public event System.Action ExplosionOccurred;

    public void Explode()
    {
        var colliders = Physics.OverlapSphere(transform.position, _explosionRadius, _explosionLayerMask);

        foreach (var collider in colliders)
            if (collider.TryGetComponent<Rigidbody>(out var rb))
                rb.AddExplosionForce(_explosionForce, transform.position, _explosionRadius, _upwardsModifier, ForceMode.Impulse);

        ExplosionOccurred?.Invoke();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }
}