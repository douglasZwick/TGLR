using UnityEngine;


[RequireComponent(typeof(Health))]
public class HealthGaugeUpdater : MonoBehaviour
{
  [SerializeField]
  EventChannel m_ShieldDispatcher;
  [SerializeField]
  EventChannel m_HpDispatcher;


  void Awake()
  {
    var health = GetComponent<Health>();

    health.m_Events.ShieldReceivedDamage.AddListener(OnShieldReceivedDamage);
    health.m_Events.ShieldReceivedHeal.AddListener(OnShieldReceivedHeal);
    health.m_Events.ShieldDamageUpdate.AddListener(OnShieldUpdate);
    health.m_Events.ShieldHealUpdate.AddListener(OnShieldUpdate);
    health.m_Events.HpReceivedDamage.AddListener(OnHpReceivedDamage);
    health.m_Events.HpReceivedHeal.AddListener(OnHpReceivedHeal);
    health.m_Events.HpDamageUpdate.AddListener(OnHpUpdate);
    health.m_Events.HpHealUpdate.AddListener(OnHpUpdate);
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
    m_ShieldDispatcher.Dispatch(GaugeEvents.GaugeChangeStarted, gaugeED);
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
    m_ShieldDispatcher.Dispatch(GaugeEvents.GaugeChangeStarted, gaugeED);
  }


  void OnShieldUpdate(HealthEventData healthED)
  {
    var gaugeED = new GaugeEventData()
    {
      m_MaxValue = healthED.m_ShieldMax,
      m_CurrentValue = healthED.m_CurrentShield,
    };
    m_ShieldDispatcher.Dispatch(GaugeEvents.GaugeUpdate, gaugeED);
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
    m_HpDispatcher.Dispatch(GaugeEvents.GaugeChangeStarted, gaugeED);
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
    m_HpDispatcher.Dispatch(GaugeEvents.GaugeChangeStarted, gaugeED);
  }


  void OnHpUpdate(HealthEventData healthED)
  {
    var gaugeED = new GaugeEventData()
    {
      m_MaxValue = healthED.m_HpMax,
      m_CurrentValue = healthED.m_CurrentHp,
    };
    m_HpDispatcher.Dispatch(GaugeEvents.GaugeUpdate, gaugeED);
  }
}
