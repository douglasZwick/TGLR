using UnityEngine;
using UnityEngine.Events;


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
}


[System.Serializable]
public class ScrollEvent : UnityEvent<ScrollEventData> { }

public class ScrollEventData
{
  public float m_Speed;
  public float m_Acceleration;
  public Vector2 m_Direction;
}
