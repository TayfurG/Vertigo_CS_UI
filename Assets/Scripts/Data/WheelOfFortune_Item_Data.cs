using UnityEngine;

namespace Data
{
    public abstract class WheelOfFortune_Item_Data : ScriptableObject
    {
        [SerializeField] private Sprite _sprite;
        public Sprite Sprite => _sprite;
    }
}