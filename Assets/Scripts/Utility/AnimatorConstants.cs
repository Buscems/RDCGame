using UnityEngine;

namespace Utility
{
    public static class AnimatorConstants
    {
        public static readonly int Death = Animator.StringToHash("Death");
        public static readonly int Speed = Animator.StringToHash("speed");
        public static readonly int LowShield = Animator.StringToHash("lowShield");
        public static readonly int HighShield = Animator.StringToHash("highShield");
        public static readonly int Attack1 = Animator.StringToHash("attack");
        public static readonly int FadeIn = Animator.StringToHash("fadeIn");
        public static readonly int Walk = Animator.StringToHash("Walk");
        public static readonly int Direction = Animator.StringToHash("direction");
    }
}