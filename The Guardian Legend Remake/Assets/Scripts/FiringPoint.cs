using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public record FiringPoint
{
  public Vector2 m_Position;
  public Quaternion m_Rotation;
}


[System.Serializable]
public class ClusterPattern : IEnumerable<FiringPoint>
{
  public List<FiringPoint> m_Pattern;

  public IEnumerator<FiringPoint> GetEnumerator()
  {
    foreach (var item in m_Pattern)
      yield return item;
  }

  IEnumerator IEnumerable.GetEnumerator()
  {
    return GetEnumerator();
  }
}
