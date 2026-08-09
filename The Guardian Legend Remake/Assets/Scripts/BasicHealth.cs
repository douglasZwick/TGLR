using UnityEngine;


public class BasicHealth : Health
{
  protected override void ReceiveDamage(HealthEventData healthED)
  {
    healthED.m_StartingHp = m_Hp;
    m_Hp = Mathf.Max(m_Hp - healthED.m_IncomingHealthDamage, 0);
    healthED.m_CurrentHp = m_Hp;
    ED.Dispatch(Events.HealthReceivedDamage, healthED);
    healthED.SourceDispatch(Events.CausedHealthDamage, healthED);

    if (m_Hp <= 0)
      Die(healthED);
  }


  protected override void ReceiveHeal(HealthEventData healthED)
  {
    healthED.m_StartingHp = m_Hp;
    m_Hp = Mathf.Min(m_Hp + healthED.m_IncomingHealthHeal, m_HpMax);
    healthED.m_CurrentHp = m_Hp;
    ED.Dispatch(Events.HealthReceivedHeal, healthED);
    // SourceDispatch CausedHealthHeal healthED

    if (m_Hp >= m_HpMax && healthED.m_StartingEnergy < m_Hp)
      Fill(healthED);
  }
}
