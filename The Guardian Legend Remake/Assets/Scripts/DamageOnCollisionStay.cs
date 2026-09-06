using UnityEngine;


[RequireComponent(typeof(EventDispatcher))]
[RequireComponent(typeof(DamageSource))]
public class DamageOnCollisionStay : MonoBehaviour
{
  // How long, in seconds, between each damage attempt
  public static readonly float s_DelayDuration = 0.25f;
  // TODO:
  //   Replace this with mercy invincibility on Health

  public EventDispatcher ED { get; private set; }

  private DamageSource m_DamageSource;
  private float m_Timer = s_DelayDuration;
  
  private bool Ready => m_Timer >= s_DelayDuration;


  protected virtual void Awake()
  {
    ED = GetComponent<EventDispatcher>();
    m_DamageSource = GetComponent<DamageSource>();
  }


  void Update()
  {
    m_Timer += Time.deltaTime;
  }


  void OnTriggerStay2D(Collider2D collision)
  {
    if (Ready)
    {
      AttemptDamage(collision.gameObject);
      m_Timer = 0;
    }
  }


  void AttemptDamage(GameObject other)
  {
    m_DamageSource.TryRequestDamage(other);
  }
}
