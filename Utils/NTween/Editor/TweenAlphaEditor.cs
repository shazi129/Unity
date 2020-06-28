#if TKF_EDITOR &&(TKF_ALL_EXTEND||TKFE_NTWEEN)//TKFrame Auto Gen
//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using UnityEditor;

namespace NTween
{

    [CustomEditor(typeof(TweenAlpha))]
    public class TweenAlphaEditor : UITweenerEditor
    {
        public override void OnInspectorGUI()
        {
            GUILayout.Space(6f);
            NTweenEditorTools.SetLabelWidth(120f);

            TweenAlpha tw = target as TweenAlpha;
            GUI.changed = false;

            float from = EditorGUILayout.Slider("From", tw.from, 0f, 1f);
            float to = EditorGUILayout.Slider("To", tw.to, 0f, 1f);
            bool include_children = EditorGUILayout.Toggle("Include Children", tw.include_children);
            bool use_canvas_group = EditorGUILayout.Toggle("Use Canvas Group", tw.use_canvas_group);

            if (GUI.changed)
            {
                NTweenEditorTools.RegisterUndo("Tween Change", tw);
                tw.from = from;
                tw.to = to;
                tw.include_children = include_children;
                tw.use_canvas_group = use_canvas_group;
                NTweenTools.SetDirty(tw);
            }

            DrawCommonProperties();
        }
    }
}
#endif //TKFrame Auto Gen
