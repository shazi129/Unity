using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskDisplayItem : MonoBehaviour {

    public Text intervalText;
    public Text frequencyText;
    public Text executeTimeText;
    public Text lastExecuteTimeText;

	// Use this for initialization
	void Start ()
    {
        initUI();
    }
	
    public int interval { get; set; }
    public int frequency { get; set; }
    public ulong executeTime { get; set; }

    void initUI()
    {
        executeTimeText.text = executeTime.ToString();
        frequencyText.text = frequency.ToString();
        intervalText.text = interval.ToString();
    }

    public void func(ulong curTime)
    {
        lastExecuteTimeText.text = executeTime.ToString();

        executeTime = curTime;
        frequency--;

        initUI();
    }
}
