using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class ClusterMember : MonoBehaviour
{
  [System.Serializable]
  public class Events
  {
    public FireEvent WasFired;
  }

  private BulletCluster m_Owner;

  public Events m_Events;


  void Awake()
  {
    m_Events.WasFired.AddListener(OnWasFired);
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


  void OnDestroy()
  {
    m_Owner.MemberDestroyed(this);
  }
}
