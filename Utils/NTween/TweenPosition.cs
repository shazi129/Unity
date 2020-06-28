//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------

using System;
using UnityEngine;

namespace NTween
{

    /// <summary>
    /// Tween the object's position.
    /// </summary>
    /// 


    [AddComponentMenu("NTween/Tween Position")]
    public class TweenPosition : UITweener ,ITransformTween
    {
        public Vector3 from;
        public Vector3 to;
        public Transform target;
        
        //是否使用RectTransform的
        public bool anchoredPosition = false;
        

        public Transform cachedTransform { get { if (target == null) target = transform; return target; } }

        [System.Obsolete("Use 'value' instead")]
        public Vector3 position { get { return this.value; } set { this.value = value; } }

        /// <summary>
        /// Tween's current value.
        /// </summary>
        public Vector3 value
        {
            get
            {
                if (anchoredPosition)
                {
                    return ((RectTransform)cachedTransform).anchoredPosition;
                }else
                {
                    return cachedTransform.localPosition;
                }
            }
            set
            {
                if (anchoredPosition)
                {
                    ((RectTransform)cachedTransform).anchoredPosition = value;
                }
                else
                {
                    cachedTransform.localPosition = value;
                }
            }
        }

        void Awake()
        {
            
        }

        /// <summary>
        /// Tween the value.
        /// </summary>

        protected override void OnUpdate(float factor, bool isFinished) { value = from * (1f - factor) + to * factor; }

        /// <summary>
        /// Start the tweening operation.
        /// </summary>

        static public TweenPosition Begin(GameObject go, float duration, Vector3 pos, bool isAnchoredPosition = false, Method moveMethod = Method.Linear)
        {
            TweenPosition comp = UITweener.Begin<TweenPosition>(go, duration);
            comp.anchoredPosition = isAnchoredPosition;
            comp.method = moveMethod;
            comp.from = comp.value;
            comp.to = pos;

            if (duration <= 0f)
            {
                comp.Sample(1f, true);
                comp.enabled = false;
            }
            return comp;
        }

        static public TweenPosition Begin(GameObject go, float duration, Vector3 from, Vector3 to, bool isAnchoredPosition = false, Method moveMethod = Method.Linear)
        {
            TweenPosition comp = UITweener.Begin<TweenPosition>(go, duration);
            comp.anchoredPosition = isAnchoredPosition;
            comp.method = moveMethod;
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

        public Transform GetTarget()
        {
            return target;
        }

        public void SetTarget(Transform target)
        {
            this.target = target;
        }
    }
}
