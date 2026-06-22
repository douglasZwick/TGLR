using UnityEngine;


public class Lifespan : MonoBehaviour
{
  public float m_Duration = 1;


  void Start()
  {
    Destroy(gameObject, m_Duration);
  }
}
