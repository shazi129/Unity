#if TKF_ALL_EXTEND || TKFE_NTWEEN//TKFrame Auto Gen
//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2015 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace NTween
{

    /// <summary>
    /// Very simple sprite animation. Attach to a sprite and specify a common prefix such as "idle" and it will cycle through them.
    /// </summary>

    [ExecuteInEditMode]
    [RequireComponent(typeof(Image))]
    [AddComponentMenu("NTween/Sprite Animation")]
    public class UISpriteAnimation : MonoBehaviour
    {
        /// <summary>
        /// Animation framerate.
        /// </summary>
        public int fps = 30;

        /// <summary>
        /// Set the animation to be looping or not
        /// </summary>
        public bool loop = true;


        public List<Sprite> SpriteList;

        protected Image mImage;
        protected float mDelta = 0f;
        protected int mIndex = 0;
        protected bool mActive = true;

        /// <summary>
        /// Number of frames in the animation.
        /// </summary>

        public int frames { get { return SpriteList.Count; } }


        /// <summary>
        /// Returns is the animation is still playing or not
        /// </summary>

        public bool isPlaying { get { return mActive; } }

        /// <summary>
        /// Rebuild the sprite list first thing.
        /// </summary>

        protected virtual void Start()
        {
            mImage = GetComponent<Image>();
        }

        /// <summary>
        /// Advance the sprite animation process.
        /// </summary>

        protected virtual void Update()
        {
            if (mActive && SpriteList.Count > 1 && Application.isPlaying && fps > 0)
            {
                mDelta += RealTime.deltaTime;
                float rate = 1f / fps;

                if (rate < mDelta)
                {
                    mDelta = (rate > 0f) ? mDelta - rate : 0f;

                    if (++mIndex >= SpriteList.Count)
                    {
                        mIndex = 0;
                        mActive = loop;
                    }

                    if (mActive)
                    {
                        mImage.sprite = SpriteList[mIndex];
                        //if (mSnap) mImage.MakePixelPerfect();
                    }
                }
            }
        }

        /// <summary>
        /// Rebuild the sprite list after changing the sprite name.
        /// </summary>

        //public void RebuildSpriteList ()
        //{
        //    if (mImage == null) mImage = GetComponent<Image>();
        //    mSpriteNames.Clear();

        //    if (mImage != null && mImage.atlas != null)
        //    {
        //        List<UISpriteData> sprites = mImage.atlas.spriteList;

        //        for (int i = 0, imax = sprites.Count; i < imax; ++i)
        //        {
        //            UISpriteData sprite = sprites[i];

        //            if (string.IsNullOrEmpty(mPrefix) || sprite.name.StartsWith(mPrefix))
        //            {
        //                mSpriteNames.Add(sprite.name);
        //            }
        //        }
        //        mSpriteNames.Sort();
        //    }
        //}

        /// <summary>
        /// Reset the animation to the beginning.
        /// </summary>

        public void Play() { mActive = true; }

        /// <summary>
        /// Pause the animation.
        /// </summary>

        public void Pause() { mActive = false; }

        /// <summary>
        /// Reset the animation to frame 0 and activate it.
        /// </summary>

        public void ResetToBeginning()
        {
            mActive = true;
            mIndex = 0;

            if (mImage != null && SpriteList.Count > 0)
            {
                mImage.sprite = SpriteList[mIndex];
                //mImage.spriteName = mSpriteNames[mIndex];
            }
        }
    }
}
#endif //TKFrame Auto Gen
