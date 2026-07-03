using System.Collections.Generic;
using UnityEngine;


public class SimpleFacer : MonoBehaviour
{
  public float m_AngleOffset;
  [SerializeField]
  private List<Transform> m_RotationNodes;


  public void OnMoveRequested(MovementEventData movementED)
  {
    FaceDirection(movementED.m_Direction);
  }


  void FaceDirection(Vector2 direction)
  {
    if (direction == Vector2.zero) return;

    var angle = ComputeAngleFromDirection(direction);

    foreach (var node in m_RotationNodes)
      SetNodeRotation(node, angle);
  }


  float ComputeAngleFromDirection(Vector2 direction)
  {
    var x = direction.x;
    var y = direction.y;
    var angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;

    return angle;
  }


  void SetNodeRotation(Transform rotationNode, float angle)
  {
    var localEulerAngles = rotationNode.localEulerAngles;
    localEulerAngles.y = m_AngleOffset - angle;
    rotationNode.localEulerAngles = localEulerAngles;
  }
}
