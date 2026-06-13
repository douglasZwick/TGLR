using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
public class BasicMover : MonoBehaviour
{
  public float m_MovementSpeed = 5;

  private Rigidbody2D m_RB;
  private PlayerInput m_PlayerInput;

  private InputAction m_MoveAction;


  void Awake()
  {
    m_RB = GetComponent<Rigidbody2D>();
    m_PlayerInput = GetComponent<PlayerInput>();
    
    m_MoveAction = m_PlayerInput.actions.FindAction("Move");
  }
  

  void Update()
  {
    var input = m_MoveAction.ReadValue<Vector2>();
    var velocity = input * m_MovementSpeed;
    m_RB.linearVelocity = velocity;
  }
}
