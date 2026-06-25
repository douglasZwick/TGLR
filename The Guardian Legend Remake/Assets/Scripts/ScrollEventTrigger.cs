using System.Collections.Generic;
using UnityEngine;


public class ScrollEventTrigger : MonoBehaviour
{
  private readonly static string s_ScrollTag = "ScrollSentinel";

  public ScrollController m_ScrollController { get; private set; }

  private List<ScrollEventPayload> m_Payloads = new();


  void Awake()
  {
    m_ScrollController = GetComponentInParent<ScrollController>();
    if (!m_ScrollController)
      Debug.LogWarning("No ScrollController found");
  }


  public void RegisterPayload(ScrollEventPayload payload)
  {
    m_Payloads.Add(payload);
  }

  
  void OnTriggerEnter2D(Collider2D collision)
  {
    if (!collision.CompareTag(s_ScrollTag)) return;

    Execute();
  }


  void Execute()
  {
    foreach (var payload in m_Payloads)
      payload.Execute();
  }
}
