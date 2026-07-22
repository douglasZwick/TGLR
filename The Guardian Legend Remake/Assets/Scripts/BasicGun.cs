using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

  private readonly static string s_FiringGroupTag = "FiringGroup";
  private readonly static string s_FiringPointTag = "FiringPoint";

  public float m_BulletSpeed = 10;
  public ClusterMember m_BulletPrefab;
  public float m_CooldownDuration = 0.25f;
  public int m_MaxClusters = 4;

  [SerializeField]
  private Transform m_GunBaseNode;
  private InputAction m_PrimaryFireAction;
  private float m_CooldownTimer = float.PositiveInfinity;
  private HashSet<BulletCluster> m_CurrentClusters = new();
  private List<List<Transform>> m_FiringPattern = new();
  private int m_PatternIndex = 0;

  private bool ClusterAvailable => m_CurrentClusters.Count < m_MaxClusters;
  private bool CoolingDown => m_CooldownTimer < m_CooldownDuration;

  public Events m_Events;


  void Awake()
  {
    var playerInput = GetComponent<PlayerInput>();
    m_PrimaryFireAction = playerInput.actions.FindAction("PrimaryFire");

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
    var groups = m_GunBaseNode.Cast<Transform>()
      .Where(child => child.CompareTag(s_FiringGroupTag));
    foreach (var parent in groups)
    {
      var group = parent.Cast<Transform>().Where(child => child.CompareTag(s_FiringPointTag));
      m_FiringPattern.Add(group.ToList());
    }

    // foreach (Transform child in m_GunBaseNode)
    // {
    //   if (child.CompareTag(s_FiringGroupTag))
    //   {
    //     var firingGroup = new List<Transform>();

    //     foreach (Transform grandchild in child)
    //     {
    //       if (grandchild.CompareTag(s_FiringPointTag))
    //       {
    //         firingGroup.Add(grandchild);
    //       }
    //     }

    //     m_FiringPattern.Add(firingGroup);
    //   }
    // }
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
      var bullet = Instantiate(m_BulletPrefab, firingPoint.position, firingPoint.rotation);
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
