using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
  public Orientation2D m_Orientation = Orientation2D.YForward;

  private Rigidbody2D m_RB;
  [SerializeField]
  private float m_DefaultSpeed = 10;

  private Vector2 Forward => m_Orientation switch
  {
    Orientation2D.XForward => transform.right,
    _                      => transform.up,
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
