using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(KinematicMover))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
  private KinematicMover m_KinematicMover;
  private InputAction m_MoveAction;


  void Awake()
  {
    m_KinematicMover = GetComponent<KinematicMover>();

    var playerInput = GetComponent<PlayerInput>();
    m_MoveAction = playerInput.actions.FindAction("Move");
  }

  
  void Update()
  {
    SetPlayerInput();
  }


  void SetPlayerInput()
  {
    var moveInput = m_MoveAction.ReadValue<Vector2>();
    m_KinematicMover.SetMoveInput(moveInput);
  }
}
