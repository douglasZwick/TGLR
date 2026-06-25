using UnityEngine;


abstract public class Mover : MonoBehaviour
{
  [System.Serializable]
  public class Events
  {
    public MovementEvent m_MoveRequested;
    public MovementEvent m_Moved;
  }
  
  // TODO:
  //   Consider changing this to an accumulator that I clear on LateUpdate. That might be more
  //   responsive in certain cases.
  protected Vector2 m_MoveInput;

  public Events m_Events;


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
