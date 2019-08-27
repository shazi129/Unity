using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenJceFileBtn : MonoBehaviour {

    public bool forServer = true;

	// Use this for initialization
	void Start () {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(onClick);
	}

    private void onClick()
    {
        GenJceMain main = new GenJceMain();
        if (forServer)
        {
            main.serilizeServerJceData();
        }
        else
        {
            main.serilizeClientData();
        }
    }
}
