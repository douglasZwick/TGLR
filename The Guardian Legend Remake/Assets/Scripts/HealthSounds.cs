using UnityEngine;


[RequireComponent(typeof(EventDispatcher))]
public class HealthSounds : MonoBehaviour
{
  public EventDispatcher ED { get; private set; }
  
  [SerializeField]
  private AudioItem m_ShieldDamageSound;
  [SerializeField]
  private AudioItem m_HpDamageSound;
  [SerializeField]
  private AudioItem m_DeathSound;


  void Awake()
  {
    ED = GetComponent<EventDispatcher>();
  }


  void OnEnable()
  {
    ED.AddListener(Events.ShieldReceivedDamage, OnShieldReceivedDamage);
    ED.AddListener(Events.HealthReceivedDamage, OnHpReceivedDamage);
    ED.AddListener(Events.Died, OnDied);
  }


  public void OnShieldReceivedDamage(HealthEventData healthED)
  {
    if (m_ShieldDamageSound == null) return;
    AudioManager.Instance.Play(m_ShieldDamageSound);
  }


  public void OnHpReceivedDamage(HealthEventData healthED)
  {
    if (m_HpDamageSound == null) return;
    AudioManager.Instance.Play(m_HpDamageSound);
  }


  public void OnDied(HealthEventData healthED)
  {
    if (m_DeathSound == null) return;
    AudioManager.Instance.Play(m_DeathSound);
  }


  void OnDisable()
  {
    ED.RemoveListener(Events.ShieldReceivedDamage, OnShieldReceivedDamage);
    ED.RemoveListener(Events.HealthReceivedDamage, OnHpReceivedDamage);
    ED.RemoveListener(Events.Died, OnDied);
  }
}
