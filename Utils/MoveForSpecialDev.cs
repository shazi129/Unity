using UnityEngine;
using System;
using System.Collections.Generic;

public class MoveForSpecialDev : MonoBehaviour
{
    public enum E_DEV_TYPE
    {
        E_TYPE_NONE,
        E_TYPE_IPHONE_X,
    }

    [Serializable]
    private class Movement
    {
        public E_DEV_TYPE dev;
        public Vector3 offset;
    }

    [SerializeField]
    private List<Movement> movements = new List<Movement>();

    private void Awake()
    {
        E_DEV_TYPE devType = getDeviceType();
        Movement movement = null;

        for (int i = 0; i < movements.Count; i++)
        {
            if (movements[i].dev == devType)
            {
                movement = movements[i];
                break;
            }
        }
        if (movement != null)
        {
            Transform trans = gameObject.GetComponent<Transform>();
            Vector3 pos = trans.localPosition;
            pos += movement.offset;
            trans.localPosition = pos;
        }
    }

    private E_DEV_TYPE getDeviceType()
    {
        if (SystemInfo.deviceModel.Contains("iPhone10,3") || SystemInfo.deviceModel.Contains("iPhone10,6"))
        {
            return E_DEV_TYPE.E_TYPE_IPHONE_X;
        }
        return E_DEV_TYPE.E_TYPE_NONE;
    }
}
