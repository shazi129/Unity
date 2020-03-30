using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CustomBehaviour), true)]
[CanEditMultipleObjects]
public class CustomBehaviourEditor : Editor
{
    List<string> _autoBindFields = new List<string>();

    void OnEnable()
    {
        autoBind(target as CustomBehaviour);
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }

    private void autoBind(CustomBehaviour customBehaviour)
    {
        _autoBindFields.Clear();

        FieldInfo[] fields = customBehaviour.GetType().GetFields();
        foreach (FieldInfo field in fields)
        {
            object[] attributes = field.GetCustomAttributes(true);
            foreach (object attr in attributes)
            {
                if (attr is BindAttribute)
                {
                    string bindName = ((BindAttribute)attr).getBindName();
                    if (string.IsNullOrEmpty(bindName))
                    {
                        continue;
                    }

                    Transform bindTr = null;
                    if (bindName.Equals("."))
                    {
                        bindTr = customBehaviour.transform;
                    }
                    else
                    {
                        bindTr = customBehaviour.transform.Find(bindName);
                    }

                    if (bindTr == null)
                    {
                        Debug.LogError("cannot find object:" + bindName);
                        continue;
                    }

                    Type bindType = field.FieldType;
                    if (bindType == typeof(GameObject))
                    {
                        field.SetValue(customBehaviour, bindTr.gameObject);
                        _autoBindFields.Add(field.Name);
                    }
                    else if (bindType.IsSubclassOf(typeof(Component)))
                    {
                        Component c = bindTr.GetComponent(bindType);
                        if (c != null)
                        {
                            field.SetValue(customBehaviour, c);
                            _autoBindFields.Add(field.Name);
                        }
                        else
                        {
                            Debug.LogError("cannot find component " + bindType.Name + " in bindName");
                        }
                    }
                    else
                    {
                        Debug.LogError("not suport bind type: " + bindType.Name);
                    }
                }
            }
        }
    }
}
