using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "WheelOfFortune/Settings", fileName = "WheelOfFortune_Settings", order = 11)]
    public class WheelOfFortune_Settings : ScriptableObject
    {
        [field: SerializeField] public int ItemCount { get; private set; }
        [field: SerializeField] public float RotationSpeed { get; private set; }
        [field: SerializeField] public AnimationCurve AnimationCurve { get; private set; }
        [field: SerializeField] public float RotateDuration { get; private set; }
        [field: SerializeField] public int SafeZoneThresold { get; private set; }
        [field: SerializeField] public int SpecialZoneThresold { get; private set; }
        [field: SerializeField] public float ZoneSwapDuration { get; private set; }
        [field: SerializeField] public float DummyItemsSpreadRadius { get; private set; }
        [field: SerializeField] public float DummyItemsSpreadDuration { get; private set; }
        [field: SerializeField] public float DummyItemsPanelMoveSpeed { get; private set; }

        [field: SerializeField] public WheelProperties SilverWheelProperties { get; private set; }
        [field: SerializeField] public WheelProperties BronzeWheelProperties { get; private set; }
        [field: SerializeField] public WheelProperties GoldWheelProperties { get; private set; }
    }
}