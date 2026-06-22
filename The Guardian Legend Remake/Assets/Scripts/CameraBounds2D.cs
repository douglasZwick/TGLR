using UnityEngine;


[RequireComponent(typeof(Camera))]
public class CameraBounds2D : MonoBehaviour
{
  public Margin m_Margin;

  private Transform m_Tx;
  private Camera m_Camera;


  void Awake()
  {
    m_Tx = transform;
    m_Camera = GetComponent<Camera>();
  }


  public Vector2 ComputeCorrectedPosition(Vector2 desiredPosition, Margin margin)
  {
    var halfH = m_Camera.orthographicSize;
    var halfW = halfH * m_Camera.aspect;
    var local = m_Tx.InverseTransformPoint(desiredPosition);

    margin += m_Margin;

    var l = -halfW - margin.m_LeftBottom.x;
    var r = +halfW + margin.m_RightTop.x;
    var b = -halfH - margin.m_LeftBottom.y;
    var t = +halfH + margin.m_RightTop.y;

    local.x = Mathf.Clamp(local.x, l, r);
    local.y = Mathf.Clamp(local.y, b, t);

    return m_Tx.TransformPoint(local);
  }
}
