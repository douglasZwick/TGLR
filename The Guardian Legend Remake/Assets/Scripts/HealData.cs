[System.Serializable]
public struct HealData
{
  public float m_ShieldHealAmount;
  public float m_HealthHealAmount;

  public override readonly string ToString()
    => $"Sh {m_ShieldHealAmount} | Hp {m_HealthHealAmount}";
}
