using UnityEngine;


public class AutoRotator : MonoBehaviour
{
  [Tooltip("The axis around which to rotate.")]
  public Vector3 m_Axis = Vector3.up;
  [Tooltip("How long one full rotation should take, in seconds.")]
  public float m_Period = 1;

  private Transform m_Tx;
  private float m_Timer = 0;


  void Awake()
  {
    m_Tx = transform;
  }


  void Update()
  {
    Rotate(Time.deltaTime);
  }


  void Rotate(float dt)
  {
    if (m_Period == 0) return;
    if (m_Axis == Vector3.zero) return;

    var t = m_Timer / m_Period;
    var angle = 360 * t;
    m_Tx.localRotation = Quaternion.AngleAxis(angle, m_Axis);

    m_Timer = (m_Timer + dt) % m_Period;
  }
}
