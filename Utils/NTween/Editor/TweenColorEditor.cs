#if TKF_EDITOR &&(TKF_ALL_EXTEND||TKFE_NTWEEN)//TKFrame Auto Gen
//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using UnityEditor;

namespace NTween
{

    [CustomEditor(typeof(TweenColor))]
    public class TweenColorEditor : UITweenerEditor
    {
        public override void OnInspectorGUI()
        {
            GUILayout.Space(6f);
            NTweenEditorTools.SetLabelWidth(120f);

            TweenColor tw = target as TweenColor;
            GUI.changed = false;

            Color from = EditorGUILayout.ColorField("From", tw.from);
            Color to = EditorGUILayout.ColorField("To", tw.to);
            bool include_children = EditorGUILayout.Toggle("Include Children", tw.include_children);

            if (GUI.changed)
            {
                NTweenEditorTools.RegisterUndo("Tween Change", tw);
                tw.from = from;
                tw.to = to;
                tw.include_children = include_children;
                NTweenTools.SetDirty(tw);
            }

            DrawCommonProperties();
        }
    }
}
#endif //TKFrame Auto Gen
