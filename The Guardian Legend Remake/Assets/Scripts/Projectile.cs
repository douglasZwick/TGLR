using UnityEngine;


[RequireComponent(typeof(EventDispatcher))]
[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
  public EventDispatcher ED { get; private set; }

  public Orientation3D m_Orientation = Orientation3D.PosXForward;

  private Rigidbody2D m_RB;
  [SerializeField]
  private float m_DefaultSpeed = 10;

  private Vector2 Forward => m_Orientation switch
  {
    Orientation3D.PosXForward => transform.right,
    Orientation3D.NegXForward => -transform.right,
    Orientation3D.PosYForward => transform.up,
    Orientation3D.NegYForward => -transform.up,
    Orientation3D.PosZForward => transform.forward,
    _                         => -transform.forward,
  };


  void Awake()
  {
    ED = GetComponent<EventDispatcher>();
    m_RB = GetComponent<Rigidbody2D>();

    SetVelocity(m_DefaultSpeed);
  }


  void OnEnable()
  {
    ED.AddListener(Events.ProjectileSetup, OnProjectileSetup);
  }


  void OnProjectileSetup(ProjectileEventData projectileED)
  {
    SetVelocity(projectileED.m_Speed);
  }

  
  void SetVelocity(float speed)
  {
    m_RB.linearVelocity = speed * Forward;
  }


  void OnDisable()
  {
    ED.RemoveListener(Events.ProjectileSetup, OnProjectileSetup);
  }
}
