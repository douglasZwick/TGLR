using UnityEngine;


[RequireComponent(typeof(EventDispatcher))]
public class DamageSource : MonoBehaviour
{
  public EventDispatcher ED { get; private set; }

  [SerializeField]
  private float m_ShieldDamageAmount;
  [SerializeField]
  private float m_HealthDamageAmount;
  [SerializeField]
  private float m_Penetration;
  [SerializeField]
  private DamageType m_Type;


  void Awake()
  {
    ED = GetComponent<EventDispatcher>();
  }


  public void TryRequestDamage(GameObject target)
  {
    if (!target.TryGetComponent<EventDispatcher>(out var ed)) return;
    var healthED = CreateHealthEventData();
    ed.Dispatch(Events.DamageRequest, healthED);
  }


  HealthEventData CreateHealthEventData()
  {
    var healthED = new HealthEventData()
    {
      m_Source = this,
      m_IncomingShieldDamage = m_ShieldDamageAmount,
      m_IncomingHealthDamage = m_HealthDamageAmount,
      m_Penetration = m_Penetration,
      m_Type = m_Type,
    };

    return healthED;
  }
}


public enum DamageType
{
  // No particular type
  None,
  // Damage by being too hot
  Heat,
  // Damage by being too cold
  Cold,
  // Damage by electrical shock
  Electricity,
  // Damage by chemical burn
  Corrosion,
  // Damage by biological interference
  Toxic,
  // Damage by shockwave
  Force,
  // Damage directly to the mind
  Psychic,
  // Damage caused by divine intervention
  Holy,

  // Damage by puncture via a fine point
  Piercing,
  // Damage by cut via a sharp edge
  Slashing,
  // Damage by blunt trauma
  Smashing,
  // Damage by being pulled apart
  Tension,
  // Damage by being crushed inward
  Compression,
}
