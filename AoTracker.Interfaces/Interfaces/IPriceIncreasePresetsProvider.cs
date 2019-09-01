namespace AoTracker.Infrastructure.Infrastructure
{
    public interface IPriceIncreasePresetsProvider
    {
        (double Percentage, int Offset) GetPreset();
    }
}