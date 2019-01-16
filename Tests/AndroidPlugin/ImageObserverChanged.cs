
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ImageObserverChanged : AndroidJavaProxy
{
    public Image screenshot { get; set; }
    public Text outText { get; set; }

    public TestAndroid taskExecutor { get; set; }

    public ImageObserverChanged() : base("com.zw.utils.IImageObserverChanged")
    {

    }

    public void onChange(string path)
    {
        int currentThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;

        string log = "ImageObserverChanged::onChange:path:" + path + ", threadid:" + currentThreadID;
        Debug.Log(log);

        TaskList taskList = TaskList.getInstance();
        taskList.tasks += () =>
        {
            if (outText != null)
            {
                outText.text = path;
            }

            if (screenshot != null)
            {
                taskExecutor.StartCoroutine(LoadImage(path));
            }
        };
    }

    public void onError(int errno, string errMsg)
    {
        TaskList taskList = TaskList.getInstance();
        taskList.tasks += () =>
        {
            if (outText != null)
            {
                outText.text = errMsg;
            }
        };
    }

    IEnumerator LoadImage(string path)
    {
        double startTime = (double)Time.time;
        //请求WWW
        WWW www = new WWW("file://" + path);
        yield return www;
        if (www != null && string.IsNullOrEmpty(www.error))
        {
            //获取Texture
            Texture2D texture = www.texture;

            //创建Sprite
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            screenshot.sprite = sprite;

            startTime = (double)Time.time - startTime;
            Debug.Log("WWW加载用时:" + startTime);
        }
    }
}
