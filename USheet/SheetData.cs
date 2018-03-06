
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheetData : ScriptableObject
{
    //需要序列化的数据
    public List<string> titles = new List<string>(); //用来确定列的顺序

    public ColumnDataEntry columnData = new ColumnDataEntry();
    //end

    //索引
    private Dictionary<string, IColumnData> _table = new Dictionary<string, IColumnData>();

    public SheetData()
    {
        columnData.reloadDataMap();
    }

    public void Awake()
    {
        Debug.Log("SheetData awake");
    }

    //将序列化的数据载入索引
    public void loadData()
    {
        _table.Clear();

        foreach(var item in columnData.typeEntryMap)
        {
            for (int columnIndex = 0; columnIndex < item.Value.Count; columnIndex++)
            {
                IColumnData columnData = item.Value[columnIndex] as IColumnData;
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
    public void insertColumn(string name, E_DATA_TYPE dataType, int index = -1)
    {
        if (String.IsNullOrEmpty(name))
        {
            Debug.LogError("Invalid title name!");
            return;
        }

        if (_table.ContainsKey(name))
        {
            Debug.LogError("Add Column Error: title name exists");
            return;
        }

        //反射创建一个IColumnData
        Type columnType = DataTypeManager.instance.getColumnType(dataType);
        if (columnType != null)
        {
            IColumnData newColumn = (IColumnData)Activator.CreateInstance(columnType, new object[] { rowCount });
            newColumn.title = name;

            _table.Add(name, newColumn);
            columnData.typeEntryMap[dataType].Add(newColumn);

            //插入title
            if (index >= 0 && index < titles.Count)
                titles.Insert(index, name);
            else
                titles.Add(name);
        }
    }
    

    public void insert(int index = -1)
    {
        List<object> values = new List<object>();
        for (int i = 0; i < titles.Count; i++)
        {
            values.Add(null);
        }
        insert(titles, values, index);
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

    public void deleteRow(int index = -1)
    {
        foreach(var item in _table)
        {
            item.Value.deleteRow(index);
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

    public void modifyColumnName(string oldName, string newName)
    {
        int index = titles.IndexOf(oldName);
        if (index >=0 && _table.ContainsKey(oldName) && !titles.Contains(newName) && !_table.ContainsKey(newName))
        {
            titles[index] = newName;
            _table[oldName].title = newName;
        }
    }

    public void deleteColumn(string columnName)
    {
        //删除表头
        titles.Remove(columnName);

        //删除索引
        _table.Remove(columnName);

        //删除数据
        foreach (var item in columnData.typeEntryMap)
        {
            bool hasDeleted = false;

            for (int i = 0; i < item.Value.Count; i++)
            {
                IColumnData colomndata = item.Value[i] as IColumnData;
                if (colomndata != null)
                {
                    if (colomndata.title == columnName)
                    {
                        hasDeleted = true;
                        item.Value.RemoveAt(i);
                        break;
                    }
                }
            }
            if (hasDeleted)
            {
                break;
            }
        }
    }
}
