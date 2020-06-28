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

    [AddComponentMenu("NTween/Tween Mesh Color")]
    public class TweenRenderValue : UITweener
    {
        public float from = 0;
        public float to = 0;

        public string field = "_TintColor";

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



        Renderer[] _graphics;

        public Renderer[] graphics
        {
            get
            {
                if (_graphics == null)
                {
                    if (include_children)
                    {
                        _graphics = target.GetComponentsInChildren<Renderer>();
                    }
                    else
                    {
                        var g = target.GetComponent<Renderer>();
                        if (g)
                            _graphics = new Renderer[] { g };
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
                if (graphics != null && graphics.Length > 0)
                    return GetMatValue(graphics[0].material);
                else
                    return 0;
            }
            set
            {
                if (graphics != null)
                {
                    foreach (var graphic in graphics)
                    {
                        SetMatValue(graphic.material, value);
                    }
                }
            }
        }
        protected override void Start()
        {
            base.Start();
        }
        private void OnEnable()
        {

        }
        private void OnDisable()
        {

        }
        private float GetMatValue(Material mat)
        {
            return mat.GetFloat(field);
        }
        private void SetMatValue(Material mat, float val)
        {
            mat.SetFloat(field, val);
        }
        /// <summary>
        /// Tween the value.
        /// </summary>

        protected override void OnUpdate(float factor, bool isFinished) { value = Mathf.Lerp(from, to, factor); }

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
