
using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public class BindAttribute : ShowOnlyAttribute
{
    private string _bindName = "";
    public BindAttribute(string name)
    {
        _bindName = name;
    }

    public string getBindName()
    {
        return _bindName;
    }
}
