using UnityEngine;


public class Facer : MonoBehaviour
{
  public Transform m_RotationRoot;
  public float m_MaxTurnDuration = 0.25f; // How long a 180-degree turn takes

  private float m_FromAngle;
  private float m_ToAngle;
  private float m_TurnTimer = 0;
  private float m_TurnDuration;
  private bool Turning => m_TurnTimer < m_MaxTurnDuration;

  private float Alpha => m_TurnDuration == 0 ? 1 : m_TurnTimer / m_TurnDuration;


  void Update()
  {
    if (!Turning) return;
    
    Turn(Time.deltaTime);
  }


  public void OnMoveRequested(MovementEventData movementED)
  {
    SetTargetAngle(movementED);

    BeginTurning();
  }


  float GetCurrentAngle()
  {
    return m_RotationRoot.localEulerAngles.y;
  }


  void SetTargetAngle(MovementEventData movementED)
  {
    m_ToAngle = ComputeAngleFromDirection(movementED.m_Direction);
  }


  void AngleSetup(float fromAngle, float toAngle)
  {
    m_FromAngle = fromAngle;
    m_ToAngle = toAngle;
  }


  float ComputeAngleFromDirection(Vector2 direction)
  {
    var x = direction.x;
    var y = direction.y;
    return Mathf.Atan2(y, x);
  }


  void BeginTurning()
  {
    m_TurnTimer = 0;
  }


  void Turn(float dt)
  {
    m_TurnTimer += dt;

    if (m_TurnTimer >= m_TurnDuration)
    {
      EndTurning();
      
      return;
    }

    var newAngle = Mathf.Lerp(m_FromAngle, m_ToAngle, Alpha);
    SetRotation(newAngle);
  }


  void EndTurning()
  {
    SetRotation(m_ToAngle);
  }


  void SetRotation(float angle)
  {
    m_RotationRoot.localRotation = Quaternion.AngleAxis(angle, Vector3.up);
  }
}
