using System;
using System.Collections;
using UnityEngine;

public class CountdownBehaviour : MonoBehaviour
{
    [SerializeField]
    private float _interval = 0.1f;

    //����ʱ�õ�Э��
    private Coroutine _coroutine = null;

    //��ʱ�����ı���ʱ���
    private long _endTime = 0;
    public void setLastTime(long lastTime)
    {
        if (lastTime > 0)
        {
            _endTime = (long)CommonFunc.getCurTime() / 1000 + lastTime;
            startCountDown();
        }
        else
        {
            stopCountDown();
        }
    }

    //����ʱ����ʱ�Ļص�
    private Action _endTimeAction = null;
    public void setEndTimeAction(Action a)
    {
        _endTimeAction = a;
    }

    //ʱ��չʾ�Ļص�
    private Action<int> _displayAction = null;
    public void setDisplayAction(Action<int> a)
    {
        _displayAction = a;
    }

    virtual public void startCountDown()
    {
        if (_coroutine == null && _endTime > 0 && this.gameObject.activeInHierarchy)
        {
            _coroutine = StartCoroutine(countdown());
        }
    }

    virtual public void stopCountDown()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        _coroutine = null;
    }

    virtual public void OnDisable()
    {
        stopCountDown();
    }

    virtual public void OnEnable()
    {
        startCountDown();
    }

    private IEnumerator countdown()
    {
        while (_endTime > 0)
        {
            long now = (long)CommonFunc.getCurTime() / 1000;
            if (now <= _endTime)
            {
                if (_displayAction != null)
                {
                    _displayAction((int)(_endTime - now));
                }
            }
            else
            {
                _endTime = 0;
                if (_endTimeAction != null)
                {
                    _endTimeAction();
                }
                break;
            }

            yield return new WaitForSeconds(_interval);
        }

        _coroutine = null;
        yield return null;
    }
}