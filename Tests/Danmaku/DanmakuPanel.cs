
using System.Collections.Generic;
using UnityEngine;

public class DanmakuPositionData
{
    //������Ļ�ڵ�Ļ�����������yֵ
    public int yTop;   
    public int yBottom;

    //������Ļ��β������ڵ�Ļ������Ҳ�ƫ��
    public int endX = 0;

    //���漰����ʵ��
    public DanmakuItem item = null;
}

public class DanmakuPanel : MonoBehaviour
{
    [SerializeField]
    GameObject danmakuItemPrefab;

    [SerializeField]
    RectTransform displayArea;

    //��Ļ�ռ����Ҳ໹û����ȫչʾ����Ļ�ϵĵ�Ļ��Ϣ
    List<DanmakuPositionData> _laskDanmakuList = new List<DanmakuPositionData>();

    private void updateLastDanmakuList()
    {
        //����λ����Ϣ�� ɾ���Ѿ���ȫ����Ļ��ĵ�Ļλ����Ϣ
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

        //�����ɾ����
        if (_laskDanmakuList.Count == 0)
        {
            DanmakuPositionData firstData = new DanmakuPositionData();
            firstData.yTop = getDisplayTop();
            firstData.yBottom = getDisplayBottom();
            _laskDanmakuList.Add(firstData);
        }
        //û��ɾ�꣬����һ���������
        else if (_laskDanmakuList[0].yTop < getDisplayTop())
        {
            DanmakuPositionData firstData = new DanmakuPositionData();
            firstData.yTop = getDisplayTop();
            firstData.yBottom = _laskDanmakuList[0].yTop;
            _laskDanmakuList.Insert(0, firstData);
        }

        int nextIterY2 = getDisplayBottom(); //չʾ�ռ�����·�
        int nextIterY1 = nextIterY2;
        DanmakuPositionData nextIter = null;

        //�ؽ��հ���
        for (int i = _laskDanmakuList.Count - 1; i >= 0; i--)
        {
            //�п�϶
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

    //Ϊ�µ�Ļ����һ��λ����Ϣ��itemHeight���µ�Ļ�ĸ߶�
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

            //���ҽ���
            if (iter2 >= _laskDanmakuList.Count)
            {
                break;
            }

            while (iter1 < iter2 && _laskDanmakuList[iter1+1].yTop - _laskDanmakuList[iter2].yBottom >= itemHeight)
            {
                iter1 = iter1 + 1;
            }

            //����endx
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

        //��������
        DanmakuPositionData data = new DanmakuPositionData();
        data.item = item;
        data.yTop = _laskDanmakuList[finalIter1].yTop;
        data.yBottom = data.yTop - item.getHeight();
        data.endX = maxEndx + item.getWidth();

        _laskDanmakuList[finalIter2].yTop = data.yBottom;

        //ɾ�������ǵĲ���
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

            //����
            DanmakuItem item = go.GetComponent<DanmakuItem>();
            item.setContent(itemDatas[i]);

            DanmakuPositionData positionData = createNewDanmakuPostionData(item);

            go.transform.SetParent(displayArea.transform, false);
            go.transform.localPosition = getLocalPosition(positionData);


            //�ƶ�����
            NTween.TweenPosition animation = go.GetComponent<NTween.TweenPosition>();
            animation.from = go.transform.localPosition;

            animation.to = go.transform.localPosition;
            animation.to.x = -(displayArea.rect.width + item.getWidth()) / 2;

            //�ٶȺ�ʱ��
            float speed = 50f;
            animation.duration = (animation.from.x - animation.to.x) / speed;

            animation.PlayForward();

            //�Ƴ���Ļ����ʧ
            animation.onFinishCallback = (tweener) =>
            {
                tweener.transform.SetParent(null, false);
                GameObject.Destroy(tweener.gameObject);
            };
        }
    }

    //��positionData�л�ȡlocalposition
    public Vector2 getLocalPosition(DanmakuPositionData positionData)
    {
        int offsetX = (int)(displayArea.rect.width + positionData.item.getWidth()) / 2;
        int offsetY = -(int)(displayArea.rect.height + positionData.item.getHeight()) / 2;

        return new Vector2(positionData.endX - positionData.item.getWidth() + offsetX, positionData.yTop + offsetY);
    }

    //��ȡ��Ļ�����Ҳ������չʾ��������Ҳ��ƫ��
    private int getEndX(DanmakuItem item)
    {
        int offsetX = (int)(displayArea.rect.width + item.getWidth()) / 2;
        return (int)item.transform.localPosition.x - offsetX + item.getWidth();
    }

    //������Ļ�Ƿ���ȫչʾ������
    private bool isEntirelyDisplay(DanmakuItem item)
    {
        return getEndX(item) <= 0;
    }

    //������Ϣ
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