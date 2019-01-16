using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class TestApplication : MonoBehaviour {

    Button selfBtn;

	// Use this for initialization
	void Start () {

        selfBtn = gameObject.GetComponent<Button>();
        if (selfBtn != null)
        {
            selfBtn.onClick.AddListener(onSelfBtnClick);
        }
	}

    private void onSelfBtnClick()
    {
        Debug.Log("Application.absoluteURL: " + Application.absoluteURL);
        Debug.Log("Application.dataPath: " + Application.dataPath);

        GetDirs(Application.dataPath);
    }

    private static void GetDirs(string dirPath)
    {
        foreach (string path in Directory.GetFiles(dirPath))
        {
            Debug.Log(path);
        }

//         if (Directory.GetDirectories(dirPath).Length > 0)  //遍历所有文件夹
//         {
//             foreach (string path in Directory.GetDirectories(dirPath))
//             {
//                 GetDirs(path, ref dirs);
//             }
//         }
    }
}
