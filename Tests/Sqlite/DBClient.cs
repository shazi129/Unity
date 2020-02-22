using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DBClient
{
    private SqliteConnection _connection = null;
    private string _dbPath = "";
    private List<string> _aliasList = new List<string>();

    public DBClient(string dbPath)
    {
        _dbPath = dbPath;
    }

    public void addAlias(string alias)
    {
        if (!hasAlias(alias))
        {
            _aliasList.Add(alias);
        }
    }

    public void removeAlias(string alias)
    {
        _aliasList.Remove(alias);
    }

    public bool hasAlias(string alias)
    {
        return _aliasList.IndexOf(alias) >= 0;
    }

    public string getDBPath()
    {
        return _dbPath;
    }

    public void connect()
    {
        if (string.IsNullOrEmpty(_dbPath))
        {
            Debug.LogError("connect error: db path is empty");
            return;
        }

        try
        {
            //新建数据库连接
            _connection = new SqliteConnection(@"Data Source = " + _dbPath);

            //打开数据库
            _connection.Open();

            Debug.Log("open db: " + _dbPath);
        }
        catch (Exception e)
        {
            _connection = null;
            Debug.Log(e.ToString());
        }
    }

    public SqliteDataReader search<T>(string tableName, string keyTitle, T keyValue)
    {
        string sql = "select * from " + tableName + " where " + keyTitle + "=" + keyValue + ";";

        return execute(sql);
    }

    public void createTable(string tableName, List<string> colNames, List<string> colTypes)
    {
        if (colNames.Count != colTypes.Count)
        {
            Debug.LogError("DBClient.creatTable error: colName not match colTypes");
            return;
        }

        string sql = "create table " + tableName + "(";
        for (int i = 0; i < colNames.Count; i++)
        {
            sql = sql + colNames[i] + " " + colTypes[i];
            if (i < colNames.Count - 1)
            {
                sql += ",";
            }
        }
        sql += ");";

        execute(sql);
    }

    /// <summary>
    /// 删除一个表
    /// </summary>
    /// <param name="tableName">表名</param>
    public void removeTable(string tableName)
    {
        execute("drop table " + tableName + ";");
    }

    /// <summary>
    /// 清除一个表的内容
    /// </summary>
    /// <param name="tableName">表名</param>
    public void clearTable(string tableName)
    {
        execute("delete from " + tableName + ";");
    }

    /// <summary>
    /// 插入一行
    /// </summary>
    /// <param name="table">表名</param>
    /// <param name="colNames">列名</param>
    /// <param name="colValues">值，如果是列的值是string， 必须加上引号</param>
    public void insert(string tableName, List<string> colNames, List<string> colValues)
    {
        string sql = "insert into " + tableName + "(" + join(", ", colNames) + ") values(" + join(", ", colValues) + ");";
        //Debug.Log(sql);
        execute(sql);
    }

    /// <summary>
    /// 获取一个表的所有列名
    /// </summary>
    /// <param name="tableName">表名</param>
    /// <returns></returns>
    public List<string> getColNames(string tableName)
    {
        List<string> colNames = new List<string>();

        SqliteDataReader reader = execute("PRAGMA table_info(" + tableName + ");");
        while(reader.Read())
        {
            colNames.Add(reader["name"].ToString());
        }
        return colNames;
    }

    private string join(string spliter, List<string>strs)
    {
        string ret = "";
        for (int i = 0; i < strs.Count; i++)
        {
            ret += strs[i];
            if (i < strs.Count - 1)
            {
                ret += spliter;
            }
        }
        return ret;
    }


    private SqliteDataReader execute(string sql)
    {
        try
        {
            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = sql;
            return command.ExecuteReader();
        }
        catch(Exception e)
        {
            Debug.LogError("execute sql["+sql+"] error: " + e.ToString());
            return null;
        }
    }

    public void close()
    {
        if (_connection != null)
        {
            _connection.Close();
        }
    }
}