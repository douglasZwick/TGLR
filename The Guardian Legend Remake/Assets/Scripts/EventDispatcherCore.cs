using System;
using System.Collections.Generic;


public sealed class EventDispatcherCore
{
  // Holds all the events that this dispatcher can invoke.
  // The key type is EventKey, an abstract class whose generic subclass, EventKey<TData>, is used to
  //   create keys for this dictionary.
  private readonly Dictionary<EventKey, Delegate> m_Listeners = new();


  public void AddListener<TData>(EventKey<TData> key, Action<TData> newListener)
  {
    m_Listeners.TryGetValue(key, out var anyExistingListeners);
    // Combine is used rather than += because the dictionary stores Delegates, not events
    m_Listeners[key] = Delegate.Combine(anyExistingListeners, newListener);
  }


  public void RemoveListener<TData>(EventKey<TData> key, Action<TData> listener)
  {
    if (!m_Listeners.TryGetValue(key, out var anyExistingListeners)) return;

    // See AddListener above for why Remove is used instead of -=
    var whateverIsLeft = Delegate.Remove(anyExistingListeners, listener);

    if (whateverIsLeft == null)
      m_Listeners.Remove(key);
    else
      m_Listeners[key] = whateverIsLeft;
  }


  public void Dispatch<TData>(EventKey<TData> key, TData eventData)
  {
    // CONSIDER:
    //   It's remotely possible that it may at some point possibly maybe come to pass that there's a
    //   slight chance that the minor minor tiny tiny performance overhead of the ?. here could
    //   become noticeable. If this happens, it can probably just be replaced with . anyway, since
    //   the way the system is written, removing the last listener from a delegate also removes the
    //   delegate itself, so the TryGetValue here would fail in that case, and we wouldn't get to
    //   the Invoke call.
    if (m_Listeners.TryGetValue(key, out var listeners))
      ((Action<TData>)listeners)?.Invoke(eventData);
  }


  public void Clear()
  {
    m_Listeners.Clear();
  }
}
