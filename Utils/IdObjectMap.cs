using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdObjectMap : MonoBehaviour
{
    [Serializable]
    private class IdObjectMapItem
    {
        public int id = 0;
        public UnityEngine.Object obj = null;
        public string desc = "";
    }

    public string myTag = "";

    [SerializeField]
    private List<IdObjectMapItem> idObjectMap = new List<IdObjectMapItem>();

    public UnityEngine.Object getObject(int id)
    {
        for (int i = 0; i < idObjectMap.Count; i++)
        {
            if (idObjectMap[i].id == id)
            {
                return idObjectMap[i].obj;
            }
        }
        return null;
    }

    public string getDesc(int id)
    {
        for (int i = 0; i < idObjectMap.Count; i++)
        {
            if (idObjectMap[i].id == id)
            {
                return idObjectMap[i].desc;
            }
        }
        return "";
    }

    public bool hasId(int id)
    {
        for (int i = 0; i < idObjectMap.Count; i++)
        {
            if (idObjectMap[i].id == id)
            {
                return true;
            }
        }
        return false;
    }

    public void clear()
    {
        idObjectMap.Clear();
    }
}