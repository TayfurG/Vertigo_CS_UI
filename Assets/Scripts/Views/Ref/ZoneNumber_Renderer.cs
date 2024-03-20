using TMPro;
using UnityEngine;

namespace Views.Ref
{
    public class ZoneNumber_Renderer : MonoBehaviour
    {
        [field: SerializeField] public TextMeshProUGUI ZoneNumberText { get; private set; }
        [field: SerializeField] public RectTransform RectTransform { get; private set; }
    }
}
