
public class PlatformGenerationSettings
{
    public readonly float minSize;
    public readonly float maxSize;
    public readonly float minDistance;
    public readonly float maxDistance;

    public PlatformGenerationSettings(float minSize, float maxSize, float minDistance, float maxDistance)
    {
        this.minSize = minSize;
        this.maxSize = maxSize;
        this.minDistance = minDistance;
        this.maxDistance = maxDistance;
    }
}
