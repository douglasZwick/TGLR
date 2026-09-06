using UnityEngine;


[RequireComponent(typeof(EventDispatcher))]
public class HealthGaugeUpdater : MonoBehaviour
{
  public EventDispatcher ED { get; private set; }

  [SerializeField]
  EventChannel m_ShieldChannel;
  [SerializeField]
  EventChannel m_HealthChannel;
  [SerializeField]
  float m_SetupAnimationDuration = 4;
  // TODO:
  //   I am pretty confident that this is not the best way to do this, with this animation
  //   duration business. Ultimately, the animation should probably be done as a heal that
  //   triggers at the start, but the problem is that Health isn't necessarily RollingHealth,
  //   so it can't roll up.
  //   
  //   Actually it isn't a problem after all, I'll just do it as a heal, and Health and Shield
  //   will animate differently and that's fine.


  void Awake()
  {
    ED = GetComponent<EventDispatcher>();
  }


  void OnEnable()
  {
    ED.AddListener(Events.ShieldSetup, OnShieldSetup);
    ED.AddListener(Events.HealthSetup, OnHealthSetup);
    ED.AddListener(Events.ShieldDamageStarted, OnShieldDamageStarted);
    ED.AddListener(Events.ShieldHealStarted, OnShieldHealStarted);
    ED.AddListener(Events.ShieldUpdate, OnShieldUpdate);
    ED.AddListener(Events.HealthReceivedDamage, OnHealthReceivedDamage);
    ED.AddListener(Events.HealthReceivedHeal, OnHealthReceivedHeal);
  }


  void OnShieldSetup(HealthEventData healthED)
  {
    var gaugeED = new GaugeEventData()
    {
      m_MaxValue = healthED.m_EnergyMax,
      m_AnimationDuration = m_SetupAnimationDuration,
    };
    m_ShieldChannel.Dispatch(Events.GaugeSetup, gaugeED);
  }



  void OnHealthSetup(HealthEventData healthED)
  {
    var gaugeED = new GaugeEventData()
    {
      m_MaxValue = healthED.m_HpMax,
      m_AnimationDuration = m_SetupAnimationDuration,
    };
    m_HealthChannel.Dispatch(Events.GaugeSetup, gaugeED);
  }


  void OnShieldDamageStarted(HealthEventData healthED)
  {
    var gaugeED = new GaugeEventData()
    {
      m_MaxValue = healthED.m_EnergyMax,
      m_StartingValue = healthED.m_StartingEnergy,
      m_CurrentValue = healthED.m_CurrentEnergy,
      m_EndingValue = healthED.m_StartingEnergy - healthED.m_DamageData.m_ShieldDamageAmount,
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
      m_EndingValue = healthED.m_StartingEnergy + healthED.m_HealData.m_ShieldHealAmount,
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
      m_EndingValue = healthED.m_CurrentHp - healthED.m_DamageData.m_HealthDamageAmount,
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
      m_EndingValue = healthED.m_CurrentHp + healthED.m_HealData.m_HealthHealAmount,
    };
    m_HealthChannel.Dispatch(Events.GaugeUpdate, gaugeED);
  }


  void OnDisable()
  {
    ED.RemoveListener(Events.ShieldSetup, OnShieldSetup);
    ED.RemoveListener(Events.HealthSetup, OnHealthSetup);
    ED.RemoveListener(Events.ShieldDamageStarted, OnShieldDamageStarted);
    ED.RemoveListener(Events.ShieldHealStarted, OnShieldHealStarted);
    ED.RemoveListener(Events.ShieldUpdate, OnShieldUpdate);
    ED.RemoveListener(Events.HealthReceivedDamage, OnHealthReceivedDamage);
    ED.RemoveListener(Events.HealthReceivedHeal, OnHealthReceivedHeal);
  }
}
