using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Views.Ref
{
    public class WheelOfFortune_Item_Renderer : MonoBehaviour
    {
        [field: SerializeField] public Image Image { get; private set; }
        [field: SerializeField] public TextMeshProUGUI AmountText { get; private set; }
        [field: SerializeField] public RectTransform RectTransform { get; private set; }
    }
}