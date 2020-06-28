#if TKF_EDITOR &&(TKF_ALL_EXTEND||TKFE_NTWEEN)//TKFrame Auto Gen
//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using UnityEditor;

namespace NTween
{

    [CustomEditor(typeof(TweenScale))]
    public class TweenScaleEditor : UITweenerEditor
    {
        public override void OnInspectorGUI()
        {
            GUILayout.Space(6f);
            NTweenEditorTools.SetLabelWidth(120f);

            TweenScale tw = target as TweenScale;
            GUI.changed = false;
            Object targetObj = EditorGUILayout.ObjectField("Target", tw.target, typeof(Transform), true);
            Vector3 from = EditorGUILayout.Vector3Field("From", tw.from);
            Vector3 to = EditorGUILayout.Vector3Field("To", tw.to);
            bool table = EditorGUILayout.Toggle("Update Table", tw.updateTable);

            if (GUI.changed)
            {
                NTweenEditorTools.RegisterUndo("Tween Change", tw);
                tw.target = (Transform)targetObj;
                tw.from = from;
                tw.to = to;
                tw.updateTable = table;
                NTweenTools.SetDirty(tw);
            }

            DrawCommonProperties();
        }
    }

}
#endif //TKFrame Auto Gen
