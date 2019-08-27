//�����ε�ͼ����㷨�� һЩ��ظ���
//1. �༭������ϵͳ��
//   /
//

using System;
using System.Collections.Generic;
using UnityEngine;

public class HexagonalAlgorithm
{
    //�༭������ϵת�㷨
    public static Vector2 positionToAlgorithm(Vector2 position)
    {
        return new Vector2() { x = position.x + (int)(position.y + 1) / 2, y = position.y };
    }

    //�㷨����ϵת�༭������ϵ
    public static Vector2 positionToEditor(Vector2 position)
    {
        return new Vector2() { x = position.x - (int)(position.y + 1) / 2, y = position.y };
    }

    //һ��������ģ
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
            //�ϱ߽�
            neighors.Add(new Vector2(x + i, y + r));
            //���ϱ߽�
            neighors.Add(new Vector2(x + r, y + r - i));
            //���±߽�
            neighors.Add(new Vector2(x + r - i, y - i));
            //�±߽�
            neighors.Add(new Vector2(x - i, y - r));
            //���±߽�
            neighors.Add(new Vector2(x - r, y - r + i));
            //���ϱ߽�
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

            //�������޿�ȿ��ܱ�ɶ�������һ����ֵ
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

    //��ȡ�ཻ����
    private static List<Vector2> getIntersecting(Vector2 c1, int r1, Vector2 c2, int r2)
    {
        //Բ�ĵľ���
        int d = distance(c1, c2);

        //����
        if (d > r1 + r2)
        {
            return new List<Vector2>();
        }
        //����
        else if (Math.Abs(r1 - r1) <= d)
        {
            //�ĸ�С�����ĸ�
            if (r1 < r2)
            {
                return area(c1, r1);
            }
            return area(c2, r2);
        }
        //�ཻ
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