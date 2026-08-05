using System;
using UnityEngine;


public class EventDispatcher : MonoBehaviour
{
  private readonly EventDispatcherCore m_Dispatcher = new();


  public void AddListener<TData>(EventKey<TData> key, Action<TData> newListener)
    => m_Dispatcher.AddListener(key, newListener);


  public void RemoveListener<TData>(EventKey<TData> key, Action<TData> listener)
    => m_Dispatcher.RemoveListener(key, listener);


  public void Dispatch<TData>(EventKey<TData> key, TData eventData)
    => m_Dispatcher.Dispatch(key, eventData);
}
