using Data;
using Extentions;
using Model;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace Views
{
    public class WheelOfFortuneScreen : MonoBehaviour
    {
        [SerializeField] private Button[] _spinButtons;
        [SerializeField] private PlayableDirector _timeLine;

        [SerializeField] private WheelOfFortune_BaseRenderer _wheelOfFortuneBaseRenderer;
        [SerializeField] private WheelOfFortune_ZoneRenderer _wheelOfFortuneZoneRenderer;
        [SerializeField] private WheelOfFortune_PanelRenderer _wheelOfFortunePanelRenderer;
        [SerializeField] private WheelOfFortune_BombScreenRenderer _wheelOfFortuneBombScreenRenderer;

        [SerializeField] private WheelOfFortune_Settings _wheelOfFortuneSettings;

        private WheelOfFortune_Model model;

        private void Awake()
        {
            this.model = WheelOfFortune_Model.Create(_wheelOfFortuneSettings);
            Initialize();
        }

        public void Initialize()
        {
            model.Reset();

            _wheelOfFortuneBaseRenderer.Initialize(model);
            _wheelOfFortuneZoneRenderer.Initialize(model);
            _wheelOfFortunePanelRenderer.Initialize(model);
            _wheelOfFortuneBombScreenRenderer.Initialize(model);
        }

        private async void OnSpinEnded(SpinResolveResult spinResolveResult)
        {
            if (spinResolveResult.ItemProperties.Item is WheelOfFortune_Bad_Item_Data) return;

            _wheelOfFortunePanelRenderer.AddRewardedItemVisualToPanel(spinResolveResult);

            var dummyItems = await _wheelOfFortuneBaseRenderer.CreateDummyRewardedItemsTask(spinResolveResult, 5);

            await _timeLine.PlayTask();
            await _wheelOfFortunePanelRenderer.GetDummyRewardedItemsToPanelTask(spinResolveResult, dummyItems);
            await _wheelOfFortuneZoneRenderer.NextZoneTask(spinResolveResult);

            model.CanSpinAgain();

            await _timeLine.ReversePlayTask();
        }

        private void OnGiveUp()
        {
            model.CanSpinAgain();
            Initialize();
        }

        private void OnExit() => Initialize();
        private void OnRevive() => model.CanSpinAgain();

        private void Start()
        {
            model.OnSpinEnded += OnSpinEnded;
            model.OnGiveUp += OnGiveUp;
            model.OnExit += OnExit;
            model.OnRevive += OnRevive;
        }

        private void OnDestroy()
        {
            model.OnSpinEnded -= OnSpinEnded;
            model.OnGiveUp -= OnGiveUp;
            model.OnExit -= OnExit;
            model.OnRevive -= OnRevive;
        }
    }
}