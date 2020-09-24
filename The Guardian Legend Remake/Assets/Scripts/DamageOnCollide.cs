using UnityEngine;

public class DamageOnCollide : MonoBehaviour
{
  public int m_DamageAmount = 10;
  public bool m_DestroyOnDamage = true;


  private void OnCollisionEnter(Collision collision)
  {
    var health = collision.gameObject.GetComponent<Health>();
    if (health != null)
    {
      health.TakeDamage(m_DamageAmount);

      if (m_DestroyOnDamage)
        Destroy(gameObject);
    }
  }
}
