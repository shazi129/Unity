using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

	// Use this for initialization
	void Start () {
        IntervalTaskMgr.getInstance().update(getCurTime());


    }
	
	// Update is called once per frame
	void Update () {
        IntervalTaskMgr.getInstance().update(getCurTime());

    }

    private ulong getCurTime()
    {
        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return (ulong)(ts.TotalSeconds * 1000);
    }
}
