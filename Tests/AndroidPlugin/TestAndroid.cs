using System;
using UnityEngine;
using UnityEngine.UI;

public class TestAndroid : MonoBehaviour {

    public Button regBtn;
    public Button unRegBtn;
    public Button clearBtn;

    public Text outText;
    public Image image;

    private AndroidJavaObject _ajo = null;

    // Use this for initialization
    void Start ()
    {
        regBtn.onClick.AddListener(onRegBtnClick);
        unRegBtn.onClick.AddListener(onUnRegBtnClick);
        clearBtn.onClick.AddListener(onClearBtnClick);

        //初始化一些数据
#if UNITY_ANDROID
        try
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject context = activity.Call<AndroidJavaObject>("getApplicationContext");

            ImageObserverChanged changedCb = new ImageObserverChanged();
            changedCb.outText = outText;
            changedCb.screenshot = image;
            changedCb.taskExecutor = this;

            _ajo = new AndroidJavaObject("com.zw.utils.ScreenShotObserverMgr", context, changedCb);
        }
        catch (Exception e)
        {
            outText.text = e.ToString();
        }
#endif
    }

    private void onClearBtnClick()
    {
        outText.text = "";
        image.sprite = null;
    }

    private void onUnRegBtnClick()
    {
#if UNITY_ANDROID
        try
        {

            if (_ajo != null)
            {
                _ajo.Call("unRegister");
            }
        }
        catch (Exception e)
        {
            outText.text = e.ToString();
        }
#endif
    }

    private void onRegBtnClick()
    {
#if UNITY_ANDROID
        try
        {

            if (_ajo != null)
            {
                _ajo.Call("register");
            }
        }
        catch(Exception e)
        {
            outText.text = e.ToString();
        }
#endif

    }

    private void Update()
    {
        TaskList taskList = TaskList.getInstance();
        if (taskList.tasks != null)
        {
            Debug.Log("execute tasks~~~~~~~~~");
            taskList.tasks();
        }
        taskList.tasks = null;
    }
}
