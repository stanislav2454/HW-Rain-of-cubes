using UnityEngine;

public class StatisticsManager : MonoBehaviour
{
    private int _totalCubesSpawned = 0;
    private int _totalBombsSpawned = 0;
    private int _totalExplosions = 0;

    public int TotalCubesSpawned => _totalCubesSpawned;
    public int TotalBombsSpawned => _totalBombsSpawned;
    public int TotalExplosions => _totalExplosions;

    public event System.Action StatisticsChanged;

    public void IncrementCubesSpawned()
    {
        _totalCubesSpawned++;
        StatisticsChanged?.Invoke();
    }

    public void IncrementBombsSpawned()
    {
        _totalBombsSpawned++;
        StatisticsChanged?.Invoke();
    }

    public void IncrementExplosions()
    {
        _totalExplosions++;
        StatisticsChanged?.Invoke();
    }

    public void ResetStatistics()
    {
        _totalCubesSpawned = 0;
        _totalBombsSpawned = 0;
        _totalExplosions = 0;
        StatisticsChanged?.Invoke();
    }
}