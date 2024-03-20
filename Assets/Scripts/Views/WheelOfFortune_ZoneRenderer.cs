using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using Model;
using UnityEngine;
using UnityEngine.UI;
using Views.Ref;

namespace Views
{
    [RequireComponent(typeof(RectTransform))]
    public class WheelOfFortune_ZoneRenderer : MonoBehaviour
    {
        [SerializeField] private ZoneNumber_Renderer _zoneNumberPrefab;
        [SerializeField] private RectTransform _zoneNumbersParent;
        [SerializeField] private HorizontalLayoutGroup _horizontalLayoutGroup;

        private const int INITIAL_ZONE_NUMBER_SPAWN_AMOUNT = 9;
        private WheelOfFortune_Model model;
        private RectTransform rectTransform;
        private int zonePaddingAmount;

        private Dictionary<int, ZoneNumber_Renderer> _zoneNumberRenderers = new Dictionary<int, ZoneNumber_Renderer>();

        public void Initialize(WheelOfFortune_Model model)
        {
            this.model = model;
            rectTransform = this.GetComponent<RectTransform>();

            zonePaddingAmount = (int)((rectTransform.sizeDelta.x / INITIAL_ZONE_NUMBER_SPAWN_AMOUNT));
            _zoneNumbersParent.anchoredPosition = Vector2.zero;

            ClearZoneVisuals();
            CreateInitialZoneVisuals();
        }

        private void ClearZoneVisuals()
        {
            foreach (var zoneNumberRenderer in _zoneNumberRenderers)
                Destroy(zoneNumberRenderer.Value.gameObject);

            _zoneNumberRenderers.Clear();
        }

        private void CreateInitialZoneVisuals()
        {
            for (int i = 0; i < INITIAL_ZONE_NUMBER_SPAWN_AMOUNT; i++)
                CreateZoneVisual(i + 1);

            _horizontalLayoutGroup.padding.left = (int)(rectTransform.sizeDelta.x / 2 - (zonePaddingAmount / 2));
        }

        private void CreateZoneVisual(int index)
        {
            ZoneNumber_Renderer itemRenderer = Instantiate(_zoneNumberPrefab, _zoneNumbersParent);
            itemRenderer.ZoneNumberText.text = (index).ToString();
            itemRenderer.ZoneNumberText.color = model.wheelOfFortune.IsSpecialZone(index) ? Color.yellow :
                model.wheelOfFortune.IsSafeZone(index) ? Color.green :
                Color.white;

            itemRenderer.RectTransform.sizeDelta =
                new Vector2(rectTransform.sizeDelta.x / INITIAL_ZONE_NUMBER_SPAWN_AMOUNT, rectTransform.sizeDelta.y);

            _zoneNumberRenderers[index] = itemRenderer;
        }

        public async Task NextZoneTask(SpinResolveResult spinResolveResult)
        {
            CreateZoneVisual(_zoneNumberRenderers.Count + 1);
            Vector2 paddingAmount = new Vector2(-zonePaddingAmount * spinResolveResult.zoneIndex, 0);
            await _zoneNumbersParent.DOAnchorPos(paddingAmount, model.wheelOfFortuneSettings.ZoneSwapDuration)
                .AsyncWaitForCompletion();
        }
    }
}