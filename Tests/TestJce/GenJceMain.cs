using SLGMapConfig;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Wup.Jce;

public class GenJceMain
{ 
	public void serilizeServerJceData()
    {
        //序列化服务器需要的数据
        MapLayerConfigServer jceData = new MapLayerConfigServer();

        jceData.iMAxY = getRowNum();
        jceData.iMaxX = getColNum();
        jceData.vectSlots = new List<MapSlotInfoServer>();

        for (int y = 0; y < jceData.iMAxY; y++)
        {
            for (int x = 0; x < jceData.iMaxX; x++)
            {
                MapSlotInfoServer jceTileData = new MapSlotInfoServer();

                //瓦片坐标
                jceTileData.iX = x;
                jceTileData.iY = y;

                jceTileData.iTerrainLayer = getTerrainLayerId(x, y);    //草地
                jceTileData.iTerrainObject = getTerrainObjectId(x, y); //椰子树
                jceTileData.iTerrainType = getTerrainObjectType(x, y); //树
                jceTileData.vectLogicLayers = getLayerId(x, y);
        
                jceData.vectSlots.Add(jceTileData);
            }
        }

        string path = Application.dataPath + "/slg_server_map.b";
        serilizeJceStruct(jceData, path);
    }

    public void serilizeClientData()
    {
        string path = Application.dataPath + "/slg_client_map.b";
        FileStream fileStream = File.Open(path, FileMode.Create, FileAccess.Write); ;
        BinaryWriter binaryWriter = new BinaryWriter(fileStream);

        short row = (short)getRowNum();
        short col = (short)getColNum();

        //不用jce存客户端数据，直接二进制存，体积小，能随机读取
        binaryWriter.Write(row);
        binaryWriter.Write(row);

        for (int y = 0; y < row; y++)
        {
            for (int x = 0; x < col; x++)
            {
                binaryWriter.Write((char)getTerrainLayerId(x, y));
                binaryWriter.Write((char)getTerrainObjectId(x, y));
                binaryWriter.Write((char)getTerrainObjectType(x, y));
            }
        }

        binaryWriter.Close();
        fileStream.Close();

        //         MapLayerConfigClient jceData = new MapLayerConfigClient();
        //         jceData.iMAxY = getRowNum();
        //         jceData.iMaxX = getColNum();
        //         jceData.vectSlot = new List<List<MapSlotInfoClient>>();
        // 
        //         for (int y = 0; y < jceData.iMAxY; y++)
        //         {
        //             List<MapSlotInfoClient> row = new List<MapSlotInfoClient>();
        //             for (int x = 0; x < jceData.iMaxX; x++)
        //             {
        //                 MapSlotInfoClient jceTileData = new MapSlotInfoClient();
        // 
        // 
        //                 jceTileData.iTerrainLayer = getTerrainLayerId(x, y);    //草地
        //                 jceTileData.iTerrainObject = getTerrainObjectId(x, y); //椰子树
        //                 jceTileData.iTerrainType = getTerrainObjectType(x, y); //树
        // 
        //                 row.Add(jceTileData);
        //             }
        //             jceData.vectSlot.Add(row);
        //         }
        // 
        //         serilizeJceStruct(jceData, path);
    }

    public void serilizeJceStruct(JceStruct jce, string path)
    {
        JceOutputStream outStream = new JceOutputStream();
        jce.WriteTo(outStream);
        byte[] data = outStream.toByteArray();

        //打开文件写
        Debug.Log("b path:" + path);

        FileStream fileStream = File.Open(path, FileMode.Create, FileAccess.Write); ;
        BinaryWriter binaryWriter = new BinaryWriter(fileStream);

        binaryWriter.Write(data);

        binaryWriter.Close();
        fileStream.Close();
    }


    //地图接口实现
    private int getRowNum()
    {
        return 500;
    }

    private int getColNum()
    {
        return 500;
    }

    private int getTerrainLayerId(int x, int y)
    {
        return 0;
    }

    private int getTerrainObjectType(int x, int y)
    {
        return 0;
    }

    private int getTerrainObjectId(int x, int y)
    {
        return 0;
    }

    //
    private List<int> getLayerId(int x, int y)
    {
        List<int> ret = new List<int>();
        ret.Add(1);
        ret.Add(5);
        ret.Add(9);
        return ret;
    }
}
