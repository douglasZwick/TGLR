using UnityEngine;


[RequireComponent(typeof(Health))]
public class CameraShakeOnDamage : MonoBehaviour
{
  [SerializeField]
  EventChannel m_Dispatcher;
  [SerializeField]
  float m_ShieldDamageTrauma = 0.5f;
  [SerializeField]
  float m_HpDamageTrauma = 1.0f;

  Health m_Health;


  void OnEnable()
  {
    m_Health = GetComponent<Health>();
    m_Health.m_Events.ShieldReceivedDamage.AddListener(OnReceivedShieldDamage);
    m_Health.m_Events.HpReceivedDamage.AddListener(OnReceivedHpDamage);
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

    m_Dispatcher.Dispatch(ShakeEvents.ShakeRequest, shakeED);
  }


  void OnDisable()
  {
    m_Health.m_Events.ShieldReceivedDamage.RemoveListener(OnReceivedShieldDamage);
    m_Health.m_Events.HpReceivedDamage.RemoveListener(OnReceivedHpDamage);
  }
}
