
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheetDataSet
{
    public Type dataType;
    public Type columnType;
    public IList columnSet;

    public SheetDataSet(Type dataType, Type columnType, IList columnSet)
    {
        this.dataType = dataType;
        this.columnType = columnType;
        this.columnSet = columnSet;
    }
}

public class SheetData : ScriptableObject
{
    //需要序列化的数据
    public List<string> titles = new List<string>(); //用来确定列的顺序

    public List<IntColumnData> intColumns = new List<IntColumnData>();
    public List<StringColumnData> stringColumns = new List<StringColumnData>();
    public List<SpriteColumnData> spriteColumns = new List<SpriteColumnData>();
    //end

    //索引
    private Dictionary<string, IColumnData> _table = new Dictionary<string, IColumnData>();

    private List<SheetDataSet> dataSet = new List<SheetDataSet>();

    public SheetData()
    {
        dataSet.Clear();
        dataSet.Add(new SheetDataSet(typeof(int), typeof(IntColumnData), intColumns));
        dataSet.Add(new SheetDataSet(typeof(string), typeof(StringColumnData), stringColumns));
        dataSet.Add(new SheetDataSet(typeof(Sprite), typeof(SpriteColumnData), spriteColumns));
    }

    
    public void Awake()
    {
        Debug.Log("SheetData awake");
    }

    //将序列化的数据载入索引
    public void loadData()
    {
        _table.Clear();
        for (int typeIndex = 0; typeIndex < dataSet.Count; typeIndex++)
        {
            IList columns = dataSet[typeIndex].columnSet;
            for (int columnIndex = 0; columnIndex < columns.Count; columnIndex++)
            {
                IColumnData columnData = columns[columnIndex] as IColumnData;
                if (_table.ContainsKey(columnData.title))
                {
                    Debug.LogError("LoadData error: duplicate title:" + columnData.title);
                }
                else
                {
                    _table.Add(columnData.title, columnData);
                }
            }
        }
    }

    public void clearData()
    {
        _table.Clear();
        titles.Clear();

        for (int i = 0; i < dataSet.Count; i++)
        {
            dataSet[i].columnSet.Clear();
        }
    }

    public int columnCount { get { return _table.Count; } }

    //get rows of this sheet
    public int rowCount
    {
        get
        {
            foreach(var item in _table)
            {
                return item.Value.size();
            }
            return 0;
        }
    }

    /// <summary>
    /// add a new column
    /// </summary>
    public void insertColumn<T>(string name, T defValue, int index = -1)
    {
        if (_table.ContainsKey(name))
        {
            Debug.LogError("Add Column Error: title name exists");
            return;
        }

        //反射创建一个IColumnData
        IColumnData newColumn = null;
        for (int typeIndex = 0; typeIndex < dataSet.Count; typeIndex++)
        {
            if (typeof(T) == dataSet[typeIndex].dataType)
            {
                newColumn = (IColumnData)Activator.CreateInstance(dataSet[typeIndex].columnType, new object[] {defValue, rowCount });
                newColumn.title = name;

                _table.Add(name, newColumn);
                dataSet[typeIndex].columnSet.Add(newColumn);

                //插入title
                if (index >= 0 && index < titles.Count)
                    titles.Insert(index, name);
                else
                    titles.Add(name);

                break;
            }
        }
    }
    

    public void insert()
    {
        List<object> values = new List<object>();
        for (int i = 0; i < titles.Count; i++)
        {
            values.Add(null);
        }
        insert(titles, values);
    }

    public void insert(List<String> titles, List<object> values, int index = -1)
    {
        if (titles.Count != values.Count)
        {
            Debug.LogError("SheetData insert error, param error");
            return;
        }
        foreach (var item in _table)
        {
            if (titles.Contains(item.Key))
            {
                object value = values[titles.IndexOf(item.Key)];
                if (value == null)
                {
                    item.Value.insert(index, null);
                }
                else
                {
                    Type gridClassType = typeof(GridData<>);
                    gridClassType = gridClassType.MakeGenericType(value.GetType());
                    IGridData gridData = (IGridData)Activator.CreateInstance(gridClassType, new object[] { value });

                    item.Value.insert(index, gridData);
                }
            }
            else
            {
                item.Value.insert(index, null);
            }
        }
    }

    public Dictionary<string, IGridData> getRow(int index)
    {
        Dictionary<string, IGridData> result = new Dictionary<string, IGridData>();
        foreach (var item in _table)
        {
            result.Add(item.Key, item.Value.getValue(index));
        }
        return result;
    }

    public void delete(int index = -1)
    {
        foreach(var item in _table)
        {
            item.Value.delete(index);
        }
    }

    public void modify(string title, int rowIndex, IGridData iData)
    {
        if (_table.ContainsKey(title))
        {
            IColumnData columnData = _table[title];
            columnData.modify(rowIndex, iData);
        }
        else
        {
            Debug.LogError("Modify SheetData error: no that title:" + title);
        }
    }
}
