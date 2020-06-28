#if TKF_ALL_EXTEND || TKFE_NTWEEN//TKFrame Auto Gen
//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

namespace NTween
{

    /// <summary>
    /// Tween the object's rotation.
    /// </summary>

    [AddComponentMenu("NTween/Tween Rotation")]
    public class TweenRotation : UITweener ,ITransformTween
    {
        public Vector3 from;
        public Vector3 to;

        public Transform target;

        public Transform cachedTransform { get { if (target == null) target = transform; return target; } }

        [System.Obsolete("Use 'value' instead")]
        public Quaternion rotation { get { return this.value; } set { this.value = value; } }

        /// <summary>
        /// Tween's current value.
        /// </summary>

        public Quaternion value { get { return cachedTransform.localRotation; } set { cachedTransform.localRotation = value; } }

        /// <summary>
        /// Tween the value.
        /// </summary>

        protected override void OnUpdate(float factor, bool isFinished)
        {
            Vector3 cur = new Vector3();
            cur.x = from.x + (to.x - from.x) * factor;
            cur.y = from.y + (to.y - from.y) * factor;
            cur.z = from.z + (to.z - from.z) * factor;

            value = Quaternion.Euler(cur);

            //value = Quaternion.Slerp(Quaternion.Euler(from), Quaternion.Euler(to), factor);
        }

        /// <summary>
        /// Start the tweening operation.
        /// </summary>

        static public TweenRotation Begin(GameObject go, float duration, Quaternion rot)
        {
            TweenRotation comp = UITweener.Begin<TweenRotation>(go, duration);
            comp.from = comp.value.eulerAngles;
            comp.to = rot.eulerAngles;

            if (duration <= 0f)
            {
                comp.Sample(1f, true);
                comp.enabled = false;
            }
            return comp;
        }

        static public TweenRotation Begin(GameObject go, float duration, Quaternion from, Quaternion to)
        {
            TweenRotation comp = UITweener.Begin<TweenRotation>(go, duration);
            comp.value = from;
            comp.from = comp.value.eulerAngles;
            comp.to = to.eulerAngles;

            if (duration <= 0f)
            {
                comp.Sample(1f, true);
                comp.enabled = false;
            }
            return comp;
        }

        [ContextMenu("Set 'From' to current value")]
        public override void SetStartToCurrentValue() { from = value.eulerAngles; }

        [ContextMenu("Set 'To' to current value")]
        public override void SetEndToCurrentValue() { to = value.eulerAngles; }

        [ContextMenu("Assume value of 'From'")]
        void SetCurrentValueToStart() { value = Quaternion.Euler(from); }

        [ContextMenu("Assume value of 'To'")]
        void SetCurrentValueToEnd() { value = Quaternion.Euler(to); }
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
#endif //TKFrame Auto Gen
