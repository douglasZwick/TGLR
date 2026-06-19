using UnityEngine;


public class DestroyOnLeftScreen : MonoBehaviour
{
  public Margin m_Buffer;

  private Transform m_Tx;
  private Camera m_Camera;


  void Awake()
  {
    m_Tx = transform;
    // CONDITIONAL TODO:
    //   If I ever use multiple cameras, this shouldn't necessarily use caching
    m_Camera = Camera.main;
  }


  void Update()
  {
    var p = m_Camera.WorldToViewportPoint(m_Tx.position);

    var l = 0 - m_Buffer.m_LeftBottom.x;
    var r = 1 + m_Buffer.m_RightTop.x;
    var b = 0 - m_Buffer.m_LeftBottom.y;
    var t = 1 + m_Buffer.m_RightTop.y;

    if (l < p.x && p.x < r &&
        b < p.y && p.y < t)
      return;

    Destroy(gameObject);
  }
}
