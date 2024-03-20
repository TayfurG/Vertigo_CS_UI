using Data;
using DG.Tweening;
using Model;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class WheelOfFortune_BombScreenRenderer : MonoBehaviour
    {
        [SerializeField] private RectTransform _gameoverScreen;
        [SerializeField][HideInInspector] private Button _giveUpButton;
        [SerializeField][HideInInspector] private Button _reviveButton;
        
        private CanvasGroup _canvasGroup;
        private WheelOfFortune_Model model;

        public void Initialize(WheelOfFortune_Model model)
        {
            this.model = model;
        }

        private void OnSpinEnded(SpinResolveResult spinResolveResult)
        {
            if (spinResolveResult.ItemProperties.Item is WheelOfFortune_Bad_Item_Data)
                DOTween.To(()=> _canvasGroup.alpha, x=> _canvasGroup.alpha = x, 1f, 0.25f);
        }

        private void OnGiveUpButton()
        {
            model.GiveUp();
            
            DOTween.To(()=> _canvasGroup.alpha, x=> _canvasGroup.alpha = x, 0, 0.25f);
        }

        private void OnReviveButton()
        {
            model.Revive();
            DOTween.To(()=> _canvasGroup.alpha, x=> _canvasGroup.alpha = x, 0, 0.25f);
        }

        private void Start()
        {
            _canvasGroup = transform.GetComponent<CanvasGroup>();

            _giveUpButton.onClick.AddListener(OnGiveUpButton);
            _reviveButton.onClick.AddListener(OnReviveButton);

            model.OnSpinEnded += OnSpinEnded;
        }

        private void OnDestroy()
        {
            _giveUpButton.onClick.RemoveListener(OnGiveUpButton);
            _giveUpButton.onClick.RemoveListener(OnReviveButton);

            model.OnSpinEnded -= OnSpinEnded;
        }

        private void OnValidate()
        {
            _giveUpButton = GameObject.Find("give_up_button_value").GetComponent<Button>();
            _reviveButton = GameObject.Find("revive_button_value").GetComponent<Button>();
        }
    }
}