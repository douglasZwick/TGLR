using UnityEngine;
using UnityEngine.Events;


public abstract class EventKey { }
public sealed class EventKey<TData> : EventKey { }


[System.Serializable]
public class FireEvent : UnityEvent<FireEventData> { }

public class FireEventData
{
  public float m_Speed;
}


[System.Serializable]
public class BoundsEvent : UnityEvent<BoundsEventData> { }

public class BoundsEventData
{
  public Vector2 m_WorldPosition;
  public Vector2 m_Resolution;
}


[System.Serializable]
public class MovementEvent : UnityEvent<MovementEventData> { }

public class MovementEventData
{
  public Vector3 m_PreviousPosition;
  public Vector3 m_FinalPosition;
  public Vector3 m_Delta;
  public Vector2 m_Direction;
}


[System.Serializable]
public class ScrollEvent : UnityEvent<ScrollEventData> { }

public class ScrollEventData
{
  public float m_Speed;
  public float m_Acceleration;
  public Vector2 m_Direction;
}


[System.Serializable]
public class HealthEvent : UnityEvent<HealthEventData> { }

public class HealthEventData
{
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

public static class ShakeEvents
{
  public static readonly EventKey<ShakeEventData> ShakeRequest = new();
}


public class GaugeEventData
{
  public float m_MaxValue;
  public float m_StartingValue;
  public float m_CurrentValue;
  public float m_EndingValue;
}

public static class GaugeEvents
{
  public static readonly EventKey<GaugeEventData> GaugeChangeStarted = new();
  public static readonly EventKey<GaugeEventData> GaugeUpdate = new();
  public static readonly EventKey<GaugeEventData> GaugeChangeEnded = new();
}
