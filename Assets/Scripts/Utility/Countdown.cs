using UnityEngine;

namespace Utility
{
    public class Countdown
    {
        private float min, max, current;
        public Countdown(float min, float max)
        {
            this.min = min;
            this.max = max;
            current = Random.Range(min, max);
        }

        public Countdown(Vector2 range)
        {
            min = range.x;
            max = range.y;
            current = Random.Range(min, max);
        }

        public bool DecrementAndCheck(float deltaTime)
        {
            current -= deltaTime;
            return current <= 0;
        }

        public void Reset()
        {
            current = Random.Range(min, max);
        }
    }
}