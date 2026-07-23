using UnityEngine;


public class DamageSource : MonoBehaviour
{
  [System.Serializable]
  public class Events
  {
    public HealthEvent CausedShieldDamage;
    public HealthEvent CausedHpDamage;
  }


  [SerializeField]
  private float m_ShieldDamageAmount;
  [SerializeField]
  private float m_HpDamageAmount;
  [SerializeField]
  private DamageType m_Type;

  public Events m_Events;


  public void RequestDamage(Health receiver)
  {
    var healthED = CreateHealthEventData();
    receiver.m_Events.DamageRequested.Invoke(healthED);
  }


  HealthEventData CreateHealthEventData()
  {
    var healthED = new HealthEventData()
    {
      m_ShieldDelta = m_ShieldDamageAmount,
      m_HpDelta = m_HpDamageAmount,
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
