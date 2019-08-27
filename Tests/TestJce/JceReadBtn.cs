using SLGMapConfig;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Wup.Jce;

public class JceReadBtn : MonoBehaviour {

    public bool forServer = true;

    // Use this for initialization
    void Start () {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(onClick);
	}

    private void onClick()
    {
        //打开文件写
        string path = Application.dataPath + "/slg_client_map.b";
        if (forServer)
        {
            path = Application.dataPath + "/slg_server_map.b";
        }
        Debug.Log("b path:" + path);

        FileStream fileStream = File.Open(path, FileMode.Open, FileAccess.Read);
        BinaryReader binaryReader = new BinaryReader(fileStream);

        byte[] bytes = binaryReader.ReadBytes((int)fileStream.Length);
        JceInputStream inputStream = new JceInputStream(bytes);

        JceStruct jceData = null;
        if (forServer)
        {
            jceData = new MapLayerConfigServer();
        }
        else
        {
            jceData = new MapLayerConfigClient();
        }
        jceData.ReadFrom(inputStream);

        binaryReader.Close();
        fileStream.Close();

        StringBuilder builder = new StringBuilder();
        jceData.Display(builder, 0);
        Debug.Log(builder.ToString());

    }
}
