#if TKF_EDITOR &&(TKF_ALL_EXTEND||TKFE_NTWEEN)//TKFrame Auto Gen
//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using UnityEditor;

namespace NTween
{

#if UNITY_3_5
[CustomEditor(typeof(UITweener))]
#else
    [CustomEditor(typeof(UITweener), true)]
#endif
    public class UITweenerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            GUILayout.Space(6f);
            NTweenEditorTools.SetLabelWidth(110f);
            base.OnInspectorGUI();
            DrawCommonProperties();
        }

        protected void DrawCommonProperties()
        {
            UITweener tw = target as UITweener;

            if (NTweenEditorTools.DrawHeader("Tweener"))
            {
                NTweenEditorTools.BeginContents();
                NTweenEditorTools.SetLabelWidth(110f);

                GUI.changed = false;

                UITweener.Style style = (UITweener.Style)EditorGUILayout.EnumPopup("Play Style", tw.style);
                AnimationCurve curve = EditorGUILayout.CurveField("Animation Curve", tw.animationCurve, GUILayout.Width(170f), GUILayout.Height(62f));
                //UITweener.Method method = (UITweener.Method)EditorGUILayout.EnumPopup("Play Method", tw.method);

                GUILayout.BeginHorizontal();
                float dur = EditorGUILayout.FloatField("Duration", tw.duration, GUILayout.Width(170f));
                GUILayout.Label("seconds");
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                int lt = EditorGUILayout.IntField("Loop Times", tw.loopTimes, GUILayout.Width(170f));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                int fd = EditorGUILayout.IntField(new GUIContent("Frame Delay", "由于Tween自身的时间计算机制，预制键实例化引起的卡顿会造成绑定在上面的tween动画在开始播放时出现跳帧的情况，基于帧数延迟启动可以改善该问题（帧延迟结束后才开始时间延迟计时）"), tw.delayFrame, GUILayout.Width(170f));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                float del = EditorGUILayout.FloatField("Time Delay", tw.delay, GUILayout.Width(170f));
                GUILayout.Label("seconds");
                GUILayout.EndHorizontal();

                int tg = EditorGUILayout.IntField("Tween Group", tw.tweenGroup, GUILayout.Width(170f));
                bool ts = EditorGUILayout.Toggle("Ignore TimeScale", tw.ignoreTimeScale);
                bool resetAtStart = EditorGUILayout.Toggle(new GUIContent("Reset At Start"), tw.resetAtStart, GUILayout.Width(170f));

                Object receiver = EditorGUILayout.ObjectField("Receiver", tw.eventReceiver, typeof(GameObject));
                string callWhenFinished = EditorGUILayout.TextField("Finished Callback", tw.callWhenFinished);

                if (GUI.changed)
                {
                    NTweenEditorTools.RegisterUndo("Tween Change", tw);
                    tw.animationCurve = curve;
                    //tw.method = method;
                    tw.style = style;
                    tw.ignoreTimeScale = ts;
                    tw.tweenGroup = tg;
                    tw.resetAtStart = resetAtStart;
                    tw.duration = dur;
                    tw.loopTimes = lt;
                    tw.delayFrame = fd;
                    tw.delay = del;
                    tw.eventReceiver = receiver as GameObject;
                    tw.callWhenFinished = callWhenFinished;
                    NTweenTools.SetDirty(tw);
                }
                NTweenEditorTools.EndContents();
            }

            NTweenEditorTools.SetLabelWidth(80f);
            //NTweenEditorTools.DrawEvents("On Finished", tw, tw.onFinished);
        }
    }
}
#endif //TKFrame Auto Gen
