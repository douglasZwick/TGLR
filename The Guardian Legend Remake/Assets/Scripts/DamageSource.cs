using UnityEngine;


[RequireComponent(typeof(EventDispatcher))]
public class DamageSource : MonoBehaviour
{
  public EventDispatcher ED { get; private set; }

  [SerializeField]
  private DamageData m_DamageData;
  
  private DamageSource m_ProxyTarget;

  public DamageData Data => m_DamageData;


  void Awake()
  {
    ED = GetComponent<EventDispatcher>();
  }


  void OnEnable()
  {
    ED.AddListener(Events.DamageSetup, OnDamageSetup);
  }


  public void TryRequestDamage(GameObject target)
  {
    if (!target.TryGetComponent<EventDispatcher>(out var ed)) return;
    var healthED = CreateHealthEventData();
    ed.Dispatch(Events.DamageRequest, healthED);
  }


  public void ReturnDamage(DamageSource damageSource)
  {
    // CONSIDER:
    //   I don't expect that this function will ever be called in a context where this DamageSource uses Durability. As such, I feel it's safe to put the durability reduction code only in TryRequestDamage, rather than factoring it out and calling it here too.

    var healthED = CreateHealthEventData();
    damageSource.ED.Dispatch(Events.DamageRequest, healthED);
  }


  HealthEventData CreateHealthEventData()
  {
    var healthED = new HealthEventData()
    {
      m_Source = this,
      m_DamageData = m_DamageData,
    };

    return healthED;
  }


  void TryReduceDurability()
  {
    if (!Data.m_UseDurability) return;

    ReduceDurability();
  }


  void ReduceDurability()
  {
    // CONSIDER:
    //   From my current perspective, there's no reason that the durability should ever change by
    //   any amount other than 1
    --m_DamageData.m_Durability;

    if (m_DamageData.m_Durability > 0) return;

    Dispatch(Events.DurabilityExhausted, new DurabilityEventData());
  }


  void OnDamageSetup(HealthEventData healthED)
  {
    m_ProxyTarget = healthED.m_Source;
    m_DamageData = healthED.m_DamageData;
  }


  public void Dispatch<TData>(EventKey<TData> key, TData eventData)
  {
    ED.Dispatch(key, eventData);

    if (m_ProxyTarget == null) return;

    m_ProxyTarget.Dispatch(key, eventData);
  }


  void OnDisable()
  {
    ED.RemoveListener(Events.DamageSetup, OnDamageSetup);
  }
}
