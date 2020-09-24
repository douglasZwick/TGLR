using UnityEngine;

public class BasicBullet : MonoBehaviour
{
  public float m_Speed = 16.0f;

  Rigidbody m_RB;


  void Start()
  {
    m_RB = GetComponent<Rigidbody>();
    var velocity = m_Speed * Vector3.up;
    m_RB.velocity = velocity;
  }
}
