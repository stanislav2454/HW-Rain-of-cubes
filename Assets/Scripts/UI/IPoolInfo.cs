public interface IPoolInfo
{
    int SpawnedCount { get; }
    int ActiveCount { get; }
    int InactiveCount { get; }
    string PoolName { get; }
}