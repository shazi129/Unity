#if TKF_ALL_EXTEND || TKFE_NTWEEN//TKFrame Auto Gen
//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using UnityEngine.UI;

namespace NTween
{

    /// <summary>
    /// Tween the object's alpha.
    /// </summary>
    /// 

    [AddComponentMenu("NTween/Tween Alpha")]
    public class TweenAlpha : UITweener
    {
        [Range(0f, 1f)]
        public float from = 1f;
        [Range(0f, 1f)]
        public float to = 1f;

        public bool include_children = true;

        public bool use_canvas_group = false;

        Graphic[] _graphics;

        CanvasGroup _canvasGroup;
        CanvasGroup canvasGroup
        {
            get
            {
                if(_canvasGroup == null)
                {
                    _canvasGroup = GetComponent<CanvasGroup>();
                    if (_canvasGroup == null)
                        _canvasGroup = gameObject.AddComponent<CanvasGroup>();
                }
                return _canvasGroup;
            }
        }

        public Graphic[] graphics
        {
            get
            {
                if (_graphics == null)
                {
                    if (include_children)
                    {
                        _graphics = GetComponentsInChildren<Graphic>();
                    }
                    else
                    {
                        var g = gameObject.GetComponent<Graphic>();
                        if (g)
                            _graphics = new Graphic[] { g };
                    }
                }
                return _graphics;
            }
        }

        /// <summary>
        /// Tween's current value.
        /// </summary>

        public float value
        {
            get
            {
                if (use_canvas_group == true)
                {
                    return canvasGroup.alpha;
                }
                else
                {
                    if (graphics != null && graphics.Length > 0)
                        return graphics[0].color.a;
                    else
                        return 1f;
                }

            }
            set
            {
                if (use_canvas_group == true)
                {
                    canvasGroup.alpha = value;
                }
                else
                {
                    if (graphics != null)
                    {
                        foreach (var graphic in graphics)
                        {
                            Color c = graphic.color;
                            c.a = value;
                            graphic.color = c;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Tween the value.
        /// </summary>

        protected override void OnUpdate(float factor, bool isFinished) { value = Mathf.Lerp(from, to, factor); }

        /// <summary>
        /// Start the tweening operation.
        /// </summary>

        static public TweenAlpha Begin(GameObject go, float duration, float alpha)
        {
            TweenAlpha comp = UITweener.Begin<TweenAlpha>(go, duration);
            comp.from = comp.value;
            comp.to = alpha;

            if (duration <= 0f)
            {
                comp.Sample(1f, true);
                comp.enabled = false;
            }
            return comp;
        }

        static public TweenAlpha Begin(GameObject go, float duration, float from, float to)
        {
            TweenAlpha comp = UITweener.Begin<TweenAlpha>(go, duration);
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

        public override void SetStartToCurrentValue() { from = value; }
        public override void SetEndToCurrentValue() { to = value; }
    }
}
#endif //TKFrame Auto Gen
