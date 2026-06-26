using UnityEngine;


public class Facer : MonoBehaviour
{
  public Transform m_RotationRoot;
  public float m_MaxTurnDuration = 0.25f; // How long a 180-degree turn takes

  private float m_TargetAngle;
  private float m_TurnTimer = 0;
  private bool Rotating => m_TurnTimer < m_MaxTurnDuration;
  private float m_FromAngle;


  void Update()
  {
    Turn(Time.deltaTime);
  }


  public void OnMoveRequested(MovementEventData movementED)
  {
    SetTargetAngle(movementED);
    BeginTurning();
  }


  void SetTargetAngle(MovementEventData movementED)
  {
    m_TargetAngle = ComputeAngleFromDirection(movementED.m_Direction);
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

  }


  void EndTurning()
  {
    SetRotation(m_TargetAngle);
  }


  void SetRotation(float angle)
  {
    m_RotationRoot.localRotation = Quaternion.AngleAxis(angle, Vector3.up);
  }
}
