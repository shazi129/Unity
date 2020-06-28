#if TKF_ALL_EXTEND || TKFE_NTWEEN//TKFrame Auto Gen
//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

namespace NTween
{

    /// <summary>
    /// Tween the object's local scale.
    /// </summary>

    [AddComponentMenu("NTween/Tween Scale")]

    public class TweenScale : UITweener,ITransformTween
    {
        public Vector3 from = Vector3.one;
        public Vector3 to = Vector3.one;
        public bool updateTable = false;

        public Transform target;
        
        public Transform cachedTransform { get { if (target == null) target = transform; return target; } }

        public Vector3 value { get { return cachedTransform.localScale; } set { cachedTransform.localScale = value; } }

        [System.Obsolete("Use 'value' instead")]
        public Vector3 scale { get { return this.value; } set { this.value = value; } }
        public Transform GetTarget()
        {
            return target;
        }

        public void SetTarget(Transform target)
        {
            this.target = target;
        }
        /// <summary>
        /// Tween the value.
        /// </summary>

        protected override void OnUpdate(float factor, bool isFinished)
        {
            value = from * (1f - factor) + to * factor;
        }

        /// <summary>
        /// Start the tweening operation.
        /// </summary>

        static public TweenScale Begin(GameObject go, float duration, Vector3 to)
        {
            TweenScale comp = UITweener.Begin<TweenScale>(go, duration);
            comp.from = comp.value;
            comp.to = to;

            if (duration <= 0f)
            {
                comp.Sample(1f, true);
                comp.enabled = false;
            }
            return comp;
        }

        static public TweenScale Begin(GameObject go, float duration, Vector3 from, Vector3 to)
        {
            TweenScale comp = UITweener.Begin<TweenScale>(go, duration);
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
