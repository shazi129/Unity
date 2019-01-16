using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskPanel : MonoBehaviour {

    public GameObject taskItemPrefab;

    private int taskCount = 1000;

    //只能装下24个
    private List<TaskDisplayItem> items = new List<TaskDisplayItem>();
 
	// Use this for initialization
	void Start () {
		for (int i = 0; i < taskCount; i++)
        {
            items.Add(null);
        }
        RectTransform rectTransform = transform as RectTransform;
        Vector2 sizeDelta = rectTransform.sizeDelta;
        sizeDelta.y = 102 * (taskCount / 4);
        rectTransform.sizeDelta = sizeDelta;
	}
	
	// Update is called once per frame
	void Update ()
    {
		for (int i = 0; i < items.Count; i++)
        {
            if (items[i] != null && items[i].frequency <= 2)
            {
                IntervalTaskMgr.getInstance().removeTask(items[i].func);
                GameObject.Destroy(items[i].gameObject);
                items[i] = null;
            }

            if (items[i] == null)
            {
                TaskDisplayItem newItem = createItem();
                if (newItem != null)
                {
                    newItem.gameObject.name = "TaskItem" + i;
                    items[i] = newItem;
                    newItem.transform.SetParent(transform, false);
                    newItem.transform.SetSiblingIndex(i);
                }
            }
        }
	}

    TaskDisplayItem createItem()
    {
        int interval = Random.Range(500, 2000);
        int frequency = Random.Range(10, 20);

        GameObject newObj = GameObject.Instantiate(taskItemPrefab);
        TaskDisplayItem taskItem = newObj.GetComponent<TaskDisplayItem>();
        if (taskItem != null)
        {
            taskItem.interval = interval;
            taskItem.frequency = frequency;

            IntervalTaskMgr.getInstance().addTask((uint)interval, frequency, taskItem.func);

            return taskItem;
        }

        return null;
    }

}
