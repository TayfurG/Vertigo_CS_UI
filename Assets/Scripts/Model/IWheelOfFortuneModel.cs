using System;
using System.Collections.Generic;
using Data;

namespace Model
{
    public interface IWheelOfFortuneModel
    {
        public IWheelOfFortune wheelOfFortune { get; }
        public WheelOfFortune_Settings wheelOfFortuneSettings { get; }
        public Dictionary<WheelOfFortune_Item_Data, int> rewardedItems { get; }

        event Action<SpinResolveResult> OnSpinTheWheel;
        event Action<SpinResolveResult> OnSpinEnded;
        event Action OnGiveUp;
        event Action OnCanSpinAgain;
        event Action OnExit;
        event Action OnRevive;

        int GetItemAmount(WheelOfFortune_Item_Data item);
        void Reset();
        void CanSpinAgain();
        void GiveUp();
        void Exit();
        void Revive();
        void SpinEnded(SpinResolveResult spinResolveResult);
        void SpinTheWheel();
    }
}