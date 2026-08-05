using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class KinematicMover : Mover
{
  [SerializeField]
  private float m_MovementSpeed = 12;

  private Rigidbody2D m_RB;


  override protected void Awake()
  {
    base.Awake();
    
    m_RB = GetComponent<Rigidbody2D>();
  }


  public void SetMovementSpeed(float movementSpeed)
  {
    m_MovementSpeed = movementSpeed;
  }


  protected override void Move()
  {
    var velocity = m_MoveInput * m_MovementSpeed;
    var delta = velocity * Time.deltaTime;
    var previousPosition = m_RB.position;
    var finalPosition = previousPosition + delta;
    
    var movementED = new MovementEventData()
    {
      m_PreviousPosition = previousPosition,
      m_FinalPosition = finalPosition,
      m_Delta = delta,
      m_Direction = m_MoveInput.normalized,
    };

    ED.Dispatch(Events.MoveRequested, movementED);

    m_RB.MovePosition(movementED.m_FinalPosition);

    ED.Dispatch(Events.Moved, movementED);
  }
}
