using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using System;

/* Author:       Running
** Time:         17.6.22
** Describtion:  
*/

public class Timer
{
    class TimeContainer
    {
        public int timerID = 0;                //ID. 标志TimeContainer唯一的。

        public float interval = 0;             //时间间隔

        public int repeat = 0;                 //重复次数

        public float currentTime = 0;          //当前的时间

        public Action<int, object[]> callback; //回调函数

        public object[] args;                  //回调函数的可变参数
    }

    /// <summary>
    /// 容器，用于储存所有的侦听。
    /// </summary>
    static List<TimeContainer> s_timeContainer = new List<TimeContainer>();

    /// <summary>
    /// 每次有侦听添加，s_timerID会加1.用于标记TimeContainer的唯一。
    /// </summary>
    static int s_timerID = 0;

    private Timer() { }

    /// <summary>
    /// 通过timerID来删除TimeContainer
    /// </summary>
    /// <param name="timerID"></param>
    /// <returns></returns>
    public static bool Delete(int timerID)
    {
        if (timerID <= 0)
            Debug.LogError("Timer.Delete() ---> timerID < 0, timerID : " + timerID);

        for (int i = 0; i < s_timeContainer.Count; i++)
        {
            if (s_timeContainer[i].timerID == timerID)
            {
                s_timeContainer.RemoveAt(i);
                return true;
            }
        }

        Debug.Log("Timer.Delete(): ---> 没有这个timerID");
        return false;
    }

    /// <summary>
    /// 延迟delay秒后调用, 仅调用一次
    /// </summary>
    public static int Add(float delay, Action<int, object[]> callback, params object[] args)
    {
        return Init(delay, 0, 1, callback, args);
    }

    /// <summary>
    /// 调用repeat次，repeat为-1，调用无限次。
    /// </summary>
    public static int Add(int repeat, Action<int, object[]> callback, params object[] args)
    {
        return Init(0, 0, repeat, callback, args);
    }

    /// <summary>
    /// 每隔interval调用一次，调用repeat次（repeat = -1, 无限次）
    /// </summary>
    public static int Add(float interval, int repeat, Action<int, object[]> callback, params object[] args)
    {
        return Init(0, interval, repeat, callback, args);
    }

    /// <summary>
    /// 延迟delay秒后, 每隔interval调用一次，调用repeat次（repeat = -1, 无限次）
    /// </summary>
    public static int Add(float delay, float interval, int repeat, Action<int, object[]> callback, params object[] args)
    {
        return Init(delay, interval, repeat, callback, args);
    }

    private static int Init(float delay, float interval, int repeat, Action<int, object[]> callback, params object[] args)
    {
        if (delay < 0 || interval < 0)
            Debug.LogError("Timer.Init(): ---> 参数有错，delay:" + delay + " interval: " + interval);

        //初始化TimeContainer，记录所有参数。并添加到容器里面.
        TimeContainer container = new TimeContainer();
        container.timerID = ++s_timerID;
        container.interval = interval;
        container.repeat = repeat;
        container.currentTime = Time.time + interval + delay;
        container.callback = callback;
        container.args = args;
        s_timeContainer.Add(container);

        return container.timerID;
    }

    /// <summary>
    /// 外部调用的Update，检查哪些容器符合条件.
    /// 不符合条件的删除。
    /// </summary>
    public static void Update()
    {
        //遍历所有的容器
        for (int i = 0; i < s_timeContainer.Count; i++)
        {
            TimeContainer timeContainer = s_timeContainer[i];

            //如果重复的次数为0，或者回调函数为空。那么清除此容器。
            if (0 == timeContainer.repeat || null == timeContainer.callback)
            {
                s_timeContainer.RemoveAt(i);
                continue;
            }

            //叠加的时间符合条件，执行回调函数。并让repeat减1.
            if (timeContainer.currentTime <= Time.time)
            {
                timeContainer.currentTime += timeContainer.interval;
                timeContainer.repeat = (timeContainer.repeat <= -1) ? -1 : --timeContainer.repeat;
                timeContainer.callback(timeContainer.timerID, timeContainer.args);
            }
        }
    }
}