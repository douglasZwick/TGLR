using UnityEngine;

public class Health : MonoBehaviour
{
  public int m_MaxHP = 100;

  public int m_HP { get; private set; }


  void Start()
  {
    m_HP = m_MaxHP;
  }


  public void TakeDamage(int amount)
  {
    m_HP -= amount;

    if (amount > 0)
      TookDamage(amount);

    if (m_HP <= 0)
    {
      Die();
    }
  }


  void Die()
  {
    Destroy(gameObject);  // TODO: replace with events
  }


  void TookDamage(int amount)
  {

  }
}
