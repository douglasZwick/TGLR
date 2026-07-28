using System.Collections.Generic;
using System.Text;
using UnityEngine;


public class DummyCannon : MonoBehaviour
{
  [SerializeField]
  private List<float> m_ShotTimings;
  [SerializeField]
  private float m_Period;
  [SerializeField]
  private Projectile m_BulletPrefab;
  [SerializeField]
  private float m_BulletSpeed = 10;
  [SerializeField]
  private Transform m_FiringPoint;
  [SerializeField]
  private AudioItem m_ShootSound;
  [SerializeField]
  private bool m_Noisy = false;

  private float m_Timer = 0;
  private int m_ShotIndex = 0;
  private float CurrentTiming =>
    m_ShotIndex >= m_ShotTimings.Count ? float.PositiveInfinity : m_ShotTimings[m_ShotIndex];


  void Update()
  {
    if (m_Timer >= CurrentTiming)
    {
      Fire();
      ++m_ShotIndex;
    }

    m_Timer += Time.deltaTime;
    if (m_Timer >= m_Period)
    {
      m_Timer %= m_Period;
      m_ShotIndex = 0;
    }
  }


  void Fire()
  {
    if (m_Noisy)
      GoPew();
      
    var bullet = Instantiate(m_BulletPrefab, m_FiringPoint.position, m_FiringPoint.rotation);
    bullet.Setup(m_BulletSpeed);

    // AudioManager.Instance.Play(m_ShootSound);
  }


  void GoPew()
  {
    var paddingWidth = (int)(m_Timer * 4.0);
    var padding = new string(' ', 2 * paddingWidth);
    Zbug.Log($"{padding}Pew!");
  }
}
