using UnityEngine;


[RequireComponent(typeof(EventDispatcher))]
abstract public class Mover : MonoBehaviour
{
  public EventDispatcher ED { get; private set; }
  
  // TODO:
  //   Consider changing this to an accumulator that I clear on LateUpdate. That might be more
  //   responsive in certain cases.
  protected Vector2 m_MoveInput;


  protected virtual void Awake()
  {
    ED = GetComponent<EventDispatcher>();
  }


  void FixedUpdate()
  {
    Move();
  }


  public void SetMoveInput(Vector2 moveInput)
  {
    m_MoveInput = moveInput;
  }

  protected virtual void Move() {}
}
