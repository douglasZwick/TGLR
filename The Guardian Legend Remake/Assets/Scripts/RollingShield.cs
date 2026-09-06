using UnityEngine;


[RequireComponent(typeof(EventDispatcher))]
public class RollingShield : Shield
{
  protected class ActiveChange
  {
    public HealthEventData EventData { get; private set; }
    
    public ActiveChange(HealthEventData eventData)
    {
      EventData = eventData;
    }
  }


  [SerializeField][Tooltip("How fast (units/sec) Shield or HP values change")]
  protected float m_ChangeRate = 4.0f;
  protected float m_TargetEnergy;
  protected ActiveChange m_OngoingChange = null;

  protected bool Rolling => m_OngoingChange != null;
  protected bool GoingDown => m_TargetEnergy < m_Energy;
  protected bool GoingUp => m_TargetEnergy > m_Energy;
  protected bool WillBeFull => m_TargetEnergy >= m_EnergyMax;
  protected bool WillBeEmpty => m_TargetEnergy <= 0;
  protected bool Invincible => Rolling;


  void Update()
  {
    var dt = Time.deltaTime;

    if (!Rolling) return;

    if (GoingDown)
      RollDown(dt);
    else
      RollUp(dt);
  }


  protected override void ReceiveDamage(HealthEventData healthED)
  {
    if (Invincible) return;

    healthED.m_StartingEnergy = healthED.m_CurrentEnergy = m_Energy;

    BeginRollingDown(healthED);
    ED.Dispatch(Events.ShieldReceivedDamage, healthED);
    ED.Dispatch(Events.ShieldDamageStarted, healthED);

    healthED.SourceDispatch(Events.CausedShieldDamage, healthED);
  }


  protected override void ReceiveHeal(HealthEventData healthED)
  {
    healthED.m_StartingEnergy = healthED.m_CurrentEnergy = m_Energy;
    
    BeginRollingUp(healthED);
    ED.Dispatch(Events.ShieldReceivedHeal, healthED);
    ED.Dispatch(Events.ShieldHealStarted, healthED);

    // Dispatch CausedShieldHeal here if I ever create that event
  }


  void BeginRollingDown(HealthEventData healthED)
  {
    m_TargetEnergy = Mathf.Max(m_Energy - healthED.m_DamageData.m_ShieldDamageAmount, 0);
    m_OngoingChange = new ActiveChange(healthED);
  }


  void RollDown(float dt)
  {
    var delta = m_ChangeRate * dt;
    var difference = m_Energy - m_TargetEnergy;
    var healthED = m_OngoingChange.EventData;

    m_Energy -= delta;

    // If the remaining difference is within one delta, then this is the final update
    if (difference <= delta)
    {
      m_Energy = m_TargetEnergy;
      EndRollingDown();
    }

    healthED.m_CurrentEnergy = m_Energy;
    ED.Dispatch(Events.ShieldUpdate, healthED);
  }


  void BeginRollingUp(HealthEventData healthED)
  {
    m_TargetEnergy = Mathf.Min(m_Energy + healthED.m_HealData.m_ShieldHealAmount, m_EnergyMax);
    m_OngoingChange = new ActiveChange(healthED);
  }


  void RollUp(float dt)
  {
    var delta = m_ChangeRate * dt;
    var difference = m_TargetEnergy - m_Energy;
    var healthED = m_OngoingChange.EventData;

    m_Energy += delta;

    // If the remaining difference is within one delta, then this is the final update
    if (difference <= delta)
    {
      m_Energy = m_TargetEnergy;
      EndRollingUp();
    }

    healthED.m_CurrentEnergy = m_Energy;
    ED.Dispatch(Events.ShieldUpdate, healthED);
  }


  void EndRollingDown()
  {
    var healthED = m_OngoingChange.EventData;

    if (Empty)
      Deplete(healthED);

    m_OngoingChange = null;
  }


  void EndRollingUp()
  {
    var healthED = m_OngoingChange.EventData;

    if (Full)
      Fill(healthED);

    m_OngoingChange = null;
  }
}
