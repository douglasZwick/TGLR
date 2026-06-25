using UnityEngine;


[RequireComponent(typeof(KinematicMover))]
public class ScrollController : MonoBehaviour
{
  [SerializeField]
  private Vector2 m_Direction;
  [SerializeField]
  private float m_Speed = 5;
  private float m_Acceleration = 0;
  private float m_TargetSpeed;

  private KinematicMover m_KinematicMover;


  void Awake()
  {
    m_KinematicMover = GetComponent<KinematicMover>();
  }


  void Update()
  {
    SetScrollInput();

    if (m_Acceleration == 0) return;

    Accelerate();
  }


  void SetScrollInput()
  {
    m_KinematicMover.SetMovementSpeed(m_Speed);
    m_KinematicMover.SetMoveInput(-m_Direction);
  }


  void Accelerate()
  {
    var delta = m_Acceleration * Time.deltaTime;
    var newSpeed = m_Speed + delta;

    if (newSpeed <= m_TargetSpeed && m_TargetSpeed <= m_Speed
     || m_Speed <= m_TargetSpeed && m_TargetSpeed <= newSpeed)
    {
      m_Speed = m_TargetSpeed;
      StopAccelerating();

      return;
    }

    m_Speed = newSpeed;
  }


  public void SetSpeed(float speed)
  {
    m_Speed = speed;
  }


  public void SetAcceleration(float acceleration, float targetSpeed)
  {
    m_Acceleration = acceleration;
    m_TargetSpeed = targetSpeed;
  }


  public void SetTargetSpeed(float targetSpeed)
  {
    m_TargetSpeed = targetSpeed;
  }


  public void SetDirection(Vector2 direction)
  {
    m_Direction = direction;
  }


  public void StopAccelerating()
  {
    m_Acceleration = 0;
  }
}
