using Data;

namespace Model
{
    public class SpinResolveResult
    {
        public WheelProperties wheelProperties { get; private set; }
        public WheelProperties.ItemProperties ItemProperties { get; private set; }
        public int itemIndex { get; private set; }
        public int zoneIndex { get; private set; }

        public SpinResolveResult(WheelProperties wheelProperties, WheelProperties.ItemProperties ItemProperties,
            int itemIndex, int zoneIndex)
        {
            this.wheelProperties = wheelProperties;
            this.ItemProperties = ItemProperties;
            this.itemIndex = itemIndex;
            this.zoneIndex = zoneIndex;
        }
    }
}