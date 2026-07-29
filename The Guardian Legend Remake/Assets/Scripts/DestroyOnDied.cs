using UnityEngine;


public class DestroyOnDied : MonoBehaviour
{
  [SerializeField]
  private Health m_EventSource;
  [SerializeField]
  private GameObject m_DestructionTarget;


  void Awake()
  {
    if (m_EventSource == null)
      m_EventSource = GetComponent<Health>();

    if (m_EventSource == null)
    {
      Zbug.Warn($"{this} has no event receiver. Did you forget to hook it up in the inspector?");
      return;
    }

    m_EventSource.m_Events.Died.AddListener(OnDied);

    if (m_DestructionTarget == null)
      m_DestructionTarget = gameObject;
  }


  void OnEnable()
  {
    // CONSIDER:
    //   Seems doubtful that it would happen, but if performance really calls for it, I could get
    //   rid of the null check here and in OnDisable by treating a missing event source as an error
    //   and letting it throw here (or maybe better yet, I could manually throw in Awake)
    if (m_EventSource == null) return;
    m_EventSource.m_Events.Died.AddListener(OnDied);
  }


  void OnDisable()
  {
    if (m_EventSource == null) return;
    m_EventSource.m_Events.Died.RemoveListener(OnDied);
  }


  public void OnDied(HealthEventData healthED)
  {
    DestroyThisObject();
  }


  void DestroyThisObject()
  {
    Destroy(m_DestructionTarget);
  }
}
