using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Views.Ref
{
    public class PanelItem_Renderer : MonoBehaviour
    {
        [field: SerializeField] public TextMeshProUGUI AmountText { get; private set; }
        [field: SerializeField] public Image Image { get; private set; }
    }
}
