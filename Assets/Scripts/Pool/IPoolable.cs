using UnityEngine;

public interface IPoolable
{
    void SetPool<T>(Pool<T> pool) where T : MonoBehaviour, IPoolable;
    void SetBombPool(BombPool bombPool); 
}