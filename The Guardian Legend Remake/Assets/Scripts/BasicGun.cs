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

  [SerializeField]
  private Transform m_RotationNode;
  private InputAction m_PrimaryFireAction;
  private float m_CooldownTimer = float.PositiveInfinity;
  private HashSet<BulletCluster> m_CurrentClusters = new();
  private int m_PatternIndex = 0;

  private bool ClusterAvailable => m_CurrentClusters.Count < m_MaxClusters;
  private bool CoolingDown => m_CooldownTimer < m_CooldownDuration;

  public Events m_Events;


  void Awake()
  {
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
      // TODO:
      //   I think I SHOULD use transformation to convert these firing points, but I can't be
      //   arsed just now to figure out how, so I'm going to do it this more naive way for now

      // ALSO:
      //   This is gonna be extremely hard coded at first. I have to map the Y-axis rotation of the
      //   rotation node to the Z-axis rotation of the bullet. I'm gonna have to come up with a
      //   better long-term solution at some point, but I'll let it slide for now.

      var angle = m_RotationNode.localEulerAngles.y + firingPoint.m_Rotation.eulerAngles.z;
      var bulletWorldPosition = m_RotationNode.TransformPoint(firingPoint.m_Position);
      var rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
      var bullet = Instantiate(m_BulletPrefab, bulletWorldPosition, rotation);
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
