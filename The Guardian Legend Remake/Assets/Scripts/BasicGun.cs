using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(PlayerInput))]
public class BasicGun : MonoBehaviour
{
  [System.Serializable]
  public class Events
  {
    public FireEvent DidFire;
  }

  public float m_BulletSpeed = 10;
  public ClusterMember m_BulletPrefab;
  public float m_CooldownDuration = 0.25f;
  public int m_MaxClusters = 4;
  public List<ClusterPattern> m_FiringPattern;

  private Transform m_Tx;
  private InputAction m_PrimaryFireAction;
  private float m_CooldownTimer = float.PositiveInfinity;
  private HashSet<BulletCluster> m_CurrentClusters = new();
  private int m_PatternIndex = 0;

  private bool ClusterAvailable => m_CurrentClusters.Count < m_MaxClusters;
  private bool CoolingDown => m_CooldownTimer < m_CooldownDuration;

  public Events m_Events;


  void Awake()
  {
    m_Tx = transform;
    
    var playerInput = GetComponent<PlayerInput>();
    m_PrimaryFireAction = playerInput.actions.FindAction("PrimaryFire");
  }


  void Update()
  {
    if (m_PrimaryFireAction.IsPressed())
      AttemptFire();

    CoolDown(Time.deltaTime);
  }


  void BeginCooldown()
  {
    m_CooldownTimer = 0;
  }


  void CoolDown(float dt)
  {
    m_CooldownTimer += dt;
  }


  void AttemptFire()
  {
    if (CoolingDown) return;
    if (!ClusterAvailable) return;

    Fire();
  }


  void Fire()
  {
    var fireED = new FireEventData();

    m_Events.DidFire.Invoke(fireED);

    var firingPoints = m_FiringPattern[m_PatternIndex];
    var cluster = new BulletCluster(DestroyCluster);

    foreach (var firingPoint in firingPoints)
    {
      var position = m_Tx.TransformPoint(firingPoint.m_Position);
      var rotation = m_Tx.rotation * firingPoint.m_Rotation;
      var bullet = Instantiate(m_BulletPrefab, position, rotation);
      cluster.Add(bullet);
      var projectile = bullet.GetComponent<Projectile>();
      projectile.Setup(m_BulletSpeed);
    }

    m_CurrentClusters.Add(cluster);

    NextPatternIndex();
    BeginCooldown();
  }


  void NextPatternIndex() => m_PatternIndex = (m_PatternIndex + 1) % m_FiringPattern.Count;


  void DestroyCluster(BulletCluster cluster)
  {
    m_CurrentClusters.Remove(cluster);
  }
}
