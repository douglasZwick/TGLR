using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
public class KinematicMover : MonoBehaviour
{
  [System.Serializable]
  public class Events
  {
    public MovementEvent m_MoveRequested;
    public MovementEvent m_Moved;
  }

  public float m_MovementSpeed = 12;

  private Rigidbody2D m_RB;
  private InputAction m_MoveAction;
  private Vector2 m_MoveInput;

  public Events m_Events;


  void Awake()
  {
    m_RB = GetComponent<Rigidbody2D>();

    var playerInput = GetComponent<PlayerInput>();
    m_MoveAction = playerInput.actions.FindAction("Move");
  }


  void Update()
  {
    CacheInput();
  }


  void FixedUpdate()
  {
    Move();
  }


  void CacheInput()
  {
    m_MoveInput = m_MoveAction.ReadValue<Vector2>();
  }


  void Move()
  {
    var velocity = m_MoveInput * m_MovementSpeed;
    var delta = velocity * Time.deltaTime;
    var previousPosition = m_RB.position;
    var finalPosition = previousPosition + delta;
    
    var movementED = new MovementEventData()
    {
      m_PreviousPosition = previousPosition,
      m_FinalPosition = finalPosition,
    };

    m_Events.m_MoveRequested.Invoke(movementED);

    m_RB.MovePosition(movementED.m_FinalPosition);
  }
}
