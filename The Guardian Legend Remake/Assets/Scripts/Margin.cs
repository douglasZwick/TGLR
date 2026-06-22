using UnityEngine;


[System.Serializable]
public struct Margin
{
  public Vector2 m_RightTop;
  public Vector2 m_LeftBottom;

  public Margin(Vector2 rightTop, Vector2 leftBottom)
  {
    m_RightTop = rightTop;
    m_LeftBottom = leftBottom;
  }

  public Margin(float right, float top, float left, float bottom)
  {
    m_RightTop = new Vector2(right, top);
    m_LeftBottom = new Vector2(left, bottom);
  }

  public static Margin operator +(Margin lhs, Margin rhs)
    => new(lhs.m_RightTop + rhs.m_RightTop, lhs.m_LeftBottom + rhs.m_LeftBottom);
}
