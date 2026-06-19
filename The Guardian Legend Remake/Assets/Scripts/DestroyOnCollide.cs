using UnityEngine;


public class DestroyOnCollide : MonoBehaviour
{
  public bool m_DestroyOnCollisionEnter2D;
  public bool m_DestroyOnTriggerEnter2D;
  public bool m_DestroyOnCollisionEnter3D;
  public bool m_DestroyOnTriggerEnter3D;


  void OnCollisionEnter2D(Collision2D collision)
  {
    if (!m_DestroyOnCollisionEnter2D) return;

    DestroyThisObject();
  }

  void OnTriggerEnter2D(Collider2D collision)
  {
    if (!m_DestroyOnTriggerEnter2D) return;

    DestroyThisObject();
  }

  void OnCollisionEnter(Collision collision)
  {
    if (!m_DestroyOnCollisionEnter3D) return;

    DestroyThisObject();
  }

  void OnTriggerEnter(Collider other)
  {
    if (!m_DestroyOnTriggerEnter3D) return;

    DestroyThisObject();
  }


  void DestroyThisObject()
  {
    Destroy(gameObject);
  }
}
