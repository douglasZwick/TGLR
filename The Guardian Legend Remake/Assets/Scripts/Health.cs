using UnityEngine;


[RequireComponent(typeof(EventDispatcher))]
public class Health : MonoBehaviour
{
  public EventDispatcher ED { get; private set; }

  private float m_Hp;
  [SerializeField]
  private float m_HpMax = 3;
  private float m_Shield;
  [SerializeField]
  private float m_ShieldMax = 64;
  [SerializeField][Tooltip("How fast (units/sec) Shield or HP values change")]
  private float m_ChangeRate = 4.0f;
  private float m_TargetShield;
  private float m_TargetHp;
  private bool m_ShieldChanging;
  private bool m_HpChanging;
  private HealthEventData m_CurrentHealthED;

  private bool ShieldWillBeFull => m_TargetShield >= m_ShieldMax;
  private bool ShieldFull => m_Shield >= m_ShieldMax;
  private bool ShieldWillEmpty => m_TargetShield <= 0;
  private bool ShieldEmpty => m_Shield <= 0;
  private bool HpWillBeFull => m_TargetHp >= m_HpMax;
  private bool HpFull => m_Hp >= m_HpMax;
  private bool WillDie => m_TargetHp <= 0;
  private bool Dead => m_Hp <= 0;
  private bool Invincible => m_ShieldChanging || m_HpChanging;


  void Awake()
  {
    ED = GetComponent<EventDispatcher>();

    m_Shield = m_ShieldMax;
    m_Hp = m_HpMax;
  }


  void OnEnable()
  {
    ED.AddListener(Events.DamageRequest, OnDamageRequest);
  }


  void Update()
  {
    var dt = Time.deltaTime;

    if (m_ShieldChanging)
      UpdateShield(dt);
    if (m_HpChanging)
      UpdateHp(dt);
  }


  public void OnDamageRequest(HealthEventData healthED)
  {
    if (Dead) return;
    if (Invincible) return;
    if (healthED.m_ShieldDelta <= 0 && healthED.m_HpDelta <= 0) return;

    healthED.m_HpMax = m_HpMax;
    healthED.m_ShieldMax = m_ShieldMax;

    m_CurrentHealthED = healthED;

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


  public void OnHealRequest(HealthEventData healthED)
  {
    if (Dead) return;
    if (healthED.m_ShieldDelta <= 0 && healthED.m_HpDelta <= 0) return;
    
    ReceiveShieldHeal(healthED);
    ReceiveHpHeal(healthED);

    m_CurrentHealthED = healthED;
  }


  void ReceiveShieldDamage(HealthEventData healthED)
  {
    Zbug.Log($"{name} received {healthED.m_ShieldDelta} shield damage");

    var newShield = Mathf.Max(m_Shield - healthED.m_ShieldDelta, 0);
    m_TargetShield = newShield;
    m_ShieldChanging = true;
    healthED.m_CurrentShield = m_Shield;
    healthED.m_CurrentHp = m_Hp;
    ED.Dispatch(Events.ShieldReceivedDamage, healthED);

    if (ShieldWillEmpty)
      ED.Dispatch(Events.ShieldReceivedTerminalDamage, healthED);

    Zbug.Log($"    {GetStatusReport()}");
  }


  void ReceiveHpDamage(HealthEventData healthED)
  {
    Zbug.Log($"{name} received {healthED.m_HpDelta} HP damage");

    var newHp = Mathf.Max(m_Hp - healthED.m_HpDelta, 0);
    m_TargetHp = newHp;
    m_HpChanging = true;
    healthED.m_CurrentShield = m_Shield;
    healthED.m_CurrentHp = m_Hp;
    ED.Dispatch(Events.HpReceivedDamage, healthED);

    if (WillDie)
      ED.Dispatch(Events.HpReceivedTerminalDamage, healthED);

    Zbug.Log($"    {GetStatusReport()}");
  }


  private void UpdateShield(float dt)
  {
    var delta = m_ChangeRate * dt * (m_TargetShield - m_Shield > 0 ? 1 : -1);
    m_Shield += delta;

    m_CurrentHealthED.m_CurrentShield = m_Shield;
    m_CurrentHealthED.m_CurrentHp = m_Hp;

    if (delta < 0)
    {
      ED.Dispatch(Events.ShieldDamageUpdate, m_CurrentHealthED);

      if (ShieldEmpty)
        DepleteShield(m_CurrentHealthED);
    }
    else
    {
      ED.Dispatch(Events.ShieldHealUpdate, m_CurrentHealthED);

      if (ShieldFull)
        FillShield(m_CurrentHealthED);
    }

    var shieldDot = delta * (m_TargetShield - m_Shield);
    if (shieldDot <= 0)
    {
      m_Shield = m_TargetShield;
      m_ShieldChanging = false;
    }
  }


  private void UpdateHp(float dt)
  {
    var delta = m_ChangeRate * dt * (m_TargetHp - m_Hp > 0 ? 1 : -1);
    m_Hp += delta;

    m_CurrentHealthED.m_CurrentShield = m_Shield;
    m_CurrentHealthED.m_CurrentHp = m_Hp;

    if (delta < 0)
    {
      ED.Dispatch(Events.HpDamageUpdate, m_CurrentHealthED);

      if (HpFull)
        FillHp(m_CurrentHealthED);
    }
    else
    {
      ED.Dispatch(Events.HpHealUpdate, m_CurrentHealthED);

      if (Dead)
        Die(m_CurrentHealthED);
    }

    var hpDot = delta * (m_TargetHp - m_Hp);
    if (hpDot <= 0)
    {
      m_Hp = m_TargetHp;
      m_HpChanging = false;
    }
  }


  void DepleteShield(HealthEventData healthED)
  {
    Zbug.Log($"{name}'s shield is depleted!");

    ED.Dispatch(Events.ShieldDepleted, healthED);
  }


  void Die(HealthEventData healthED)
  {
    Zbug.Log($"{name} has died!");

    ED.Dispatch(Events.Died, healthED);
  }


  void ReceiveShieldHeal(HealthEventData healthED)
  {
    Zbug.Log($"{name} recovered {healthED.m_ShieldDelta} shield");

    var oldShield = m_Shield;
    var newShield = Mathf.Min(m_Shield + healthED.m_ShieldDelta, m_ShieldMax);
    m_Shield = newShield;
    ED.Dispatch(Events.ShieldReceivedHeal, healthED);

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
    ED.Dispatch(Events.HpReceivedHeal, healthED);

    Zbug.Log($"    {GetStatusReport()}");

    if (m_Hp >= m_HpMax && oldHp < m_Hp)
      FillHp(healthED);
  }


  void FillShield(HealthEventData healthED)
  {
    Zbug.Log($"{name}'s shield is maxed out!");

    ED.Dispatch(Events.ShieldFilled, healthED);
  }


  void FillHp(HealthEventData healthED)
  {
    Zbug.Log($"{name}'s HP is maxed out!");

    ED.Dispatch(Events.HpFilled, healthED);
  }


  string GetStatusReport()
  {
    const string blue = "#5af";
    const string red = "#f55";
    var shieldString = $"Sh: ({m_TargetShield} / {m_ShieldMax})".B().Color(blue);
    var hpString = $"HP: ({m_TargetHp} / {m_HpMax})".B().Color(red);

    return $"{name} | {shieldString} | {hpString}";
  }


  void OnDisable()
  {
    ED.RemoveListener(Events.DamageRequest, OnDamageRequest);
    ED.RemoveListener(Events.HealRequest, OnHealRequest);
  }
}
