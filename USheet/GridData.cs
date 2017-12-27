
using System;

[Serializable]
public class IGridData
{
    public Type dataType;
}

[Serializable]
public class GridData<T> : IGridData
{
    public T data;
    public GridData(T value)
    {
        data = value;
        dataType = typeof(T);
    }
}