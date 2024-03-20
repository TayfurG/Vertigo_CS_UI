using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using DG.Tweening;
using Model;
using UnityEngine;
using UnityEngine.UI;
using Views.Ref;

namespace Views
{
    public class WheelOfFortune_PanelRenderer : MonoBehaviour
    {
        [SerializeField] private PanelItem_Renderer _panelItemPrefab;
        [SerializeField] private Button _exitButton;
        [SerializeField] private RectTransform _panelItemsParent;

        private WheelOfFortune_Model model;

        private Dictionary<WheelOfFortune_Item_Data, PanelItem_Renderer> _panelItemRenderers =
            new Dictionary<WheelOfFortune_Item_Data, PanelItem_Renderer>();

        public void Initialize(WheelOfFortune_Model model)
        {
            this.model = model;
            ClearPanelItemRendererVisuals();
        }

        private void OnExitClick()
        {
            model.Exit();
        }

        private void ClearPanelItemRendererVisuals()
        {
            foreach (var panelItemRenderer in _panelItemRenderers)
                Destroy(panelItemRenderer.Value.gameObject);

            _panelItemRenderers.Clear();
        }

        public async Task GetDummyRewardedItemsToPanelTask(SpinResolveResult spinResolveResult, List<GameObject> dummyItems)
        {
            List<Task> moveDummyItemsTasks = new List<Task>();
            var item = _panelItemRenderers[spinResolveResult.ItemProperties.Item];

            foreach (var dummyItem in dummyItems)
                moveDummyItemsTasks.Add(dummyItem.transform
                    .DOMove(item.Image.rectTransform.position, model.wheelOfFortuneSettings.DummyItemsPanelMoveSpeed)
                    .SetEase(Ease.Linear)
                    .SetSpeedBased(true)
                    .AsyncWaitForCompletion());

            await Task.WhenAll(moveDummyItemsTasks);

            foreach (var dummyItem in dummyItems)
                Destroy(dummyItem);

            var currentAmount = model.GetItemAmount(spinResolveResult.ItemProperties.Item) -
                                spinResolveResult.wheelProperties.Items[spinResolveResult.itemIndex].Amount;

            DOTween.To(() => currentAmount, x =>
            {
                currentAmount = x;
                _panelItemRenderers[spinResolveResult.ItemProperties.Item].AmountText.text = "x" + currentAmount;
            }, model.GetItemAmount(spinResolveResult.ItemProperties.Item), 0.25f).SetEase(Ease.Linear);
        }

        public void AddRewardedItemVisualToPanel(SpinResolveResult spinResolveResult)
        {
            var item = spinResolveResult.ItemProperties.Item;
            PanelItem_Renderer panelItem = null;

            if (!_panelItemRenderers.TryGetValue(spinResolveResult.ItemProperties.Item, out panelItem))
            {
                panelItem = Instantiate(_panelItemPrefab, _panelItemsParent);
                panelItem.Image.sprite = item.Sprite;
                panelItem.AmountText.text = "x0";

                _panelItemRenderers[spinResolveResult.ItemProperties.Item] = panelItem;
            }
        }

        private void OnSpinTheWheel(SpinResolveResult obj)
        {
            _exitButton.interactable = false;
        }

        private void OnCanSpinAgain()
        {
            var canExit = !model.wheelOfFortune.IsSafeZone(model.wheelOfFortune.GetZoneIndex()) &&
                          !model.wheelOfFortune.IsSpecialZone(model.wheelOfFortune.GetZoneIndex());

            _exitButton.interactable = canExit;
        }

        private void Start()
        {
            model.OnSpinTheWheel += OnSpinTheWheel;
            model.OnCanSpinAgain += OnCanSpinAgain;
        }

        private void OnDestroy()
        {
            model.OnSpinTheWheel -= OnSpinTheWheel;
            model.OnCanSpinAgain -= OnCanSpinAgain;
        }

        private void OnValidate()
        {
            _exitButton.onClick.AddListener(OnExitClick);
        }
    }
}