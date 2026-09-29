using UnityEngine;


[RequireComponent(typeof(EventDispatcher))]
public class ProjectileWeapon : MonoBehaviour
{
  public EventDispatcher ED { get; private set; }

  [SerializeField]
  private Projectile m_ProjectilePrefab;
  [SerializeField]
  private DamageSource m_DamageSource;
  [SerializeField]
  private float m_ProjectileSpeed = 10;
  [SerializeField]
  private AudioItem m_ShootSound;


  protected virtual void Awake()
  {
    ED = GetComponent<EventDispatcher>();
  }


  void OnEnable()
  {
    ED.AddListener(Events.Activated, OnActivated);
  }


  void OnActivated(SpecialWeaponEventData eventData)
  {
    Fire(eventData.m_FiringPoint);
  }


  void Fire(Transform firingPoint)
  {
    // Consider dispatching an event that says I fired

    var projectile = Instantiate(m_ProjectilePrefab, firingPoint.position, firingPoint.rotation);

    var healthED = new HealthEventData()
    {
      m_Source = m_DamageSource,
      m_DamageData = m_DamageSource.Data,
    };

    var projectileED = new ProjectileEventData()
    {
      m_Speed = m_ProjectileSpeed,
    };

    projectile.ED.Dispatch(Events.DamageSetup, healthED);
    projectile.ED.Dispatch(Events.ProjectileSetup, projectileED);

    PlayShootSound();

    // Figure out what to do about firing limits
  }


  void PlayShootSound()
  {
    if (m_ShootSound == null) return;

    AudioManager.Instance.Play(m_ShootSound);
  }


  void OnDisable()
  {
    ED.RemoveListener(Events.Activated, OnActivated);
  }
}
