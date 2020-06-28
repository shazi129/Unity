#if TKF_ALL_EXTEND || TKFE_NTWEEN//TKFrame Auto Gen
//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Spring-like motion -- the farther away the object is from the target, the stronger the pull.
/// </summary>
/// 
namespace NTween
{ 

    [AddComponentMenu("NTween/Spring Position")]
    public class SpringPosition : MonoBehaviour
    {
        static public SpringPosition current;

        /// <summary>
        /// Target position to tween to.
        /// </summary>

        public Vector3 target = Vector3.zero;

        /// <summary>
        /// Strength of the spring. The higher the value, the faster the movement.
        /// </summary>

        public float strength = 10f;

        /// <summary>
        /// Is the calculation done in world space or local space?
        /// </summary>

        public bool worldSpace = false;

        /// <summary>
        /// Whether the time scale will be ignored. Generally UI components should set it to 'true'.
        /// </summary>

        public bool ignoreTimeScale = false;

        /// <summary>
        /// Whether the parent scroll view will be updated as the object moves.
        /// </summary>

        //public bool updateScrollView = false;

        public delegate void OnFinished();

        /// <summary>
        /// Delegate to trigger when the spring finishes.
        /// </summary>

        public OnFinished onFinished;

        // Deprecated functionality
        [SerializeField]
        [HideInInspector]
        GameObject eventReceiver;
        [SerializeField]
        [HideInInspector]
        public string callWhenFinished;

        Transform mTrans;
        float mThreshold = 0f;
        //UIScrollView mSv;

        /// <summary>
        /// Cache the transform.
        /// </summary>

        void Start()
        {
            mTrans = transform;
            //if (updateScrollView) mSv = NTweenTools.FindInParents<UIScrollView>(gameObject);
        }

        /// <summary>
        /// Advance toward the target position.
        /// </summary>

        void Update()
        {
            float delta = ignoreTimeScale ? RealTime.deltaTime : Time.deltaTime;

            if (worldSpace)
            {
                if (mThreshold == 0f) mThreshold = (target - mTrans.position).magnitude * 0.001f;
                mTrans.position = SpringLerp(mTrans.position, target, strength, delta);

                if (mThreshold >= (target - mTrans.position).magnitude)
                {
                    mTrans.position = target;
                    NotifyListeners();
                    enabled = false;
                }
            }
            else
            {
                if (mThreshold == 0f) mThreshold = (target - mTrans.localPosition).magnitude * 0.001f;
                mTrans.localPosition = SpringLerp(mTrans.localPosition, target, strength, delta);

                if (mThreshold >= (target - mTrans.localPosition).magnitude)
                {
                    mTrans.localPosition = target;
                    NotifyListeners();
                    enabled = false;
                }
            }

            // Ensure that the scroll bars remain in sync
            //if (mSv != null) mSv.UpdateScrollbars(true);
        }

        static public float SpringLerp(float strength, float deltaTime)
        {
            if (deltaTime > 1f) deltaTime = 1f;
            int ms = Mathf.RoundToInt(deltaTime * 1000f);
            deltaTime = 0.001f * strength;
            float cumulative = 0f;
            for (int i = 0; i < ms; ++i) cumulative = Mathf.Lerp(cumulative, 1f, deltaTime);
            return cumulative;
        }

        static public float SpringLerp(float from, float to, float strength, float deltaTime)
        {
            if (deltaTime > 1f) deltaTime = 1f;
            int ms = Mathf.RoundToInt(deltaTime * 1000f);
            deltaTime = 0.001f * strength;
            for (int i = 0; i < ms; ++i) from = Mathf.Lerp(from, to, deltaTime);
            return from;
        }

        /// <summary>
        /// Vector2.Lerp(from, to, Time.deltaTime * strength) is not framerate-independent. This function is.
        /// </summary>

        static public Vector2 SpringLerp(Vector2 from, Vector2 to, float strength, float deltaTime)
        {
            return Vector2.Lerp(from, to, SpringLerp(strength, deltaTime));
        }

        /// <summary>
        /// Vector3.Lerp(from, to, Time.deltaTime * strength) is not framerate-independent. This function is.
        /// </summary>

        static public Vector3 SpringLerp(Vector3 from, Vector3 to, float strength, float deltaTime)
        {
            return Vector3.Lerp(from, to, SpringLerp(strength, deltaTime));
        }

        /// <summary>
        /// Quaternion.Slerp(from, to, Time.deltaTime * strength) is not framerate-independent. This function is.
        /// </summary>

        static public Quaternion SpringLerp(Quaternion from, Quaternion to, float strength, float deltaTime)
        {
            return Quaternion.Slerp(from, to, SpringLerp(strength, deltaTime));
        }

        /// <summary>
        /// Notify all finished event listeners.
        /// </summary>

        void NotifyListeners()
        {
            current = this;

            if (onFinished != null) onFinished();

            if (eventReceiver != null && !string.IsNullOrEmpty(callWhenFinished))
                eventReceiver.SendMessage(callWhenFinished, this, SendMessageOptions.DontRequireReceiver);

            current = null;
        }

        /// <summary>
        /// Start the tweening process.
        /// </summary>

        static public SpringPosition Begin(GameObject go, Vector3 pos, float strength)
        {
            SpringPosition sp = go.GetComponent<SpringPosition>();
            if (sp == null) sp = go.AddComponent<SpringPosition>();
            sp.target = pos;
            sp.strength = strength;
            sp.onFinished = null;

            if (!sp.enabled)
            {
                sp.mThreshold = 0f;
                sp.enabled = true;
            }
            return sp;
        }
    }
}
#endif //TKFrame Auto Gen
