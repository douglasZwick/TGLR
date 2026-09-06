using System;
using UnityEngine;


[RequireComponent(typeof(EventDispatcher))]
public class KillOnDurabilityExhausted : MonoBehaviour
{
  public EventDispatcher ED { get; private set; }


  protected virtual void Awake()
  {
    ED = GetComponent<EventDispatcher>();
  }


  void OnEnable()
  {
    ED.AddListener(Events.DurabilityExhausted, OnDurabilityExhausted);
  }


  private void OnDurabilityExhausted(DurabilityEventData data)
  {
    // CONSIDER:
    //   It seems from my current perspective that I'll never actually need any of the data sent
    //   with this event, but I need to construct this anyway. If I ever find that I do actually
    //   need some of this data, I'll have to figure out a way to get it, but for now, I can just
    //   whip this bad boy up and send it off
    var healthED = new HealthEventData();

    ED.Dispatch(Events.Died, healthED);
  }


  void OnDisable()
  {
    ED.RemoveListener(Events.DurabilityExhausted, OnDurabilityExhausted);
  }
}
