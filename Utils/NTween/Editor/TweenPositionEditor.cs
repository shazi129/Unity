#if TKF_EDITOR &&(TKF_ALL_EXTEND||TKFE_NTWEEN)//TKFrame Auto Gen
//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using UnityEditor;

namespace NTween
{

    [CustomEditor(typeof(TweenPosition))]
    public class TweenPositionEditor : UITweenerEditor
    {
        public override void OnInspectorGUI()
        {
            GUILayout.Space(6f);
            NTweenEditorTools.SetLabelWidth(120f);

            TweenPosition tw = target as TweenPosition;
            GUI.changed = false;

            Object targetObj = EditorGUILayout.ObjectField("Target", tw.target, typeof(Transform), true);
            Vector3 from = EditorGUILayout.Vector3Field("From", tw.from);
            Vector3 to = EditorGUILayout.Vector3Field("To", tw.to);
           
            if (GUI.changed)
            {
                NTweenEditorTools.RegisterUndo("Tween Change", tw);
                tw.from = from;
                tw.to = to;
                tw.target = (targetObj as Transform);
                NTweenTools.SetDirty(tw);
            }
            tw.anchoredPosition = EditorGUILayout.Toggle("To", tw.anchoredPosition);
            DrawCommonProperties();
        }
    }
}
#endif //TKFrame Auto Gen
