using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEditorDef : MonoBehaviour {

    public GameObject prefab;

    [SerializeField]
    GameObject _instance;
    public GameObject instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log("instance is null, create it");
                loadPrefab() ;
            }
            return _instance;
        }
        
    }

    // Use this for initialization
    void Start () {

        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
        loadPrefab();
    }


    [ContextMenu("LoadPrefab")]
    public void loadPrefab()
    {
        if (transform.childCount > 0 || prefab == null)
        {
            return;
        }

        _instance = GameObject.Instantiate(prefab);

        _instance.transform.SetParent(transform, false);
    }
}
