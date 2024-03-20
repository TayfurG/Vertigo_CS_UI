using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;

namespace Extentions
{
    public static class TimelineExtention
    {
        public static async Task PlayTask(this PlayableDirector timeline)
        {
            timeline.Play();
            await Task.Delay((int)(timeline.duration * 1000));
        }

        public static async Task ReversePlayTask(this PlayableDirector timeline, float speedFactor = 1f)
        {
            if (timeline == null)
                return;

            DirectorUpdateMode defaultUpdateMode = timeline.timeUpdateMode;
            timeline.timeUpdateMode = DirectorUpdateMode.Manual;

            if (timeline.time.ApproxEquals(timeline.duration) || timeline.time.ApproxEquals(0))
                timeline.time = timeline.duration;

            timeline.Evaluate();

            await Task.Yield();

            float dt = (float)timeline.duration;
            while (dt > 0)
            {
                dt -= (Time.deltaTime * speedFactor) / (float)timeline.duration;
                timeline.time = Mathf.Max(dt, 0);
                timeline.Evaluate();

                await Task.Yield();
            }

            timeline.time = 0;
            timeline.Evaluate();
            timeline.timeUpdateMode = defaultUpdateMode;
            timeline.Stop();
        }

        public static bool ApproxEquals(this double num, float other)
        {
            return Mathf.Approximately((float)num, other);
        }

        public static bool ApproxEquals(this double num, double other)
        {
            return Mathf.Approximately((float)num, (float)other);
        }
    }
}