//六边形地图相关算法， 一些相关概念
//1. 编辑器坐标系统：
//   /
//

using System;
using System.Collections.Generic;
using UnityEngine;

public class HexagonalAlgorithm
{
    //编辑器坐标系转算法
    public static Vector2 positionToAlgorithm(Vector2 position)
    {
        return new Vector2() { x = position.x + (int)(position.y + 1) / 2, y = position.y };
    }

    //算法坐标系转编辑器坐标系
    public static Vector2 positionToEditor(Vector2 position)
    {
        return new Vector2() { x = position.x - (int)(position.y + 1) / 2, y = position.y };
    }

    //一个向量的模
    public static int nomalize(Vector2 v)
    {
        if (v.x * v.y < 0)
        {
            return (int)(Math.Abs(v.x) + Math.Abs(v.y));
        }
        else
        {
            return (int)(Math.Abs(v.x) + Math.Abs(v.y) - Math.Min(Math.Abs(v.x), Math.Abs(v.y)));
        }
    }


    public static List<Vector2> edge(Vector2 center, int r)
    {
        List<Vector2> neighors = new List<Vector2>();
        int x = (int)center.x;
        int y = (int)center.y;

        for (int i = 0; i < r; i++)
        {
            //上边界
            neighors.Add(new Vector2(x + i, y + r));
            //右上边界
            neighors.Add(new Vector2(x + r, y + r - i));
            //右下边界
            neighors.Add(new Vector2(x + r - i, y - i));
            //下边界
            neighors.Add(new Vector2(x - i, y - r));
            //左下边界
            neighors.Add(new Vector2(x - r, y - r + i));
            //右上边界
            neighors.Add(new Vector2(x - r + i, y + i));
        }
        return neighors;
    }

    public static List<Vector2> area(Vector2 center, int r)
    {
        List<Vector2> neighors = new List<Vector2>();
        neighors.Add(center);

        for (int i = 1; i <= r; i++)
        {
            neighors.AddRange(edge(center, i));
        }

        return neighors;
    }

    public static int distance(Vector2 a, Vector2 b)
    {
        Vector2 c = b - a;
        if (c.x * c.y > 0)
        {
            return (int)Math.Max(Math.Abs(c.x), Math.Abs(c.y));
        }
        else
        {
            return (int)Math.Abs(c.x) + (int)Math.Abs(c.y);
        }
    }

    public static List<Vector2> drawLine(Vector2 v1, Vector2 v2)
    {
        Debug.Log("v1["+v1.x + ", " + v1.y+"], v2[" + v2.x + ", " + v2.y + "]");
        List<Vector2> lines = new List<Vector2>();

        Vector2 d = v2 - v1;
        float steps = Math.Abs(d.x) > Math.Abs(d.y) ? Math.Abs(d.x): Math.Abs(d.y);

        if (steps > 0)
        {
            float incX = d.x / steps;
            float incY = d.y / steps;

            Debug.Log("incX:" + incX);

            for (int i = 0; i < (int)steps; i++)
            {
                Vector2 newPoint = new Vector2((int)(v1.x + (float)(i) * incX), (int)(v1.y + (float)(i) * incY));
                addLinePoint(lines, newPoint);
            }
        }

        addLinePoint(lines, v2);
        return lines;
    }

    private static void addLinePoint(List<Vector2> line, Vector2 point)
    {
        if (line.Count > 0)
        {
            Vector2 lastPoint = line[line.Count - 1];

            //二四象限跨度可能变成二，增加一个插值
            if (distance(lastPoint, point) == 2)
            {
                Debug.Log("new [" + point.x + ", " + lastPoint.y + "]");
                line.Add( new Vector2(point.x, lastPoint.y));
            }
        }

        Debug.Log("new [" + point.x + ", " + point.y + "]");
        line.Add(point);
    }

    public static bool isIntersecting(Vector2 c1, int r1, Vector2 c2, int r2)
    {
        int dc = distance(c1, c2);
        if (dc <= r1 + r2)
        {
            return true;
        }
        return false;
    }

    //获取相交区域
    private static List<Vector2> getIntersecting(Vector2 c1, int r1, Vector2 c2, int r2)
    {
        //圆心的距离
        int d = distance(c1, c2);

        //相离
        if (d > r1 + r2)
        {
            return new List<Vector2>();
        }
        //相容
        else if (Math.Abs(r1 - r1) <= d)
        {
            //哪个小返回哪个
            if (r1 < r2)
            {
                return area(c1, r1);
            }
            return area(c2, r2);
        }
        //相交
        else
        {
            List<Vector2> items = new List<Vector2>();
            return items;
        }
        
    }

    public static List<Vector2> routing(Vector2 a, Vector2 b, List<Vector2> obstacles)
    {
        return null;
    }
}