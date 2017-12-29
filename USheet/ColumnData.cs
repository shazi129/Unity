
using System;
using System.Collections.Generic;
using UnityEngine;


public class IColumnData
{
    public string title;

    public virtual bool isOutRange(int index) { return false; }
    public virtual int size() { return 0; }
    public virtual void insert(int index, IGridData gridData) { }
    public virtual void delete(int index) { }
    public virtual void modify(int index, IGridData gridData) { }
    public virtual IGridData getValue(int index) { return null; }
}

[Serializable]
public class ColumnData<T> : IColumnData
{
    public List<T> data;

    public T defaultValue { get; set; }

    public ColumnData(T defvalue, int count)
    {
        defaultValue = defvalue;
        data = new List<T>();
        for (int i = 0; i < count; i++)
        {
            data.Add(defaultValue);
        }
    }

    public override bool isOutRange(int index)
    {
        return index < 0 || index >= data.Count;
    }

    public override int size()
    {
        return data.Count;
    }

    public override void insert(int index, IGridData gridData)
    {
        T value = defaultValue;
        if (gridData != null)
        {
            if (gridData.dataType != typeof(T))
            {
                Debug.LogError(string.Format("Column insert error, try to insert{0} to {1}", gridData.dataType, typeof(T)));
                return;
            }

            value = (gridData as GridData<T>).data;
        }
        
        if (isOutRange(index))
            data.Add(value);
        else
            data.Insert(index, value);
    }

    public override void delete(int index)
    {
        if (index < 0 || index >= data.Count)
            index = data.Count - 1;
        data.RemoveAt(index);
    }

    public override void modify(int index, IGridData gridData)
    {
        T value = defaultValue;
        if (gridData != null)
        {
            if (gridData.dataType != typeof(T))
            {
                Debug.LogError(string.Format("Column modify error, try to use {1} to modify {0}", gridData.dataType, typeof(T)));
                return;
            }

            value = (gridData as GridData<T>).data;
        }
        else
        {
            Debug.LogError("modify error, gridData is null");
        }

        if (!isOutRange(index))
            data[index] = value;
        else
            Debug.LogError(string.Format("modify error, index{0} out of range", index));
    }

    public T at(int index)
    {
        if (!isOutRange(index))
            return data[index];
        else
            Debug.LogError(string.Format("get value error, index{0} out of range", index));

        return defaultValue;
    }

    public override IGridData getValue(int index)
    {
        T value = at(index);

        return new GridData<T>(value);
    }
}

[Serializable]
public class IntColumnData : ColumnData<int>
{
    public IntColumnData(int value, int count) : base(value, count)
    {
    }
}

[Serializable]
public class StringColumnData : ColumnData<string>
{
    public StringColumnData(string value, int count) : base(value, count)
    {
    }
}

[Serializable]
public class SpriteColumnData : ColumnData<Sprite>
{
    public SpriteColumnData(Sprite value, int count) : base(value, count)
    {
    }
}
