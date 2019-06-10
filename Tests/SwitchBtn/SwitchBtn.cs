using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchBtn : MonoBehaviour {

    [SerializeField]
    private GameObject openState;

    [SerializeField]
    private GameObject closeState;

    [SerializeField]
    private bool _isOn;

    // Use this for initialization
    void Start ()
    {
        Button self = gameObject.GetComponent<Button>();
        if (self != null)
        {
            self.onClick.AddListener(onClick);
        }

        setOpenState(getOpenState());
	}

    private void onClick()
    {
        setOpenState(!getOpenState());
    }

    public bool getOpenState()
    {
        return _isOn;
    }

    public void setOpenState(bool value)
    {
        _isOn = value;
        openState.SetActive(_isOn);
        closeState.SetActive(!_isOn);
    }
}
