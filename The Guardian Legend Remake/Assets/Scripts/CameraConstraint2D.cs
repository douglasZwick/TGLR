using UnityEngine;


[RequireComponent(typeof(BoundsChecker2D))]
public class CameraConstraint2D : MonoBehaviour
{
  private BoundsChecker2D m_BoundsChecker2D;


  void Awake()
  {
    m_BoundsChecker2D = GetComponent<BoundsChecker2D>();
  }


  public void OnMoveRequested(MovementEventData movementED)
  {
    var desiredPosition = movementED.m_FinalPosition;
    var correctedPosition = m_BoundsChecker2D.ComputeCorrectedPosition(desiredPosition);
    movementED.m_FinalPosition = correctedPosition;
  }
}
