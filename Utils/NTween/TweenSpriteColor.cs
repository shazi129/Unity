#if TKF_ALL_EXTEND || TKFE_NTWEEN//TKFrame Auto Gen
//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright 漏 2011-2014 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using UnityEngine.UI;

namespace NTween
{

    /// <summary>
    /// Tween the object's color.
    /// </summary>

    [AddComponentMenu("NTween/Tween Sprite Color")]
    public class TweenSpriteColor : UITweener
    {
        public Color from = Color.white;
        public Color to = Color.white;

        public bool include_children = true;
        [SerializeField]
        GameObject _target;

        public GameObject target
        {
            get
            {
                if (_target == null) return this.gameObject;
                return _target;
            }
        }



        SpriteRenderer[] _graphics;

        public SpriteRenderer[] graphics
        {
            get
            {
                if (_graphics == null)
                {
                    if (include_children)
                    {
                        _graphics = target.GetComponentsInChildren<SpriteRenderer>();
                    }
                    else
                    {
                        var g = target.GetComponent<SpriteRenderer>();
                        if (g)
                            _graphics = new SpriteRenderer[] { g };
                    }
                }
                return _graphics;
            }
        }

        [System.Obsolete("Use 'value' instead")]
        public Color color { get { return this.value; } set { this.value = value; } }

        /// <summary>
        /// Tween's current value.
        /// </summary>

        public Color value
        {
            get
            {
                if (graphics != null && graphics.Length > 0)
                    return graphics[0].color;
                else
                    return Color.white;
            }
            set
            {
                if (graphics != null)
                {
                    foreach (var graphic in graphics)
                    {
                        graphic.color = value;
                    }
                }
            }
        }

        /// <summary>
        /// Tween the value.
        /// </summary>

        protected override void OnUpdate(float factor, bool isFinished) { value = Color.Lerp(from, to, factor); }

        /// <summary>
        /// Start the tweening operation.
        /// </summary>

        static public TweenSpriteColor Begin(GameObject go, float duration, Color color)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return null;
#endif
            TweenSpriteColor comp = UITweener.Begin<TweenSpriteColor>(go, duration);
            comp.from = comp.value;
            comp.to = color;

            if (duration <= 0f)
            {
                comp.Sample(1f, true);
                comp.enabled = false;
            }
            return comp;
        }

        static public TweenSpriteColor Begin(GameObject go, float duration, Color from, Color to)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return null;
#endif
            TweenSpriteColor comp = UITweener.Begin<TweenSpriteColor>(go, duration);
            comp.value = from;
            comp.from = comp.value;
            comp.to = to;

            if (duration <= 0f)
            {
                comp.Sample(1f, true);
                comp.enabled = false;
            }
            return comp;
        }

        [ContextMenu("Set 'From' to current value")]
        public override void SetStartToCurrentValue() { from = value; }

        [ContextMenu("Set 'To' to current value")]
        public override void SetEndToCurrentValue() { to = value; }

        [ContextMenu("Assume value of 'From'")]
        void SetCurrentValueToStart() { value = from; }

        [ContextMenu("Assume value of 'To'")]
        void SetCurrentValueToEnd() { value = to; }
    }
}
#endif //TKFrame Auto Gen
