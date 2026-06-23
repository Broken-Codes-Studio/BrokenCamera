//TODO: Add this to Broken Sigil
namespace Addon.BrokenCamera.Resources;

using Godot;

    /// <summary>
    /// Resource class for defining tween properties such as duration, transition type, and ease type.
    /// </summary>
    [GlobalClass, Tool]
    public partial class TweenResource : Resource
    {
        /// <summary>
        /// Duration of the tween.
        /// </summary>
        [Export]
        public float Duration { get; set; } = 1;

        /// <summary>
        /// Transition type of the tween.
        /// </summary>
        [Export]
        public Tween.TransitionType Transition { get; set; } = Tween.TransitionType.Linear;

        /// <summary>
        /// Ease type of the tween.
        /// </summary>
        [Export]
        public Tween.EaseType Ease { get; set; } = Tween.EaseType.InOut;
    }

