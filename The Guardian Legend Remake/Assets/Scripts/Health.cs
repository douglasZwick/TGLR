using UnityEngine;


public class Health : MonoBehaviour
{
  [System.Serializable]
  public class Events
  {
    public HealthEvent DamageRequested;
    public HealthEvent ReceivedShieldDamage;
    public HealthEvent ReceivedHpDamage;
    public HealthEvent ShieldDepleted;
    public HealthEvent Died;  // AKA HpDepleted
    public HealthEvent HealRequested;
    public HealthEvent ReceivedShieldHeal;
    public HealthEvent ReceivedHpHeal;
    public HealthEvent ShieldRestored;
    public HealthEvent HpRestored;
  }


  private float m_Hp;
  [SerializeField]
  private float m_HpMax = 3;
  private float m_Shield;
  [SerializeField]
  private float m_ShieldMax = 64;

  public Events m_Events;


  void Awake()
  {
    m_Shield = m_ShieldMax;
    m_Hp = m_HpMax;

    m_Events.DamageRequested.AddListener(OnDamageRequested);
  }


  public void OnDamageRequested(HealthEventData healthED)
  {
    if (healthED.m_ShieldDelta <= 0 && healthED.m_HpDelta <= 0) return;

    if (m_Shield > 0)
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

    Zbug.Log($"    Shields now at {m_Shield}");
    
    if (m_Shield <= 0)
      DepleteShield(healthED);
  }


  void ReceiveHpDamage(HealthEventData healthED)
  {
    Zbug.Log($"{name} received {healthED.m_HpDelta} HP damage");

    var newHp = Mathf.Max(m_Hp - healthED.m_HpDelta, 0);
    m_Hp = newHp;
    m_Events.ReceivedHpDamage.Invoke(healthED);

    Zbug.Log($"    HP now at {m_Hp}");

    if (m_Hp <= 0)
      Die(healthED);
  }


  void DepleteShield(HealthEventData healthED)
  {
    Zbug.Log($"{name}'s shields are depleted!");

    m_Events.ShieldDepleted.Invoke(healthED);
  }


  void Die(HealthEventData healthED)
  {
    Zbug.Log($"{name} has died!");

    m_Events.Died.Invoke(healthED);
  }


  void ReceiveShieldHeal(HealthEventData healthED)
  {
    var oldShield = m_Shield;
    var newShield = Mathf.Min(m_Shield + healthED.m_ShieldDelta, m_ShieldMax);
    m_Shield = newShield;
    m_Events.ReceivedShieldHeal.Invoke(healthED);

    if (m_Shield >= m_ShieldMax && oldShield < m_Shield)
      RestoreShield(healthED);
  }


  void ReceiveHpHeal(HealthEventData healthED)
  {
    var oldHp = m_Hp;
    var newHp = Mathf.Min(m_Hp + healthED.m_HpDelta, m_HpMax);
    m_Hp = newHp;
    m_Events.ReceivedHpHeal.Invoke(healthED);

    if (m_Hp >= m_HpMax && oldHp < m_Hp)
      RestoreHp(healthED);
  }


  void RestoreShield(HealthEventData healthED)
  {
    m_Events.ShieldRestored.Invoke(healthED);
  }


  void RestoreHp(HealthEventData healthED)
  {
    m_Events.HpRestored.Invoke(healthED);
  }
}
