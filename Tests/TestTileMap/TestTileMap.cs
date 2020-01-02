using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTileMap : MonoBehaviour
{
    public Camera renderCamera;
    public GameObject zero;

    //tileMap的法向量
    public Vector3 tileMapNormal;

    private Grid _grid = null;

    //tilemap上某个点的世界坐标, 设为O点
    private Vector3 _O;
    private Vector3 _C;
    private Vector3 _n; //tilemap的法向量

    private void Start()
    {
        _grid = GetComponent<Grid>();       
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //O点使用tilemap的世界坐标
            _O = transform.position;
            Debug.Log("O: " + _O);

            //C点为摄像机
            _C = renderCamera.transform.position;
            Debug.Log("C: " + _C);

            //手动设置的法向量
            _n = tileMapNormal;
            Debug.Log("n: " + _n);

            Vector3 nearClipScreenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, renderCamera.nearClipPlane);
            Vector3 nearClipWorldPos = Camera.main.ScreenToWorldPoint(nearClipScreenPos);

            //摄像机到点击位置的射线
            Vector3 m = (nearClipWorldPos - _C).normalized;
            Debug.Log("m: " + m);

            //向量OC
            Vector3 oc = _C - _O;
            Debug.Log("oc: " + oc);

            float d = Vector3.Dot(oc, _n) / Vector3.Dot(m, _n);
            Debug.Log("d: " + d);

            Vector3 p = _C - d * m;
            Debug.Log("p: " + p);

            // 将世界坐标转换为瓦片坐标
            Vector3Int cellPos = _grid.WorldToCell(p);
            Debug.Log("cellPos:" + cellPos);
        }
    }
}
