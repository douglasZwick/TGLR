using UnityEngine;


[RequireComponent(typeof(DamageSource))]
public class DamageOnCollide : MonoBehaviour
{
  public bool m_DamageOnCollisionEnter2D = true;
  public bool m_DamageOnTriggerEnter2D = true;

  private DamageSource m_DamageSource;


  void Awake()
  {
    m_DamageSource = GetComponent<DamageSource>();
  }


  void OnCollisionEnter2D(Collision2D collision)
  {
    if (!m_DamageOnCollisionEnter2D) return;

    AttemptDamage(collision.gameObject);
  }


  void OnTriggerEnter2D(Collider2D collision)
  {
    if (!m_DamageOnTriggerEnter2D) return;

    AttemptDamage(collision.gameObject);
  }


  void AttemptDamage(GameObject other)
  {
    if (other.TryGetComponent<Health>(out var receiver))
      m_DamageSource.RequestDamage(receiver);
  }
}
