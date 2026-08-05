using UnityEngine;


[RequireComponent(typeof(EventDispatcher))]
public class HealthGaugeUpdater : MonoBehaviour
{
  public EventDispatcher ED { get; private set; }

  [SerializeField]
  EventChannel m_ShieldChannel;
  [SerializeField]
  EventChannel m_HpChannel;


  void Awake()
  {
    ED = GetComponent<EventDispatcher>();
  }


  void OnEnable()
  {
    ED.AddListener(Events.ShieldReceivedDamage, OnShieldReceivedDamage);
    ED.AddListener(Events.ShieldReceivedHeal, OnShieldReceivedHeal);
    ED.AddListener(Events.ShieldDamageUpdate, OnShieldUpdate);
    ED.AddListener(Events.ShieldHealUpdate, OnShieldUpdate);
    ED.AddListener(Events.HpReceivedDamage, OnHpReceivedDamage);
    ED.AddListener(Events.HpReceivedHeal, OnHpReceivedHeal);
    ED.AddListener(Events.HpDamageUpdate, OnHpUpdate);
    ED.AddListener(Events.HpHealUpdate, OnHpUpdate);
  }


  void OnShieldReceivedDamage(HealthEventData healthED)
  {
    var gaugeED = new GaugeEventData()
    {
      m_MaxValue = healthED.m_ShieldMax,
      m_StartingValue = healthED.m_CurrentShield,
      m_CurrentValue = healthED.m_CurrentShield,
      m_EndingValue = healthED.m_CurrentShield - healthED.m_ShieldDelta,
    };
    m_ShieldChannel.Dispatch(Events.GaugeChangeStarted, gaugeED);
  }


  void OnShieldReceivedHeal(HealthEventData healthED)
  {
    var gaugeED = new GaugeEventData()
    {
      m_MaxValue = healthED.m_ShieldMax,
      m_StartingValue = healthED.m_CurrentShield,
      m_CurrentValue = healthED.m_CurrentShield,
      m_EndingValue = healthED.m_CurrentShield + healthED.m_ShieldDelta,
    };
    m_ShieldChannel.Dispatch(Events.GaugeChangeStarted, gaugeED);
  }


  void OnShieldUpdate(HealthEventData healthED)
  {
    var gaugeED = new GaugeEventData()
    {
      m_MaxValue = healthED.m_ShieldMax,
      m_CurrentValue = healthED.m_CurrentShield,
    };
    m_ShieldChannel.Dispatch(Events.GaugeUpdate, gaugeED);
  }


  void OnHpReceivedDamage(HealthEventData healthED)
  {
    var gaugeED = new GaugeEventData()
    {
      m_MaxValue = healthED.m_HpMax,
      m_StartingValue = healthED.m_CurrentHp,
      m_CurrentValue = healthED.m_CurrentHp,
      m_EndingValue = healthED.m_CurrentHp - healthED.m_HpDelta,
    };
    m_HpChannel.Dispatch(Events.GaugeChangeStarted, gaugeED);
  }


  void OnHpReceivedHeal(HealthEventData healthED)
  {
    var gaugeED = new GaugeEventData()
    {
      m_MaxValue = healthED.m_HpMax,
      m_StartingValue = healthED.m_CurrentHp,
      m_CurrentValue = healthED.m_CurrentHp,
      m_EndingValue = healthED.m_CurrentHp + healthED.m_HpDelta,
    };
    m_HpChannel.Dispatch(Events.GaugeChangeStarted, gaugeED);
  }


  void OnHpUpdate(HealthEventData healthED)
  {
    var gaugeED = new GaugeEventData()
    {
      m_MaxValue = healthED.m_HpMax,
      m_CurrentValue = healthED.m_CurrentHp,
    };
    m_HpChannel.Dispatch(Events.GaugeUpdate, gaugeED);
  }


  void OnDisable()
  {
    ED.RemoveListener(Events.ShieldReceivedDamage, OnShieldReceivedDamage);
    ED.RemoveListener(Events.ShieldReceivedHeal, OnShieldReceivedHeal);
    ED.RemoveListener(Events.ShieldDamageUpdate, OnShieldUpdate);
    ED.RemoveListener(Events.ShieldHealUpdate, OnShieldUpdate);
    ED.RemoveListener(Events.HpReceivedDamage, OnHpReceivedDamage);
    ED.RemoveListener(Events.HpReceivedHeal, OnHpReceivedHeal);
    ED.RemoveListener(Events.HpDamageUpdate, OnHpUpdate);
    ED.RemoveListener(Events.HpHealUpdate, OnHpUpdate);
  }
}
