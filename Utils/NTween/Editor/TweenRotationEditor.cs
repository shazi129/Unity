#if TKF_EDITOR &&(TKF_ALL_EXTEND||TKFE_NTWEEN)//TKFrame Auto Gen
//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using UnityEditor;

namespace NTween
{

    [CustomEditor(typeof(TweenRotation))]
    public class TweenRotationEditor : UITweenerEditor
    {
        public override void OnInspectorGUI()
        {
            GUILayout.Space(6f);
            NTweenEditorTools.SetLabelWidth(120f);

            TweenRotation tw = target as TweenRotation;
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

            DrawCommonProperties();
        }
    }
}
#endif //TKFrame Auto Gen
