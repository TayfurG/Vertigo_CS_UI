using Data;
using Model;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class WheelOfFortune_BombScreenRenderer : MonoBehaviour
    {
        [SerializeField] private RectTransform _gameoverScreen;
        [SerializeField] private Button _giveUpButton;
        [SerializeField] private Button _reviveButton;

        private WheelOfFortune_Model model;

        public void Initialize(WheelOfFortune_Model model)
        {
            this.model = model;
        }

        private void OnSpinEnded(SpinResolveResult spinResolveResult)
        {
            if (spinResolveResult.ItemProperties.Item is WheelOfFortune_Bad_Item_Data)
                _gameoverScreen.gameObject.SetActive(true);
        }

        private void OnGiveUpButton()
        {
            model.GiveUp();
            _gameoverScreen.gameObject.SetActive(false);
        }

        private void OnReviveButton()
        {
            model.Revive();
            _gameoverScreen.gameObject.SetActive(false);
        }

        private void Start()
        {
            model.OnSpinEnded += OnSpinEnded;
        }

        private void OnDestroy()
        {
            model.OnSpinEnded -= OnSpinEnded;
        }

        private void OnValidate()
        {
            _giveUpButton.onClick.AddListener(OnGiveUpButton);
            _reviveButton.onClick.AddListener(OnReviveButton);
        }
    }
}