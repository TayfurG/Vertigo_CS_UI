using Data;
using UnityEngine;

namespace Model
{
    public class WheelOfFortune : IWheelOfFortune
    {
        public WheelProperties wheelProperties { get; private set; }

        private readonly WheelOfFortune_Settings wheelOfFortuneSettings;

        private int zoneIndex = 1;
        private SpinResolveResult lastSpinResolveResult;

        public static WheelOfFortune Create(WheelOfFortune_Settings wheelOfFortuneSettings)
        {
            return new WheelOfFortune(wheelOfFortuneSettings);
        }

        public WheelOfFortune(WheelOfFortune_Settings wheelOfFortuneSettings)
        {
            this.wheelOfFortuneSettings = wheelOfFortuneSettings;

            SetWheelProperties();
        }

        private void SetWheelProperties()
        {
            wheelProperties =
                IsSpecialZone(zoneIndex) ? wheelOfFortuneSettings.GoldWheelProperties :
                IsSafeZone(zoneIndex) ? wheelOfFortuneSettings.BronzeWheelProperties :
                wheelOfFortuneSettings.SilverWheelProperties;
        }

        public bool IsSpecialZone(int zoneIndex) => zoneIndex % wheelOfFortuneSettings.SpecialZoneThresold == 0;
        public bool IsSafeZone(int zoneIndex) => zoneIndex % wheelOfFortuneSettings.SafeZoneThresold == 0;

        public SpinResolveResult Resolve()
        {
            int randomSlot = Random.Range(0, wheelProperties.Items.Length);

            SpinResolveResult spinResolveResult =
                new SpinResolveResult(wheelProperties, wheelProperties.Items[randomSlot], randomSlot, zoneIndex);

            lastSpinResolveResult = spinResolveResult;

            if (spinResolveResult.ItemProperties.Item is not WheelOfFortune_Bad_Item_Data)
                SetNextZone();

            return spinResolveResult;
        }

        public void SetNextZone()
        {
            zoneIndex++;
            SetWheelProperties();
        }

        public int GetZoneIndex() => zoneIndex;
    }
}