[System.Serializable]
public struct DamageData
{
  public float m_ShieldDamageAmount;
  public float m_HealthDamageAmount;
  public float m_Penetration;
  public float m_Durability;
  public bool m_UseDurability;
  public DamageType m_Type;


  public override readonly string ToString()
  {
    var sh = m_ShieldDamageAmount;
    var hp = m_HealthDamageAmount;
    var pen = m_Penetration;
    var dur = m_Durability;
    var use = m_UseDurability;
    var type = m_Type;

    var penStr = pen == 0 ? "" : $"Pen {pen}";
    var durStr = use ? $"Dur {dur}" : "";
    var typeStr = m_Type == DamageType.None ? "" : $"{type}";

    return $"Sh {sh} | Hp {hp} | {penStr} | {durStr} | {typeStr}";
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
