
using System;
using System.Collections.Generic;

/// <summary>
/// 一个简单的Schedule
/// </summary>
public class IntervalTaskMgr
{
    class Task
    {
        //间隔时间
        public uint interval = 0;

        //需要执行的次数
        public int frequency = 1;

        //任务回调，参数为时间戳
        public Action<ulong> func = null;
    }

    class TaskList
    {
        //该list的执行时间
        public ulong excuteTime { get; set; }

        //任务列表
        List<Task> _taskList = new List<Task>();

        public List<Task> value { get { return _taskList; } }

        public void addTask(uint interval, int frequency, Action<ulong> func)
        {
            if (interval > 0 && frequency > 0 && func != null)
            {
                Task task = new Task();
                task.interval = interval;
                task.frequency = frequency;
                task.func = func;
                _taskList.Add(task);
            }
        }

        public void removeTask(Action<ulong> func)
        {
            for (int i = 0; i < _taskList.Count; i++)
            {
                if (_taskList[i].func == func)
                {
                    _taskList.RemoveAt(i);
                    return;
                }
            }
        }

        public void clear()
        {
            _taskList.Clear();
        }
    }

    private static IntervalTaskMgr _instance = null;
    public static IntervalTaskMgr getInstance()
    {
        if (_instance == null) _instance = new IntervalTaskMgr();
        return _instance;
    }

    private ulong _currentTime = 0;
    private List<TaskList> _allTask = new List<TaskList>();


    public void addTask(uint interval, int frequency, Action<ulong> func)
    {
        if (_currentTime == 0)
        {
            return;
        }

        ulong executeTime = _currentTime + interval;
        TaskList taskList = null;

        for (int i = 0; i < _allTask.Count; i++)
        {
            if (_allTask[i].excuteTime == executeTime)
            {
                taskList = _allTask[i];
                break;
            }
            else if (executeTime < _allTask[i].excuteTime)
            {
                taskList = new TaskList();
                taskList.excuteTime = executeTime;
                _allTask.Insert(i, taskList);
                break;
            }
        }
        if (taskList == null)
        {
            taskList = new TaskList();
            taskList.excuteTime = executeTime;
            _allTask.Add(taskList);
        }

        taskList.addTask(interval, frequency, func);
    }

    public void removeTask(Action<ulong> func)
    {
        for (int i = 0; i < _allTask.Count; i++)
        {
            if (_allTask[i] != null)
            {
                _allTask[i].removeTask(func);
            }
        }
    }

    private void execute(TaskList taskList)
    {
        while (taskList.value.Count > 0)
        {
            Task task = taskList.value[0];
            taskList.value.RemoveAt(0);

            //执行任务
            task.func.Invoke(_currentTime);
            task.frequency--;

            //将任务重新加入队列
            if (task.interval > 0 && task.frequency > 0 && task.func != null)
            {
                addTask(task.interval, task.frequency, task.func);
            }
        }
    }

    /// <summary>
    /// 外部驱动，精度由curTime控制
    /// </summary>
    /// <param name="curTime">当前时间戳</param>
    public void update(ulong curTime)
    {
        _currentTime = curTime;

        //int size = 0;

        while(true)
        {
            if (_allTask.Count == 0)
            {
                break;
            }

            TaskList taskList = _allTask[0];
            if (taskList.excuteTime <= curTime)
            {
                //size += taskList.value.Count; 
                _allTask.RemoveAt(0);
                execute(taskList);
            }
            else
            {
                break;
            }
        }

        //Debug.Log("execute task size:" + size + ", list count:" + _allTask.Count);
    }
}
