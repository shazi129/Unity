using Mono.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

public class TestSqlite : MonoBehaviour
{
    public InputField idField;

    public Button searchBtn;

    public Text outputText;


    DBClient _db = null;


    void initDB()
    {
        string _dbFileName = "client_table_db";
        string _dbResroucePath = "DataBase/Test/";
        string _runtimeDBPath = Application.persistentDataPath + "/TestDev/";

        string runtimeDBFullName = _runtimeDBPath + _dbFileName;

        Debug.Log("runtimeDBFullName:" + runtimeDBFullName);

        if (!File.Exists(runtimeDBFullName))
        {
            if (!Directory.Exists(_runtimeDBPath))
            {
                Directory.CreateDirectory(_runtimeDBPath);
            }

            TextAsset textAsset = Resources.Load<TextAsset>(_dbResroucePath + _dbFileName);
            if (textAsset == null)
            {
                Debug.LogError("Load TextAsset error, path:" + (_dbResroucePath + _dbFileName));
                return;
            }

            File.WriteAllBytes(runtimeDBFullName, textAsset.bytes);
        }

        _db = new DBClient(@runtimeDBFullName);
    }


    // Start is called before the first frame update
    void Start()
    {

        outputText.text = "persistentDataPath: " + Application.persistentDataPath + "\n"
                          + "streamingAssetsPath: " + Application.streamingAssetsPath + "\n";

        initDB();

        searchBtn.onClick.AddListener(onSearchBtnClick);
        Profiler.BeginSample("sqlite connect");
        _db.connect();
        Profiler.EndSample();
    }

    private void onSearchBtnClick()
    {
        string output = "";
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

        Profiler.BeginSample("getColNames");
        List<string> colNames = _db.getColNames("hello");
        for (int i = 0; i < colNames.Count; i++)
        {
            output = output + colNames[i] + "\n";
        }
        Profiler.EndSample();

        outputText.text = output;
    }
}
