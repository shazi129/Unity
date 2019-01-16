
using System;

public class TaskList
{
    public Action tasks;

    private static TaskList _instance = null;
    public static TaskList getInstance()
    {
        if (_instance == null) _instance = new TaskList();
        return _instance;
    }
}
