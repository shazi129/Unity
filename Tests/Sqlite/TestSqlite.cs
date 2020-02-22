using Mono.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

public class TestSqlite : MonoBehaviour
{
    public InputField idField;
    public Button searchBtn;
    public Text outputText;

    DBClient _db = new DBClient(Application.streamingAssetsPath + "/test_db");

    // Start is called before the first frame update
    void Start()
    {
        searchBtn.onClick.AddListener(onSearchBtnClick);

        Profiler.BeginSample("sqlite connect");
        _db.connect();
        Profiler.EndSample();
    }

    private void onSearchBtnClick()
    {
        //         int id = int.Parse(idField.text);
        // 
        //         SqliteDataReader reader = _db.search<int>("DropBox", "iId", id);
        // 
        //         Debug.Log("=======" + reader.FieldCount);
        //         for (int i = 0; i < reader.FieldCount; i++)
        //         {
        //             string fieldName = reader.GetName(i);
        //             Debug.Log(fieldName + "\t\t" + reader["iItem1"].ToString());
        //         }


        //_db.createTable("hello", new List<string>() { "id", "name" }, new List<string> { "int", "string" });
        //_db.insert("hello", new List<string>() { "id", "name" }, new List<string> { "1", "\"zhangwen\"" });

        List<string> colNames = _db.getColNames("hello");
        for (int i = 0; i < colNames.Count; i++)
        {
            Debug.Log(colNames[i]);
        }
    }
}
