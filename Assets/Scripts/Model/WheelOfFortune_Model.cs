using System;
using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Model
{
    public class WheelOfFortune_Model : IWheelOfFortuneModel
    {
        public IWheelOfFortune wheelOfFortune { get; private set; }
        public WheelOfFortune_Settings wheelOfFortuneSettings { get; private set; }
        public Dictionary<WheelOfFortune_Item_Data, int> rewardedItems { get; private set; }

        public event Action<SpinResolveResult> OnSpinTheWheel;
        public event Action<SpinResolveResult> OnSpinEnded;
        public event Action OnGiveUp;
        public event Action OnCanSpinAgain;
        public event Action OnExit;
        public event Action OnRevive;

        public static WheelOfFortune_Model Create(WheelOfFortune_Settings wheelOfFortuneSettings)
        {
            return new WheelOfFortune_Model(wheelOfFortuneSettings);
        }

        public WheelOfFortune_Model(WheelOfFortune_Settings wheelOfFortuneSettings)
        {
            this.wheelOfFortuneSettings = wheelOfFortuneSettings;
            this.wheelOfFortune = WheelOfFortune.Create(wheelOfFortuneSettings);
            this.rewardedItems = new Dictionary<WheelOfFortune_Item_Data, int>();
        }

        public int GetItemAmount(WheelOfFortune_Item_Data item)
        {
            return rewardedItems.ContainsKey(item) ? rewardedItems[item] : 0;
        }

        public void Reset()
        {
            this.wheelOfFortune = WheelOfFortune.Create(wheelOfFortuneSettings);
            rewardedItems.Clear();
        }

        public void CanSpinAgain() => OnCanSpinAgain?.Invoke();
        public void GiveUp() => OnGiveUp?.Invoke();
        public void Exit() => OnExit?.Invoke();
        public void Revive() => OnRevive?.Invoke();

        public void SpinEnded(SpinResolveResult spinResolveResult) => OnSpinEnded?.Invoke(spinResolveResult);

        public void SpinTheWheel()
        {
            var result = wheelOfFortune.Resolve();

            OnSpinTheWheel?.Invoke(result);

            if (result.ItemProperties.Item is not WheelOfFortune_Bad_Item_Data)
            {
                if (rewardedItems.ContainsKey(result.ItemProperties.Item))
                    rewardedItems[result.ItemProperties.Item] += result.wheelProperties.Items[result.itemIndex].Amount;
                else
                    rewardedItems[result.ItemProperties.Item] = result.wheelProperties.Items[result.itemIndex].Amount;
            }

            Debug.Log($"Item : {result.ItemProperties.Item.name} , Amount : {result.ItemProperties.Amount}");
        }
    }
}