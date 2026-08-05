using UnityEngine;


public abstract class EventKey { }
public sealed class EventKey<TData> : EventKey { }


public static class Events
{
  // Fire Events
  public static readonly EventKey<FireEventData> DidFire = new();
  public static readonly EventKey<FireEventData> WasFired = new();

  // Bounds Events
  public static readonly EventKey<BoundsEventData> BoundsEnter = new();
  public static readonly EventKey<BoundsEventData> BoundsIn = new();
  public static readonly EventKey<BoundsEventData> BoundsExit = new();
  public static readonly EventKey<BoundsEventData> BoundsOut = new();

  // Movement Events
  public static readonly EventKey<MovementEventData> MoveRequested = new();
  public static readonly EventKey<MovementEventData> Moved = new();

  // Health Events
  public static readonly EventKey<HealthEventData> DamageRequest = new();
  public static readonly EventKey<HealthEventData> HealRequest = new();

  public static readonly EventKey<HealthEventData> ShieldReceivedDamage = new();
  public static readonly EventKey<HealthEventData> ShieldReceivedTerminalDamage = new();
  public static readonly EventKey<HealthEventData> ShieldDamageUpdate = new();
  public static readonly EventKey<HealthEventData> ShieldDepleted = new();

  public static readonly EventKey<HealthEventData> HpReceivedDamage = new();
  public static readonly EventKey<HealthEventData> HpReceivedTerminalDamage = new();
  public static readonly EventKey<HealthEventData> HpDamageUpdate = new();
  public static readonly EventKey<HealthEventData> Died = new();

  public static readonly EventKey<HealthEventData> HpReceivedHeal = new();
  public static readonly EventKey<HealthEventData> HpReceivedTotalHeal = new();
  public static readonly EventKey<HealthEventData> HpHealUpdate = new();
  public static readonly EventKey<HealthEventData> HpFilled = new();
  
  public static readonly EventKey<HealthEventData> ShieldReceivedHeal = new();
  public static readonly EventKey<HealthEventData> ShieldReceivedTotalHeal = new();
  public static readonly EventKey<HealthEventData> ShieldHealUpdate = new();
  public static readonly EventKey<HealthEventData> ShieldFilled = new();

  // Shake Events
  public static readonly EventKey<ShakeEventData> ShakeRequest = new();

  // Gauge Events
  public static readonly EventKey<GaugeEventData> GaugeChangeStarted = new();
  public static readonly EventKey<GaugeEventData> GaugeUpdate = new();
  public static readonly EventKey<GaugeEventData> GaugeChangeEnded = new();
}


public class FireEventData
{
  public float m_Speed;
}


public class BoundsEventData
{
  public Vector2 m_WorldPosition;
  public Vector2 m_Resolution;
}


public class MovementEventData
{
  public Vector3 m_PreviousPosition;
  public Vector3 m_FinalPosition;
  public Vector3 m_Delta;
  public Vector2 m_Direction;
}


public class ScrollEventData
{
  public float m_Speed;
  public float m_Acceleration;
  public Vector2 m_Direction;
}


public class HealthEventData
{
  public DamageSource m_Source;
  public float m_ShieldDelta;
  public float m_CurrentShield;
  public float m_ShieldMax;
  public float m_HpDelta;
  public float m_CurrentHp;
  public float m_HpMax;
  public DamageType m_Type;
}


public class ShakeEventData
{
  public float m_Trauma;
}


public class GaugeEventData
{
  public float m_MaxValue;
  public float m_StartingValue;
  public float m_CurrentValue;
  public float m_EndingValue;
}
