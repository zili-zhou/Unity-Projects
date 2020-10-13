using System;
using UnityEngine;

public class Clock : MonoBehaviour
{
    public Transform hoursTransform;
    public Transform minutesTransform;
    public Transform secondsTransform;
    //常量表示时针，分针，秒针每次旋转的角度
    const float degreesPerHour= 30f;
    const float degreesPerMinue = 6f;
    const float degreesPerSecond = 6f;

    public bool continues;
    void UpdateDiscreate()//Awake整个生命周期只执行一次，但是Update是每一帧都会执行一次
    {
        Debug.Log(DateTime.Now);//DateTime在System命名空间下的，获得系统真正时间
        DateTime time = DateTime.Now;
        hoursTransform.localRotation = Quaternion.Euler(0.0f, time.Hour * degreesPerHour, 0.0f);//Quternion.Euler就是欧拉角定义的旋转（旋转用角度表示）,localRotation是相对父亲节点的旋转
        minutesTransform.localRotation = Quaternion.Euler(0.0f, time.Minute * degreesPerMinue, 0.0f);
        secondsTransform.localRotation = Quaternion.Euler(0.0f, time.Second * degreesPerSecond, 0.0f);
    }

    void UpdateContinues()//Awake整个生命周期只执行一次，但是Update是每一帧都会执行一次
    {
        Debug.Log(DateTime.Now);//DateTime在System命名空间下的，获得系统真正时间
        TimeSpan time = DateTime.Now.TimeOfDay;
        hoursTransform.localRotation = Quaternion.Euler(0.0f, (float)time.TotalHours * degreesPerHour, 0.0f);//Quternion.Euler就是欧拉角定义的旋转（旋转用角度表示）,localRotation是相对父亲节点的旋转
        minutesTransform.localRotation = Quaternion.Euler(0.0f, (float)time.TotalMinutes * degreesPerMinue, 0.0f);
        secondsTransform.localRotation = Quaternion.Euler(0.0f, (float)time.TotalSeconds * degreesPerSecond, 0.0f);
    }


    void Update()
    {
        if(continues)
        {
            UpdateContinues();
        }
        else
        {
            UpdateDiscreate();
        }
    }
}
