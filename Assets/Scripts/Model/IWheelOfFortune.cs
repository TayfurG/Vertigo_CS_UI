using Data;

namespace Model
{
    public interface IWheelOfFortune
    {
        public WheelProperties wheelProperties { get; }

        SpinResolveResult Resolve();
        public bool IsSpecialZone(int zoneIndex);
        public bool IsSafeZone(int zoneIndex);
        public void SetNextZone();
        public int GetZoneIndex();
    }
}