
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(SheetData))]
public class SheetDataEditor : Editor {

    SheetData editorData;
    List<List<IGridUI>> _allGridUIs = new List<List<IGridUI>>();

    List<Rect> _titleRects = new List<Rect>();
    List<Rect> _rowNumRects = new List<Rect>();

    AddColumnWindow _addColumnWindow = null;

    private void createSheet()
    {
        editorData.loadData();
        createGridUIs();
    }

    protected void OnEnable()
    {
        editorData = target as SheetData;
        createSheet();
    }

    protected void reloadSheet()
    {
        createSheet();
        Repaint();
    }

    public override void OnInspectorGUI()
    {
        drawTitles();
        drawContent();

        GUILayout.Label("totol row:" + editorData.rowCount + ", totol col:" + editorData.columnCount);

        #region test

        if (GUILayout.Button("Apply"))
        {
            saveData();
        }
        if (GUILayout.Button("Reload"))
        {
            reloadSheet();
        }
        if (GUILayout.Button("Add Row"))
        {
            addRow();
            reloadSheet();
        }
        if (GUILayout.Button("Delete Row"))
        {
            deleteRow();
            reloadSheet();
        }
        if (GUILayout.Button("Add Column"))
        {
            reloadSheet();
        }
        #endregion
        //base.OnInspectorGUI();
    }

    
    private void drawTitles()
    {
        EditorGUILayout.BeginHorizontal();
        drawRowNo(-1);

        if (_titleRects.Count != editorData.columnCount)
        {
            _titleRects.Clear();
            for (int i = 0; i < editorData.columnCount; i++)
            {
                _titleRects.Add(new Rect(0, 0, 0,0));
            }
        }

        for (int i = 0; i < editorData.columnCount; i++)
        {
            string titleName = editorData.titles[i];
            if (GUILayout.Button(editorData.titles[i], EditorStyles.toolbarPopup))
            {
                PopupMenu menu = new PopupMenu();
                menu.addItem("Add Column", () =>
                {
                    showAddColumnWindow();
                });
                menu.addItem("Rename", () =>
                {
                    showRenameColumnWindow(titleName);
                });
                PopupWindow.Show(_titleRects[i], menu);
            }

            if (Event.current.type == EventType.Repaint)
                _titleRects[i] = GUILayoutUtility.GetLastRect();

        }
        EditorGUILayout.EndHorizontal();
    }

    private void drawContent()
    {
        EditorGUILayout.BeginVertical();

        for (int rowIndex = 0; rowIndex < _allGridUIs.Count; rowIndex++)
        {
            
            List<IGridUI> row = _allGridUIs[rowIndex];

            EditorGUILayout.BeginHorizontal();
            drawRowNo(rowIndex);
            for (int colIndex = 0; colIndex < row.Count; colIndex++)
            {
                row[colIndex].initStyle();
                row[colIndex].draw();
            }
            EditorGUILayout.EndHorizontal();

        }

        EditorGUILayout.EndVertical();
    }

    private void createGridUIs()
    {
        _allGridUIs.Clear();

        for (int rowIndex = 0; rowIndex < editorData.rowCount; rowIndex++)
        {
            Dictionary<string, IGridData> rowData = editorData.getRow(rowIndex);
            if (rowData.Count > 0)
            {
                List<IGridUI> row = new List<IGridUI>();

                for (int colIndex = 0; colIndex < editorData.titles.Count; colIndex++)
                {
                    string titleName = editorData.titles[colIndex];
                    IGridUI grid = GridGUIManager.getInstance().createGridUI(rowData[titleName]);
                    grid.title = titleName;
                    row.Add(grid);
                }

                _allGridUIs.Add(row);
            }
        }
    }

    private void saveData()
    {
        for (int rowIndex = 0; rowIndex < _allGridUIs.Count; rowIndex++)
        {
            for (int colIndex = 0; colIndex < _allGridUIs[rowIndex].Count; colIndex++)
            {
                IGridUI gridUI = _allGridUIs[rowIndex][colIndex];
                editorData.modify(gridUI.title, rowIndex, gridUI.getData());
            }
        }
        EditorUtility.SetDirty(target);
    }

    private void drawRowNo(int number)
    {
        GUIStyle style = new GUIStyle(EditorStyles.toolbarButton);
        style.fixedWidth = 30;
        style.fixedHeight = 20;

        string strNo = "";
        if (number >= 0) strNo = number.ToString();
        if (GUILayout.Button(strNo, style))
        {
            
        }
    }

    private void addRow()
    {
        editorData.insert();
        EditorUtility.SetDirty(target);
    }

    private void deleteRow()
    {
        editorData.delete();
        EditorUtility.SetDirty(target);
    }

    private void showAddColumnWindow()
    {
        if (_addColumnWindow == null)
        {
            _addColumnWindow = new AddColumnWindow();
            
        }
        PopupWindow.Show(new Rect(0, 0, 0, 0), _addColumnWindow);
    }

    private void showRenameColumnWindow(string titleName)
    {

        RenamePopWindow dialog = new RenamePopWindow();
        dialog.renameAction = (string name) =>
        {
            editorData.modifyColumnName(titleName, name);
            EditorUtility.SetDirty(target);
            reloadSheet();
        };
        dialog.titleName = titleName;

        PopupWindow.Show(new Rect(0, 0, 0, 0), dialog);
    }
}
