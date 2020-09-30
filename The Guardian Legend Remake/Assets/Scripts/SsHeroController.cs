using System.Collections.Generic;
using UnityEngine;

public class SsHeroController : MonoBehaviour
{
  public float m_MovementSpeed = 10.0f;
  public Vector2 m_Bounds = new Vector2(10.0f, 10.0f);
  public HeroGun m_Gun;

  Transform m_Tx;
  Rigidbody m_RB;
  Vector2 m_CurrentStickInput = Vector2.zero;


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
    // Assumes you always have a gun
    if (Input.GetButton("Fire1"))
      m_Gun.AttemptFire();
  }


  void ClampPosition()
  {
    var position = m_Tx.position;
    position.x = Mathf.Clamp(position.x, -m_Bounds.x, m_Bounds.x);
    position.y = Mathf.Clamp(position.y, -m_Bounds.y, m_Bounds.y);
    m_Tx.position = position;
  }
}


public enum HeroForm
{
  Humanoid,
  Jet,
}
