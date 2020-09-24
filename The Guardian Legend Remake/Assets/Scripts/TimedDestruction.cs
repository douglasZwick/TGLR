using UnityEngine;

public class TimedDestruction : MonoBehaviour
{
  public float m_Lifespan = 1.0f;


  void Start()
  {
    Destroy(gameObject, m_Lifespan);
  }
}
