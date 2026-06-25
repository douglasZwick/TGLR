using UnityEngine;


[RequireComponent(typeof(ScrollEventTrigger))]
abstract public class ScrollEventPayload : MonoBehaviour
{
  protected ScrollEventTrigger m_ScrollEventTrigger;

  protected ScrollController ScrollController => m_ScrollEventTrigger.m_ScrollController;


  void Awake()
  {
    m_ScrollEventTrigger = GetComponent<ScrollEventTrigger>();
    m_ScrollEventTrigger.RegisterPayload(this);
  }


  abstract public void Execute();
}
