using UnityEngine;


[RequireComponent(typeof(EventDispatcher))]
public class CameraShakeOnDamage : MonoBehaviour
{
  public EventDispatcher ED { get; private set; }

  [SerializeField]
  EventChannel m_EventChannel;
  [SerializeField]
  float m_ShieldDamageTrauma = 0.5f;
  [SerializeField]
  float m_HpDamageTrauma = 1.0f;


  void Awake()
  {
    ED = GetComponent<EventDispatcher>();
  }


  void OnEnable()
  {
    ED.AddListener(Events.ShieldReceivedDamage, OnReceivedShieldDamage);
    ED.AddListener(Events.HpReceivedDamage, OnReceivedHpDamage);
  }


  void OnReceivedShieldDamage(HealthEventData healthED)
  {
    RequestShake(m_ShieldDamageTrauma);
  }


  void OnReceivedHpDamage(HealthEventData healthED)
  {
    RequestShake(m_HpDamageTrauma);
  }


  void RequestShake(float trauma)
  {
    var shakeED = new ShakeEventData()
    {
      m_Trauma = trauma,
    };

    m_EventChannel.Dispatch(Events.ShakeRequest, shakeED);
  }


  void OnDisable()
  {
    ED.RemoveListener(Events.ShieldReceivedDamage, OnReceivedShieldDamage);
    ED.RemoveListener(Events.HpReceivedDamage, OnReceivedHpDamage);
  }
}
