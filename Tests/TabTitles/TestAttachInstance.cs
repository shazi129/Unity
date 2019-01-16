using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAttachInstance : MonoBehaviour {

    [SerializeField]
    private GameObject _instance;

    [SerializeField]
    private List<int> _intList;

    [ContextMenu("CreateInstance")]
    public void CreateInstance()
    {
        _instance = new GameObject();
        _instance.transform.SetParent(this.transform, false);

        _intList.Add(1);
    }
}
