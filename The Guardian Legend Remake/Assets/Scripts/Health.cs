using UnityEngine;


public class Health : MonoBehaviour
{
  [System.Serializable]
  public class Events
  {
    public HealthEvent ReceivedShieldDamage;
    public HealthEvent ReceivedHpDamage;
    public HealthEvent ShieldDepleted;
    public HealthEvent Died;  // AKA HpDepleted
    public HealthEvent HealRequested;
    public HealthEvent ReceivedShieldHeal;
    public HealthEvent ReceivedHpHeal;
    public HealthEvent ShieldFilled;
    public HealthEvent HpFilled;
  }


  private float m_Hp;
  [SerializeField]
  private float m_HpMax = 3;
  private float m_Shield;
  [SerializeField]
  private float m_ShieldMax = 64;

  private bool ShieldEmpty => m_Shield <= 0;
  private bool Dead => m_Hp <= 0;

  public Events m_Events;


  void Awake()
  {
    m_Shield = m_ShieldMax;
    m_Hp = m_HpMax;
  }


  public void RequestDamage(HealthEventData healthED)
  {
    if (Dead) return;
    if (healthED.m_ShieldDelta <= 0 && healthED.m_HpDelta <= 0) return;

    if (!ShieldEmpty)
    {
      ReceiveShieldDamage(healthED);

      // CONSIDER:
      //   Maybe try a system with a threshold where, if the shields fall below it, some damage gets
      //   through to HP
      return;
    }

    ReceiveHpDamage(healthED);
  }


  public void OnHealRequested(HealthEventData healthED)
  {
    ReceiveShieldHeal(healthED);
    ReceiveHpHeal(healthED);
  }


  void ReceiveShieldDamage(HealthEventData healthED)
  {
    Zbug.Log($"{name} received {healthED.m_ShieldDelta} shield damage");

    var newShield = Mathf.Max(m_Shield - healthED.m_ShieldDelta, 0);
    m_Shield = newShield;
    m_Events.ReceivedShieldDamage.Invoke(healthED);

    Zbug.Log($"    {GetStatusReport()}");
    
    if (ShieldEmpty)
      DepleteShield(healthED);
  }


  void ReceiveHpDamage(HealthEventData healthED)
  {
    Zbug.Log($"{name} received {healthED.m_HpDelta} HP damage");

    var newHp = Mathf.Max(m_Hp - healthED.m_HpDelta, 0);
    m_Hp = newHp;
    m_Events.ReceivedHpDamage.Invoke(healthED);

    Zbug.Log($"    {GetStatusReport()}");

    if (Dead)
      Die(healthED);
  }


  void DepleteShield(HealthEventData healthED)
  {
    Zbug.Log($"{name}'s shield is depleted!");

    m_Events.ShieldDepleted.Invoke(healthED);
  }


  void Die(HealthEventData healthED)
  {
    Zbug.Log($"{name} has died!");

    m_Events.Died.Invoke(healthED);
  }


  void ReceiveShieldHeal(HealthEventData healthED)
  {
    Zbug.Log($"{name} recovered {healthED.m_ShieldDelta} shield");

    var oldShield = m_Shield;
    var newShield = Mathf.Min(m_Shield + healthED.m_ShieldDelta, m_ShieldMax);
    m_Shield = newShield;
    m_Events.ReceivedShieldHeal.Invoke(healthED);

    Zbug.Log($"    {GetStatusReport()}");

    if (m_Shield >= m_ShieldMax && oldShield < m_Shield)
      FillShield(healthED);
  }


  void ReceiveHpHeal(HealthEventData healthED)
  {
    Zbug.Log($"{name} recovered {healthED.m_HpDelta} HP");

    var oldHp = m_Hp;
    var newHp = Mathf.Min(m_Hp + healthED.m_HpDelta, m_HpMax);
    m_Hp = newHp;
    m_Events.ReceivedHpHeal.Invoke(healthED);

    Zbug.Log($"    {GetStatusReport()}");

    if (m_Hp >= m_HpMax && oldHp < m_Hp)
      FillHp(healthED);
  }


  void FillShield(HealthEventData healthED)
  {
    Zbug.Log($"{name}'s shield is maxed out!");

    m_Events.ShieldFilled.Invoke(healthED);
  }


  void FillHp(HealthEventData healthED)
  {
    Zbug.Log($"{name}'s HP is maxed out!");

    m_Events.HpFilled.Invoke(healthED);
  }


  string GetStatusReport()
  {
    const string blue = "#5af";
    const string red = "#f55";
    var shieldString = $"Sh: ({m_Shield} / {m_ShieldMax})".B().Color(blue);
    var hpString = $"HP: ({m_Hp} / {m_HpMax})".B().Color(red);

    return $"{name} | {shieldString} | {hpString}";
  }
}
