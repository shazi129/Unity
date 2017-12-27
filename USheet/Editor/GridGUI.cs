﻿using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;

public class IGridUI
{
    public string title { get; set; }
    public int rowIndex { get; set; }
    public virtual void setData(IGridData data) { }
    public virtual void setGUIStyle(GUIStyle style) { }
    public virtual void draw() { }
    public virtual bool isDirty() { return false; }
}

public class GridGUI<T> : IGridUI
{
    protected GridData<T> _data;
    protected GUIStyle _style;

    public GridGUI()
    {
        _style = new GUIStyle(GUI.skin.box);
        _style.margin.left = 0;
        _style.margin.right = 0;
        _style.margin.top = 0;
        _style.margin.bottom = 0;
    }

    public override void setData(IGridData iData)
    {
        base.setData(_data);
        _data = iData as GridData<T>;
    }

    public override void draw()
    {
        EditorGUILayout.BeginVertical(_style);
        doDraw(_data.data);
        EditorGUILayout.EndVertical();
    }

    public virtual void doDraw(T data)
    {
    }
}

public class IntGridGUI : GridGUI<int>
{
    public override void doDraw(int data)
    {
        _data.data = EditorGUILayout.IntField(data);
    }
}

public class StringGridGUI : GridGUI<string>
{
    public override void doDraw(string data)
    {
        _data.data = EditorGUILayout.TextField(data);
    }
}

public class SpriteGridGUI : GridGUI<Sprite>
{
    public override void doDraw(Sprite data)
    {
        _data.data = (Sprite)EditorGUILayout.ObjectField(data, typeof(Sprite), false);
    }
}


public class GridGUIManager
{
    //单例
    private static GridGUIManager _instance = null;
    public static GridGUIManager getInstance()
    {
        if (_instance == null) _instance = new GridGUIManager();
        return _instance;
    }

    //单元格构建索引
    private Dictionary<Type, Type> _createDic = new Dictionary<Type, Type>();
    public GridGUIManager()
    {
        _createDic.Add(typeof(int), typeof(IntGridGUI));
        _createDic.Add(typeof(string), typeof(StringGridGUI));
        _createDic.Add(typeof(Sprite), typeof(SpriteGridGUI));
    }

    public IGridUI draw(IGridData iData)
    {
        IGridUI gridUI = null;
        if(_createDic.ContainsKey(iData.dataType))
        {
            Type gridType = _createDic[iData.dataType];
            gridUI = (IGridUI)Activator.CreateInstance(gridType, new object[] {});
        }

        if (gridUI != null)
        {
            gridUI.setData(iData);
            gridUI.draw();
        }

        return gridUI;
    }
}
