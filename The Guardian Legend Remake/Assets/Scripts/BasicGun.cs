using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(EventDispatcher))]
[RequireComponent(typeof(PlayerInput))]
public class BasicGun : MonoBehaviour
{
  public EventDispatcher ED { get; private set; }

  private readonly static string s_FiringGroupTag = "FiringGroup";
  private readonly static string s_FiringPointTag = "FiringPoint";

  public float m_BulletSpeed = 10;
  public ClusterMember m_BulletPrefab;
  public float m_CooldownDuration = 0.25f;
  public int m_MaxClusters = 4;
  [SerializeField]
  private AudioItem m_ShootSound;

  [SerializeField]
  private Transform m_GunProxyNode;
  private DamageSource m_GunDamageSource;
  private InputAction m_PrimaryFireAction;
  private float m_CooldownTimer = float.PositiveInfinity;
  private HashSet<BulletCluster> m_CurrentClusters = new();
  private List<List<Transform>> m_FiringPattern = new();
  private int m_PatternIndex = 0;

  private bool ClusterAvailable => m_CurrentClusters.Count < m_MaxClusters;
  private bool CoolingDown => m_CooldownTimer < m_CooldownDuration;


  void Awake()
  {
    ED = GetComponent<EventDispatcher>();

    var playerInput = GetComponent<PlayerInput>();
    m_PrimaryFireAction = playerInput.actions.FindAction("PrimaryFire");

    m_GunDamageSource = m_GunProxyNode.GetComponent<DamageSource>();

    InitializeClusterPatterns();
  }


  void Update()
  {
    if (m_PrimaryFireAction.IsPressed())
      AttemptFire();

    CoolDown(Time.deltaTime);
  }


  void InitializeClusterPatterns()
  {
    var groups = m_GunProxyNode.Cast<Transform>()
      .Where(child => child.CompareTag(s_FiringGroupTag));
      
    foreach (var parent in groups)
    {
      var group = parent.Cast<Transform>().Where(child => child.CompareTag(s_FiringPointTag));
      m_FiringPattern.Add(group.ToList());
    }
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
    ED.Dispatch(Events.DidFire, fireED);

    var firingPoints = m_FiringPattern[m_PatternIndex];
    var cluster = new BulletCluster(DestroyCluster);

    foreach (var firingPoint in firingPoints)
    {
      var bullet = Instantiate(m_BulletPrefab, firingPoint.position, firingPoint.rotation);
      cluster.Add(bullet);

      var healthED = new HealthEventData()
      {
        m_Source = m_GunDamageSource,
        m_DamageData = m_GunDamageSource.Data,
      };
      
      var projectileED = new ProjectileEventData()
      {
        m_Speed = m_BulletSpeed,
      };

      bullet.ED.Dispatch(Events.DamageSetup, healthED);
      bullet.ED.Dispatch(Events.ProjectileSetup, projectileED);
    }

    AudioManager.Instance.Play(m_ShootSound);

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
