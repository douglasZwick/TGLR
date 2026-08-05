using UnityEngine;


[RequireComponent(typeof(EventDispatcher))]
[RequireComponent(typeof(Rigidbody2D))]
public class ClusterMember : MonoBehaviour
{
  public EventDispatcher ED { get; private set; }

  private BulletCluster m_Owner;


  void Awake()
  {
    ED = GetComponent<EventDispatcher>();
  }


  void OnEnable()
  {
    ED.AddListener(Events.WasFired, OnWasFired);
  }


  public void Setup(BulletCluster owner)
  {
    m_Owner = owner;
  }


  public void OnWasFired(FireEventData fireED)
  {
    var rb = GetComponent<Rigidbody2D>();
    rb.linearVelocity = fireED.m_Speed * transform.forward;
  }


  void OnDisable()
  {
    ED.AddListener(Events.WasFired, OnWasFired);
  }


  void OnDestroy()
  {
    m_Owner.MemberDestroyed(this);
  }
}
