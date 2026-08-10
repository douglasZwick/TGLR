using UnityEngine;


[RequireComponent(typeof(EventDispatcher))]
public abstract class Health : MonoBehaviour
{
  public EventDispatcher ED { get; private set; }

  protected float m_Hp;
  [SerializeField]
  protected float m_HpMax = 3;

  protected bool HpFull => m_Hp >= m_HpMax;
  protected bool Dead => m_Hp <= 0;

  protected virtual bool Invincible => false;


  protected virtual void Awake()
  {
    ED = GetComponent<EventDispatcher>();

    m_Hp = m_HpMax;
  }


  void OnEnable()
  {
    ED.AddListener(Events.DamageRequest, OnDamageRequest);
  }


  void Start()
  {
    var healthED = new HealthEventData()
    {
      m_HpMax = m_HpMax,
    };
    ED.Dispatch(Events.HealthSetup, healthED);
  }


  protected void OnDamageRequest(HealthEventData healthED)
  {
    if (Dead) return;
    if (Invincible) return;

    healthED.m_HpMax = m_HpMax;

    ED.Dispatch(Events.DamagePreProcess, healthED);

    if (healthED.m_IncomingHealthDamage <= 0) return;
    
    ReceiveDamage(healthED);
  }


  void OnHealRequest(HealthEventData healthED)
  {
    if (Dead) return;
    if (healthED.m_IncomingHealthHeal <= 0) return;
    
    ReceiveHeal(healthED);
  }


  protected abstract void ReceiveDamage(HealthEventData healthED);
  protected abstract void ReceiveHeal(HealthEventData healthED);


  protected void Die(HealthEventData healthED)
  {
    Zbug.Log($"{name} has died!");
    
    ED.Dispatch(Events.Died, healthED);
    healthED.SourceDispatch(Events.Killed, healthED);
  }


  protected void Fill(HealthEventData healthED)
  {
    Zbug.Log($"{name}'s HP is maxed out!");

    ED.Dispatch(Events.HealthFilled, healthED);
  }


  // string GetStatusReport()
  // {
  //   const string blue = "#5af";
  //   const string red = "#f55";
  //   var shieldString = $"Sh: ({m_TargetShield} / {m_ShieldMax})".B().Color(blue);
  //   var hpString = $"HP: ({m_TargetHp} / {m_HpMax})".B().Color(red);

  //   return $"{name} | {shieldString} | {hpString}";
  // }


  void OnDisable()
  {
    ED.RemoveListener(Events.DamageRequest, OnDamageRequest);
    ED.RemoveListener(Events.HealRequest, OnHealRequest);
  }
}
