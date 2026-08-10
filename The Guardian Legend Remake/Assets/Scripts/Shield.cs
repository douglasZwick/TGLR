using UnityEngine;


[RequireComponent(typeof(EventDispatcher))]
public abstract class Shield : MonoBehaviour
{
  public EventDispatcher ED { get; private set; }

  protected float m_Energy;
  [SerializeField]
  protected float m_EnergyMax = 64;

  protected bool Full => m_Energy >= m_EnergyMax;
  protected bool Empty => m_Energy <= 0;


  protected virtual void Awake()
  {
    ED = GetComponent<EventDispatcher>();

    m_Energy = m_EnergyMax;
  }


  void OnEnable()
  {
    ED.AddListener(Events.DamagePreProcess, OnDamagePreProcess);
  }


  void Start()
  {
    var healthED = new HealthEventData()
    {
      m_EnergyMax = m_EnergyMax,
    };
    ED.Dispatch(Events.ShieldSetup, healthED);
  }


  private void OnDamagePreProcess(HealthEventData healthED)
  {
    if (Empty) return;

    healthED.m_EnergyMax = m_EnergyMax;

    ReceiveDamage(healthED);

    var penetratingDamage = Mathf.Min(healthED.m_IncomingHealthDamage, healthED.m_Penetration);
    healthED.m_IncomingHealthDamage = penetratingDamage;

    // CONSIDER:
    //   Maybe try a system with a threshold where, if the shields fall below it, some damage gets
    //   through to HP
  }


  protected abstract void ReceiveDamage(HealthEventData healthED);
  protected abstract void ReceiveHeal(HealthEventData healthED);


  protected void Deplete(HealthEventData healthED)
  {
    ED.Dispatch(Events.ShieldDepleted, healthED);
    // Dispatch DepletedShield here if I ever make that
  }


  protected void Fill(HealthEventData healthED)
  {
    ED.Dispatch(Events.ShieldFilled, healthED);
    // Dispatch FilledShield here if I ever make that
  }


  void OnDisable()
  {
    ED.AddListener(Events.DamagePreProcess, OnDamagePreProcess);
  }
}
