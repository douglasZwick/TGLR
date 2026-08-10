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
  public static readonly EventKey<HealthEventData> HealthSetup = new();
  public static readonly EventKey<HealthEventData> ShieldSetup = new();

  public static readonly EventKey<HealthEventData> DamageRequest = new();
  public static readonly EventKey<HealthEventData> HealRequest = new();
  public static readonly EventKey<HealthEventData> DamagePreProcess = new();

  public static readonly EventKey<HealthEventData> ShieldReceivedDamage = new();
  public static readonly EventKey<HealthEventData> ShieldDamageStarted = new();
  public static readonly EventKey<HealthEventData> ShieldUpdate = new();
  public static readonly EventKey<HealthEventData> ShieldDepleted = new();
  
  public static readonly EventKey<HealthEventData> ShieldReceivedHeal = new();
  public static readonly EventKey<HealthEventData> ShieldHealStarted = new();
  public static readonly EventKey<HealthEventData> ShieldFilled = new();

  public static readonly EventKey<HealthEventData> HealthReceivedDamage = new();
  public static readonly EventKey<HealthEventData> Died = new();

  public static readonly EventKey<HealthEventData> HealthReceivedHeal = new();
  public static readonly EventKey<HealthEventData> HealthFilled = new();

  public static readonly EventKey<HealthEventData> CausedShieldDamage = new();
  public static readonly EventKey<HealthEventData> CausedHealthDamage = new();
  public static readonly EventKey<HealthEventData> Killed = new();
  // CONSIDER:
  //   Add an event for depleting target's shields, if it becomes reasonable to do so

  // Shake Events
  public static readonly EventKey<ShakeEventData> ShakeRequest = new();

  // Gauge Events
  public static readonly EventKey<GaugeEventData> GaugeSetup = new();
  public static readonly EventKey<GaugeEventData> GaugeValueChanged = new();
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
  public float m_IncomingShieldDamage;
  public float m_IncomingHealthDamage;
  public float m_IncomingShieldHeal;
  public float m_IncomingHealthHeal;
  public float m_StartingEnergy;
  public float m_StartingHp;
  public float m_CurrentEnergy;
  public float m_CurrentHp;
  public float m_Penetration;
  public float m_HpMax;
  public float m_EnergyMax;
  public DamageType m_Type;

  public void SourceDispatch<TData>(EventKey<TData> key, TData eventData)
    => m_Source.ED.Dispatch(key, eventData);
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
  public float m_AnimationDuration;
}
