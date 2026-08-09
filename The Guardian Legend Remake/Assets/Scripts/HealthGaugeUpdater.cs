using UnityEngine;


[RequireComponent(typeof(EventDispatcher))]
public class HealthGaugeUpdater : MonoBehaviour
{
  public EventDispatcher ED { get; private set; }

  [SerializeField]
  EventChannel m_ShieldChannel;
  [SerializeField]
  EventChannel m_HealthChannel;


  void Awake()
  {
    ED = GetComponent<EventDispatcher>();
  }


  void OnEnable()
  {
    ED.AddListener(Events.ShieldDamageStarted, OnShieldDamageStarted);
    ED.AddListener(Events.ShieldHealStarted, OnShieldHealStarted);
    ED.AddListener(Events.ShieldUpdate, OnShieldUpdate);
    ED.AddListener(Events.HealthReceivedDamage, OnHealthReceivedDamage);
    ED.AddListener(Events.HealthReceivedHeal, OnHealthReceivedHeal);
  }


  void OnShieldDamageStarted(HealthEventData healthED)
  {
    var gaugeED = new GaugeEventData()
    {
      m_MaxValue = healthED.m_EnergyMax,
      m_StartingValue = healthED.m_StartingEnergy,
      m_CurrentValue = healthED.m_CurrentEnergy,
      m_EndingValue = healthED.m_StartingEnergy - healthED.m_IncomingShieldDamage,
    };
    m_ShieldChannel.Dispatch(Events.GaugeChangeStarted, gaugeED);
  }


  void OnShieldHealStarted(HealthEventData healthED)
  {
    var gaugeED = new GaugeEventData()
    {
      m_MaxValue = healthED.m_EnergyMax,
      m_StartingValue = healthED.m_StartingEnergy,
      m_CurrentValue = healthED.m_CurrentEnergy,
      m_EndingValue = healthED.m_StartingEnergy + healthED.m_IncomingShieldDamage,
    };
    m_ShieldChannel.Dispatch(Events.GaugeChangeStarted, gaugeED);
  }


  void OnShieldUpdate(HealthEventData healthED)
  {
    var gaugeED = new GaugeEventData()
    {
      m_MaxValue = healthED.m_EnergyMax,
      m_CurrentValue = healthED.m_CurrentEnergy,
    };
    m_ShieldChannel.Dispatch(Events.GaugeUpdate, gaugeED);
  }


  void OnHealthReceivedDamage(HealthEventData healthED)
  {
    var gaugeED = new GaugeEventData()
    {
      m_MaxValue = healthED.m_HpMax,
      m_StartingValue = healthED.m_CurrentHp,
      m_CurrentValue = healthED.m_CurrentHp,
      m_EndingValue = healthED.m_CurrentHp - healthED.m_IncomingHealthDamage,
    };
    m_HealthChannel.Dispatch(Events.GaugeValueChanged, gaugeED);
  }


  void OnHealthReceivedHeal(HealthEventData healthED)
  {
    var gaugeED = new GaugeEventData()
    {
      m_MaxValue = healthED.m_HpMax,
      m_StartingValue = healthED.m_CurrentHp,
      m_CurrentValue = healthED.m_CurrentHp,
      m_EndingValue = healthED.m_CurrentHp + healthED.m_IncomingHealthHeal,
    };
    m_HealthChannel.Dispatch(Events.GaugeUpdate, gaugeED);
  }


  void OnDisable()
  {
    ED.RemoveListener(Events.ShieldDamageStarted, OnShieldDamageStarted);
    ED.RemoveListener(Events.ShieldHealStarted, OnShieldHealStarted);
    ED.RemoveListener(Events.ShieldUpdate, OnShieldUpdate);
    ED.RemoveListener(Events.HealthReceivedDamage, OnHealthReceivedDamage);
    ED.RemoveListener(Events.HealthReceivedHeal, OnHealthReceivedHeal);
  }
}
