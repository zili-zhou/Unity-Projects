using System.Drawing;
using UnityEngine;

public class Graph: MonoBehaviour
{
    public Transform pointPrefab;
    public Transform[] points;
    [Range(10,100)]
    public int resolution=1;

    void Awake()
    {      
        float step = 2.0f / resolution;
        Vector3 scale = Vector3.one * step;
        Vector3 position;
        position.y = 0f;
        position.z = 0f;
        points = new Transform[resolution];

        for (int i = 0; i < points.Length; i++)
        {
            Transform point = Instantiate(pointPrefab);//实例化方法为我们提供了对它创建的任何内容的引用
            position.x = (i + 0.5f) *step - 1f;
            //point.localPosition = Vector3.right*((i+0.5f)/5f-1f);//Vector3.right (1,0,0)
            //position.y = position.x * position.x;
            point.localPosition = position;
            point.localScale = scale;
            point.SetParent(transform);//调用立方体的Transform组件的SetParent方法，就可以在实例化立方体之后建立父子节点关系
            points[i] = point;
        }
    }

    private void Update()
    {
        for(int i=0;i<points.Length;i++)
        {
            Transform point = points[i];
            Vector3 position= point.localPosition;
            position.y = Mathf.Sin((position.x+Time.time) * Mathf.PI);
            point.localPosition = position;

        }
    }
}
