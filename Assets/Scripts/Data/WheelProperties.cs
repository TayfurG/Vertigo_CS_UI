using System;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class WheelProperties
    {
        [field: SerializeField] public ItemProperties[] Items { get; private set; }

        [field: SerializeField] public Sprite BaseSprite { get; private set; }
        [field: SerializeField] public Sprite IndicatorSprite { get; private set; }

        [Serializable]
        public class ItemProperties
        {
            [field: SerializeField] public WheelOfFortune_Item_Data Item { get; private set; }
            [field: SerializeField] public int Amount { get; private set; }
        }
    }
}