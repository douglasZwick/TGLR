using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
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
    m_RB = GetComponent<Rigidbody2D>();
  }


  public void Setup(float? speed = null)
  {
    var actualSpeed = speed ?? m_DefaultSpeed;
    m_RB.linearVelocity = actualSpeed * Forward;
  }
}
