using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* Author:       Running
** Time:         18.11.18
** Describtion:  
*/

//事件管理
public class EventCenter
{
    private static EventCenter _instance = null;

    public static EventCenter Instance
    {
        get
        {
            return _instance ?? (_instance = new EventCenter());
        }
    }

    private class EventOwner
    {
        public event EventListener Event;

        public void OnEventTrigger(params object[] args)
        {
            if (Event != null) Event(args);
        }
    }

    //事件监听
    public delegate void EventListener(params object[] args);

    //事件列表
    private SortedList<string, EventOwner> _eventList = new SortedList<string, EventOwner>();

    /**
	 * 抛送事件
	 * 
	 */
    public void PostEvent(string regKey, params object[] args)
    {
        EventOwner owner;
        if (_eventList.TryGetValue(regKey, out owner))
        {
            owner.OnEventTrigger(args);
        }
    }

    public bool Has(string regKey)
    {
        return _eventList.ContainsKey(regKey);
    }

    /**
	 * 注册事件监听
	 */
    public void RegisterEvent(string regKey, EventListener listener)
    {
        EventOwner eventOwner;

        if (_eventList.TryGetValue(regKey, out eventOwner))
        {
            eventOwner.Event += listener;
        }
        else
        {
            eventOwner = new EventOwner();
            eventOwner.Event += listener;
            _eventList.Add(regKey, eventOwner);
        }
    }

    /**
	 * 取消注册监听
	 * 
	 */
    public void UnRegisterEvent(string regKey)
    {
        _eventList.Remove(regKey);
    }

    /**
	 * 移除监听
	 */
    public void UnRegisterEvent(string regKey, EventListener listener)
    {
        EventOwner eventOwner;

        if (_eventList.TryGetValue(regKey, out eventOwner))
        {
            eventOwner.Event -= listener;
        }
    }
}
