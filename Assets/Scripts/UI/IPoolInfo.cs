public interface IPoolInfo
{
    int SpawnedCount { get; }
    int CreatedCount { get; }
    int ActiveCount { get; }
    string ObjectType { get; }
}