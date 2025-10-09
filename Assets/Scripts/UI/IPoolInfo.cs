public interface IPoolInfo
{
    int SpawnedCount { get; }
    int CreatedCount { get; }
    int ActiveCount { get; }
    int InactiveCount { get; }
    string ObjectType { get; }
}