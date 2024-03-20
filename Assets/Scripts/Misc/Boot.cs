using UnityEngine;

namespace Misc
{
    public static class Boot
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void Init()
        {
            Application.targetFrameRate = 300;
        }
    }
}