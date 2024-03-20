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
    [RequireComponent(typeof(RectTransform))]
    public class WheelOfFortune_BaseRenderer : MonoBehaviour
    {
        [SerializeField] private RectTransform _dummyItemsParent;
        [SerializeField] private Image _dummyItemPrefab;
        [SerializeField] private RectTransform _indicatorTransform;
        [SerializeField] private RectTransform _wheelOfFortuneBase;
        [SerializeField] private Image _wheelBaseImage;
        [SerializeField] private Image _wheelIndicatorImage;
        [SerializeField] private WheelOfFortune_Item_Renderer _itemPrefab;

        [SerializeField] private float itemYOffset;
        [SerializeField] private float wheelRadiusOffset;

        [SerializeField][HideInInspector] private Button _spinButton;
        
        private RectTransform rectTransform;
        private WheelOfFortune_Model model;

        private Dictionary<int, WheelOfFortune_Item_Renderer> itemRenderers =
            new Dictionary<int, WheelOfFortune_Item_Renderer>();

        private WheelOfFortune_Item_Renderer lastRewardedItemRenderer;

        public void Initialize(WheelOfFortune_Model model)
        {
            this.model = model;

            rectTransform = this.GetComponent<RectTransform>();

            CreateWheelItemVisuals();
        }

        private void CreateWheelItemVisuals()
        {
            ClearWheelItemsVisuals();
            SetWheelVisuals(model.wheelOfFortune.wheelProperties);

            float angleStep = 360f / model.wheelOfFortuneSettings.ItemCount;
            Vector2 midpoint = rectTransform.position;
            _wheelOfFortuneBase.rotation = Quaternion.identity;

            for (int i = 0; i < model.wheelOfFortune.wheelProperties.Items.Length; i++)
            {
                WheelProperties.ItemProperties itemProperties = model.wheelOfFortune.wheelProperties.Items[i];

                float angle = i * angleStep;
                Vector2 slotPosition = CalculateSlotPosition(angle, midpoint);

                WheelOfFortune_Item_Renderer itemRenderer = Instantiate(_itemPrefab, _wheelOfFortuneBase);
                itemRenderer.RectTransform.anchoredPosition = slotPosition;

                Vector2 direction = midpoint - slotPosition;
                float rotationAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                itemRenderer.RectTransform.rotation = Quaternion.Euler(0f, 0f, rotationAngle + 90);

                itemRenderer.AmountText.enabled = true;
                itemRenderer.Image.sprite = itemProperties.Item.Sprite;

                if (itemProperties.Item is WheelOfFortune_Normal_Item_Data item)
                    itemRenderer.AmountText.text = "x" + itemProperties.Amount;
                else
                    itemRenderer.AmountText.text = "x1";

                itemRenderers[i] = itemRenderer;
            }
        }

        private void SetWheelVisuals(WheelProperties wheelProperties)
        {
            _wheelBaseImage.sprite = wheelProperties.BaseSprite;
            _wheelIndicatorImage.sprite = wheelProperties.IndicatorSprite;
        }

        public async Task<List<GameObject>> CreateDummyRewardedItemsTask(SpinResolveResult spinResolveResult, int amount)
        {
            List<GameObject> dummyItems = new List<GameObject>();
            List<Task> moveDummyItemsTasks = new List<Task>();

            Vector3 middlePoint = itemRenderers[spinResolveResult.itemIndex].RectTransform.position;

            float angleIncrement = 360f / amount;
            for (int i = 0; i < amount; i++)
            {
                float angle = Mathf.Deg2Rad * (i * angleIncrement);
                float x = middlePoint.x + model.wheelOfFortuneSettings.DummyItemsSpreadRadius * Mathf.Cos(angle);
                float y = middlePoint.y + model.wheelOfFortuneSettings.DummyItemsSpreadRadius * Mathf.Sin(angle);
                Vector3 movePosition =
                    new Vector3(x, y, itemRenderers[spinResolveResult.itemIndex].RectTransform.position.z);

                var dummyItem = Instantiate(_dummyItemPrefab, _dummyItemsParent);
                dummyItem.transform.position = middlePoint;
                dummyItem.sprite = spinResolveResult.ItemProperties.Item.Sprite;
                dummyItem.preserveAspect = true;

                dummyItems.Add(dummyItem.gameObject);

                Task moveDummyItemTask = dummyItem.transform
                    .DOMove(movePosition, model.wheelOfFortuneSettings.DummyItemsSpreadDuration)
                    .AsyncWaitForCompletion();
                moveDummyItemsTasks.Add(moveDummyItemTask);
            }

            await Task.WhenAll(moveDummyItemsTasks);

            return dummyItems;
        }

        private void ClearWheelItemsVisuals()
        {
            foreach (var itemRenderer in itemRenderers)
                Destroy(itemRenderer.Value.gameObject);

            itemRenderers.Clear();
        }

        private Vector2 CalculateSlotPosition(float angle, Vector2 midpoint)
        {
            float radians = angle * Mathf.Deg2Rad;
            float x = midpoint.x + Mathf.Sin(radians) * rectTransform.sizeDelta.x / wheelRadiusOffset;
            float y = midpoint.y + Mathf.Cos(radians) * rectTransform.sizeDelta.y / wheelRadiusOffset;

            return new Vector2(x, y - itemYOffset);
        }

        private async void OnSpinTheWheel(SpinResolveResult spinResolveResult)
        {
            await SpinTheWheelTask(spinResolveResult);
        }

        private async Task SpinTheWheelTask(SpinResolveResult spinResolveResult)
        {
            float duration = model.wheelOfFortuneSettings.RotateDuration;
            float timer = 0.0f;
            float startAngle = _wheelOfFortuneBase.eulerAngles.z;
            float maxAngle = 360f * duration +
                             (spinResolveResult.itemIndex * (360f / model.wheelOfFortuneSettings.ItemCount));
            AnimationCurve animationCurve = model.wheelOfFortuneSettings.AnimationCurve;

            maxAngle = maxAngle - startAngle;

            while (timer < duration)
            {
                float angle = maxAngle * animationCurve.Evaluate(timer / duration);
                _wheelOfFortuneBase.eulerAngles = new Vector3(0.0f, 0.0f, angle + startAngle);
                timer += Time.deltaTime;
                await Task.Yield();
            }

            _wheelOfFortuneBase.eulerAngles = new Vector3(0.0f, 0.0f, maxAngle + startAngle);

            model.SpinEnded(spinResolveResult);
        }

        private void OnCanSpinAgain()
        {
            _spinButton.interactable = true;
        }

        private void OnSpinButton()
        {
            _spinButton.interactable = false;
            model.SpinTheWheel();
        }

        private void OnValidate()
        {
            _spinButton = transform.Find("ui_spin_generic_button_value").GetComponent<Button>();
        }

        private void Start()
        {
            _spinButton.onClick.AddListener(OnSpinButton);

            model.OnSpinTheWheel += OnSpinTheWheel;
            model.OnCanSpinAgain += OnCanSpinAgain;
        }

        private void OnDestroy()
        {
            _spinButton.onClick.RemoveListener(OnSpinButton);

            model.OnSpinTheWheel -= OnSpinTheWheel;
            model.OnCanSpinAgain -= OnCanSpinAgain;
        }
    }
}