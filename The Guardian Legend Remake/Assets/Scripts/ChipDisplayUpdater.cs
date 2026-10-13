using UnityEngine;


[RequireComponent(typeof(EventDispatcher))]
public class ChipDisplayUpdater : MonoBehaviour
{
  public EventDispatcher ED { get; private set; }

  [SerializeField]
  EventChannel m_EventChannel;


  protected virtual void Awake()
  {
    ED = GetComponent<EventDispatcher>();
  }


  void OnEnable()
  {
    ED.AddListener(Events.GaugeSetup, OnGaugeSetup);
    ED.AddListener(Events.SpentChips, OnSpentChips);
  }


  void OnGaugeSetup(GaugeEventData eventData)
  {
    m_EventChannel.Dispatch(Events.GaugeSetup, eventData);
  }


  void OnSpentChips(SpecialWeaponEventData eventData)
  {
    var gaugeED = new GaugeEventData
    {
      m_EndingValue = eventData.m_AvailableChips,
      m_CurrentValue = eventData.m_AvailableChips,
    };
    m_EventChannel.Dispatch(Events.GaugeValueChanged, gaugeED);
  }


  void OnDisable()
  {
    ED.RemoveListener(Events.GaugeSetup, OnGaugeSetup);
    ED.RemoveListener(Events.SpentChips, OnSpentChips);
  }
}
