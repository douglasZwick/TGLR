using System.Collections.Generic;
using UnityEngine;

public class SsHeroController : MonoBehaviour
{
  public float m_MovementSpeed = 10.0f;
  public Vector2 m_Bounds = new Vector2(10.0f, 10.0f);
  public GameObject m_BulletPrefab;
  public float m_BasicGunCooldown = 0.25f;
  public int m_MaxBullets = 4;
  public Vector3 m_BasicGunLocalOffset = new Vector3(0.0f, 1.0f, 0.0f);  // TODO: make this a separate game object

  Transform m_Tx;
  Rigidbody m_RB;
  Vector2 m_CurrentStickInput = Vector2.zero;
  float m_CooldownTimer = 0;
  bool CoolingDown { get { return m_CooldownTimer < m_BasicGunCooldown; } }
  bool BulletsAvailable { get { return m_Bullets.Count < m_MaxBullets; } }
  List<GameObject> m_Bullets = new List<GameObject>();


  void Start()
  {
    m_Tx = transform;
    m_RB = GetComponent<Rigidbody>();
  }


  void Update()
  {
    HandleMovementInput();
    HandleFiring();
    ClampPosition();
    UpdateCooldown();
    RemoveDeadBullets();
  }


  private void FixedUpdate()
  {
    m_RB.velocity = m_CurrentStickInput * m_MovementSpeed;
  }


  void HandleMovementInput()
  {
    var x = Input.GetAxis("Horizontal");
    var y = Input.GetAxis("Vertical");
    m_CurrentStickInput = new Vector2(x, y);
  }


  void HandleFiring()
  {
    if (Input.GetButton("Fire1"))
    {
      AttemptFire();
    }
  }


  void ClampPosition()
  {
    var position = m_Tx.position;
    position.x = Mathf.Clamp(position.x, -m_Bounds.x, m_Bounds.x);
    position.y = Mathf.Clamp(position.y, -m_Bounds.y, m_Bounds.y);
    m_Tx.position = position;
  }


  void AttemptFire()
  {
    if (!CoolingDown && BulletsAvailable)
      Fire();
  }


  void Fire()
  {
    ResetCooldown();

    var bulletWorldPosition = m_Tx.TransformPoint(m_BasicGunLocalOffset);
    var bullet = Instantiate(m_BulletPrefab, bulletWorldPosition, Quaternion.identity);
    m_Bullets.Add(bullet);
  }


  void UpdateCooldown()
  {
    m_CooldownTimer += Time.deltaTime;
  }


  void ResetCooldown()
  {
    m_CooldownTimer = 0;
  }


  void RemoveDeadBullets()
  {
    var newList = new List<GameObject>();

    foreach (var bullet in m_Bullets)
      if (bullet != null)
        newList.Add(bullet);

    m_Bullets = newList;
  }
}
