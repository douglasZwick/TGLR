using UnityEngine;


[RequireComponent(typeof(Health))]
public class HealthSounds : MonoBehaviour
{
  [SerializeField]
  private AudioItem m_ShieldDamageSound;
  [SerializeField]
  private AudioItem m_HpDamageSound;
  [SerializeField]
  private AudioItem m_DeathSound;


  void Awake()
  {
    var health = GetComponent<Health>();
    health.m_Events.ReceivedShieldDamage.AddListener(OnReceivedShieldDamage);
    health.m_Events.ReceivedHpDamage.AddListener(OnReceivedHpDamage);
    health.m_Events.Died.AddListener(OnDied);
  }


  public void OnReceivedShieldDamage(HealthEventData healthED)
  {
    if (m_ShieldDamageSound == null) return;
    AudioManager.Instance.Play(m_ShieldDamageSound);
  }


  public void OnReceivedHpDamage(HealthEventData healthED)
  {
    if (m_HpDamageSound == null) return;
    AudioManager.Instance.Play(m_HpDamageSound);
  }


  public void OnDied(HealthEventData healthED)
  {
    if (m_DeathSound == null) return;
    AudioManager.Instance.Play(m_DeathSound);
  }
}
