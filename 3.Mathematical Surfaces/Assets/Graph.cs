using System.Drawing;
using UnityEngine;

public class Graph : MonoBehaviour
{
    public Transform pointPrefab;
    public Transform[] points;
    public GraphFunctionName function;
    [Range(10, 200)]
    public int resolution = 10;

    void Awake()
    {
        float step = 2.0f / resolution;
        Vector3 scale = Vector3.one * step;
        //position.y = 0f;
        //position.z = 0f;
        points = new Transform[resolution * resolution];

        for (int i=0; i < points.Length; i++)
        {
            Transform point = Instantiate(pointPrefab);//实例化方法为我们提供了对它创建的任何内容的引用
            //point.localPosition = Vector3.right*((i+0.5f)/5f-1f);//Vector3.right (1,0,0)
            //position.y = position.x * position.x;
            point.localScale = scale;
            point.SetParent(transform,false);//调用立方体的Transform组件的SetParent方法，就可以在实例化立方体之后建立父子节点关系
            points[i] = point;   
        }
    }

    static float pi = Mathf.PI;

    //水波纹
    static Vector3 Ripple(float x, float z, float t )
    {
        Vector3 p;
        float d= Mathf.Sqrt(x * x + z * z);
        p.y = Mathf.Sin(4.0f * pi * (d + t));
        p.x = x;
        p.z = z;
        p.y /= (1.0f + 10.0f * d);
        return p;
    }

    
    static Vector3 SinFunction(float x, float z, float t)
    {
        Vector3 p;
        p.x = x;
        p.z = z;
        p.y = Mathf.Sin(pi * (x + t));
        return p;
    }
    static Vector3 MultiSineFunction(float x, float z, float t)
    {
        Vector3 p;
        p = SinFunction(x, z, t);
        p.y += Mathf.Sin(2.0f * pi * (x + t)) / 2.0f;
        p *= 2.0f / 3.0f;
        return p;
    }
    
    static Vector3 Sin2DFunction(float x, float z, float t)
    {
        Vector3 p;
        p.x = x;
        p.z = z;
        p.y = Mathf.Sin(pi * (x + z + t));
        return p;
    }

    static Vector3 MultiSine2DFunction(float x, float z, float t)
    {
        Vector3 p;
        p = 4.0f * Sin2DFunction(x, z, pi * 0.5f);
        p.y += Mathf.Sin(pi * (x + t));
        p.y += Mathf.Sin(2f * pi * (z + 2.0f * t)) * 0.5f;
        p.y *= 1.0f / 5.5f;
        return p;
    }

    static Vector3 Cylinder(float u, float v, float t)
    {
        Vector3 p;
        float r = 0.8f+Mathf.Sin(pi*(6.0f*u+2.0f*v+t));
        p.x = r*Mathf.Sin(pi * u);
        p.y = v;
        p.z = r*Mathf.Cos(pi * u);
        return p;
    }

    static Vector3 Shpere(float u, float v, float t)
    {
        Vector3 p;
        float r = 0.8f + 0.1f * Mathf.Sin(pi * (6.0f * u + t))+ 0.1f * Mathf.Sin(pi * (4.0f * v + t));
        float s = r * Mathf.Cos(pi * 0.5f * v) ;
        p.x = s * Mathf.Sin(pi * u);
        p.y = r * Mathf.Sin(pi * 0.5f * v );
        p.z = s * Mathf.Cos(pi * u);
        return p;
    }

    static Graphfunction[] functions = { SinFunction, MultiSineFunction, Sin2DFunction, MultiSine2DFunction, Ripple, Cylinder , Shpere };//初始化
    private void Update()
    {
        Graphfunction f = functions[(int)function];
        float step = 2.0f / resolution; 
        float t = Time.time;
        for (int i = 0,z=0; z < resolution; z++)
        {
            float v = (z + 0.5f) * step - 1;
            for(int x=0; x < resolution; x++, i++)
            {
                float u = (x + 0.5f) * step - 1;
                points[i].localPosition = f(u, v, t);
            }
        }
    }
}
