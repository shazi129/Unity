
using System.Collections.Generic;
using UnityEngine;

public class DanmakuPositionData
{
    //本条弹幕在弹幕区域里的上下y值
    public int yTop;   
    public int yBottom;

    //本条弹幕的尾部相对于弹幕区域的右侧偏移
    public int endX = 0;

    //所涉及到的实体
    public DanmakuItem item = null;
}

public class DanmakuPanel : MonoBehaviour
{
    [SerializeField]
    GameObject danmakuItemPrefab;

    [SerializeField]
    RectTransform displayArea;

    //弹幕空间最右侧还没有完全展示在屏幕上的弹幕信息
    List<DanmakuPositionData> _laskDanmakuList = new List<DanmakuPositionData>();

    private void updateLastDanmakuList()
    {
        //更新位置信息， 删除已经完全在屏幕里的弹幕位置信息
        for (int i = _laskDanmakuList.Count - 1; i >= 0; i--)
        {
            if (_laskDanmakuList[i].item != null)
            {
                if (isEntirelyDisplay(_laskDanmakuList[i].item))
                {
                    _laskDanmakuList.RemoveAt(i);
                }
                else
                {
                    _laskDanmakuList[i].endX = getEndX(_laskDanmakuList[i].item);
                }
            }
            else
            {
                _laskDanmakuList.RemoveAt(i);
            }
        }

        //如果被删完了
        if (_laskDanmakuList.Count == 0)
        {
            DanmakuPositionData firstData = new DanmakuPositionData();
            firstData.yTop = getDisplayTop();
            firstData.yBottom = getDisplayBottom();
            _laskDanmakuList.Add(firstData);
        }
        //没被删完，但第一个不在最顶上
        else if (_laskDanmakuList[0].yTop < getDisplayTop())
        {
            DanmakuPositionData firstData = new DanmakuPositionData();
            firstData.yTop = getDisplayTop();
            firstData.yBottom = _laskDanmakuList[0].yTop;
            _laskDanmakuList.Insert(0, firstData);
        }

        int nextIterY2 = getDisplayBottom(); //展示空间的最下方
        int nextIterY1 = nextIterY2;
        DanmakuPositionData nextIter = null;

        //重建空白项
        for (int i = _laskDanmakuList.Count - 1; i >= 0; i--)
        {
            //有空隙
            if (_laskDanmakuList[i].yBottom > nextIterY1)
            {
                if (nextIter == null || nextIter.item != null)
                {
                    nextIter = new DanmakuPositionData();
                    nextIter.yBottom = nextIterY2;
                    nextIter.yTop = _laskDanmakuList[i].yBottom;
                    _laskDanmakuList.Insert(i + 1, nextIter);
                }
            }

            nextIterY1 = _laskDanmakuList[i].yTop;
            nextIterY2 = _laskDanmakuList[i].yBottom;
            nextIter = _laskDanmakuList[i];
        }

    }

    //为新弹幕创建一个位置信息，itemHeight：新弹幕的高度
    private DanmakuPositionData createNewDanmakuPostionData(DanmakuItem item)
    {
        int iter1 = 0;
        int iter2 = 0;

        int maxEndx = -1;
        int finalIter1 = 0;
        int finalIter2 = 0;

        int itemHeight = item.getHeight();

        while(iter1 < _laskDanmakuList.Count && iter2 < _laskDanmakuList.Count)
        {
            while (iter2 < _laskDanmakuList.Count && _laskDanmakuList[iter1].yTop - _laskDanmakuList[iter2].yBottom < itemHeight)
            {
                iter2 = iter2 + 1;
            }

            //查找结束
            if (iter2 >= _laskDanmakuList.Count)
            {
                break;
            }

            while (iter1 < iter2 && _laskDanmakuList[iter1+1].yTop - _laskDanmakuList[iter2].yBottom >= itemHeight)
            {
                iter1 = iter1 + 1;
            }

            //最大的endx
            int tempMaxEndx = 0;
            for (int i = iter1; i <= iter2; i++)
            {
                if (_laskDanmakuList[i].endX > tempMaxEndx)
                {
                    tempMaxEndx = _laskDanmakuList[i].endX;
                }
            }

            if (maxEndx < 0 || tempMaxEndx < maxEndx)
            {
                maxEndx = tempMaxEndx;
                finalIter1 = iter1;
                finalIter2 = iter2;
            }

            iter2++;
            iter1++;
        }

        //插入数据
        DanmakuPositionData data = new DanmakuPositionData();
        data.item = item;
        data.yTop = _laskDanmakuList[finalIter1].yTop;
        data.yBottom = data.yTop - item.getHeight();
        data.endX = maxEndx + item.getWidth();

        _laskDanmakuList[finalIter2].yTop = data.yBottom;

        //删掉被覆盖的部分
        if (_laskDanmakuList[finalIter2].yTop == _laskDanmakuList[finalIter2].yBottom)
        {
            _laskDanmakuList.RemoveAt(finalIter2);
        }

        iter2 = finalIter2 - 1;
        for (; iter2 >= finalIter1; iter2--)
        {
            _laskDanmakuList.RemoveAt(iter2);
        }
        _laskDanmakuList.Insert(finalIter1, data);

        return data;
    }

    private int getDisplayTop()
    {
        return (int)displayArea.rect.height;
    }

    private int getDisplayBottom()
    {
        return 0;
    }

    public void createItems(List<DanmakuItemData> itemDatas)
    {
        updateLastDanmakuList();

        for (int i = 0; i < itemDatas.Count; i++)
        {
            GameObject go = GameObject.Instantiate(danmakuItemPrefab);

            //内容
            DanmakuItem item = go.GetComponent<DanmakuItem>();
            item.setContent(itemDatas[i]);

            DanmakuPositionData positionData = createNewDanmakuPostionData(item);

            go.transform.SetParent(displayArea.transform, false);
            go.transform.localPosition = getLocalPosition(positionData);


            //移动动画
            NTween.TweenPosition animation = go.GetComponent<NTween.TweenPosition>();
            animation.from = go.transform.localPosition;

            animation.to = go.transform.localPosition;
            animation.to.x = -(displayArea.rect.width + item.getWidth()) / 2;

            //速度和时间
            float speed = 50f;
            animation.duration = (animation.from.x - animation.to.x) / speed;

            animation.PlayForward();

            //移出屏幕后消失
            animation.onFinishCallback = (tweener) =>
            {
                tweener.transform.SetParent(null, false);
                GameObject.Destroy(tweener.gameObject);
            };
        }
    }

    //从positionData中获取localposition
    public Vector2 getLocalPosition(DanmakuPositionData positionData)
    {
        int offsetX = (int)(displayArea.rect.width + positionData.item.getWidth()) / 2;
        int offsetY = -(int)(displayArea.rect.height + positionData.item.getHeight()) / 2;

        return new Vector2(positionData.endX - positionData.item.getWidth() + offsetX, positionData.yTop + offsetY);
    }

    //获取弹幕的最右侧相对于展示区域的最右侧的偏移
    private int getEndX(DanmakuItem item)
    {
        int offsetX = (int)(displayArea.rect.width + item.getWidth()) / 2;
        return (int)item.transform.localPosition.x - offsetX + item.getWidth();
    }

    //本条弹幕是否完全展示出来了
    private bool isEntirelyDisplay(DanmakuItem item)
    {
        return getEndX(item) <= 0;
    }

    //调试信息
    public string positionDataToString()
    {
        string str = "count:" + _laskDanmakuList.Count + "\n";
        for (int i = 0; i < _laskDanmakuList.Count; i++)
        {
            str += "yTop:" + _laskDanmakuList[i].yTop 
                + ", yBottom:" + _laskDanmakuList[i].yBottom 
                + ", has item:" + (_laskDanmakuList[i] != null).ToString()
                + "\n";
        }
        return str;
    }
}