
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(SheetData))]
public class SheetDataEditor : Editor {

    private Sprite tmp = null;

    SheetData editorData;

    List<List<IGridUI>> _allGridUIs = new List<List<IGridUI>>();

    protected void OnEnable()
    {
    }

    public override void OnInspectorGUI()
    {
        editorData = target as SheetData;
        editorData.loadData();



        drawTitles();
        drawContent();

        GUILayout.Label("totol row:" + editorData.rowCount + ", totol col:" + editorData.columnCount);

        tmp = (Sprite)EditorGUILayout.ObjectField(tmp, typeof(Sprite), true, GUILayout.Width(100));

        if (GUILayout.Button("Save Scriptable Data"))
        {
            List<string> titles = new List<string>() { "name", "age", "home", "icon"};

            editorData.insertColumn("name", "");
            editorData.insertColumn("age", 0);
            editorData.insertColumn("home", "");
            editorData.insertColumn<Sprite>("icon", null);

            editorData.insert(titles, new List<object>() {"zhangwen", 31, "guangxi", tmp});
            editorData.insert(titles, new List<object>() { "daiwenwen", 29, "henan", tmp });

            EditorUtility.SetDirty(target);
        }

        if (GUILayout.Button("Load Scriptable Data"))
        {
            for (int i = 0; i < editorData.rowCount; i++)
            {
                Dictionary<string, IGridData> row = editorData.getRow(i);
                Debug.Log("myName: " + (row["name"] as GridData<string>).data);
                Debug.Log("myLevel: " + (row["age"] as GridData<int>).data);
                Debug.Log("myIcon: " + (row["icon"] as GridData<Sprite>).data);
            }
        }

        if (GUILayout.Button("Clear Scriptable Data"))
        {
            editorData.clearData();
            EditorUtility.SetDirty(target);
        }

        base.OnInspectorGUI();
    }

    private void drawTitles()
    {
        EditorGUILayout.BeginHorizontal();
        for (int i = 0; i < editorData.columnCount; i++)
        {
            if (GUILayout.Button(editorData.titles[i], EditorStyles.toolbarButton, GUILayout.Width(100)))
            {

            }
        }
        EditorGUILayout.EndHorizontal();
    }

    private void drawContent()
    {
        EditorGUILayout.BeginVertical();

        for (int i = 0; i < editorData.rowCount; i++)
        {
            List<IGridUI> row = drawRow(i);
            if (row != null)
            {
                _allGridUIs.Add(row);
            }
        }

        EditorGUILayout.EndVertical();
    }

    private List<IGridUI> drawRow(int index)
    {
        Dictionary<string, IGridData> rowData = editorData.getRow(index);
        if (rowData.Count > 0)
        {
            List<IGridUI> row = new List<IGridUI>();

            EditorGUILayout.BeginHorizontal();
            for (int i = 0; i < editorData.titles.Count; i++)
            {
                string titleName = editorData.titles[i];
                IGridUI grid = GridGUIManager.getInstance().draw(rowData[titleName]);
                row.Add(grid);
            }
            EditorGUILayout.EndHorizontal();

            return row;
        }
        return null;        
    }
}
