using UnityEngine;


[RequireComponent(typeof(EventDispatcher))]
public class BoundsChecker2D : MonoBehaviour
{
  public EventDispatcher ED { get; private set; }

  [SerializeField]
  private UpdateType m_UpdateType = UpdateType.None;
  
  public Margin m_Margin;

  private Transform m_Tx;
  private CameraBounds2D m_Bounds;
  private bool m_PreviousInBounds = false;


  void Awake()
  {
    ED = GetComponent<EventDispatcher>();

    m_Tx = transform;

    // CONDITIONAL TODO:
    //   If I ever use multiple cameras, this shouldn't necessarily use caching
    m_Bounds = Camera.main.GetComponent<CameraBounds2D>();

    if (!m_Bounds)
      Debug.LogWarning("No CameraBounds2D found");
  }


  void Update()
  {
    if (m_UpdateType != UpdateType.Update) return;

    CheckBounds();
  }


  void FixedUpdate()
  {
    if (m_UpdateType != UpdateType.FixedUpdate) return;
    
    CheckBounds();
  }


  public void OnMoved(MovementEventData movementED)
  {
    CheckBounds();
  }


  public Vector2 ComputeCorrectedPosition(Vector2 desiredPosition)
  {
    return m_Bounds.ComputeCorrectedPosition(desiredPosition, m_Margin);
  }


  void CheckBounds()
  {
    var position = (Vector2)m_Tx.position;
    var corrected = ComputeCorrectedPosition(position);
    var inBounds = position == corrected;

    var boundsED = new BoundsEventData()
    {
      m_WorldPosition = position,
      m_Resolution = corrected - position,
    };

    if (inBounds)
    {
      ED.Dispatch(Events.BoundsIn, boundsED);

      if (!m_PreviousInBounds)
        ED.Dispatch(Events.BoundsEnter, boundsED);
    }
    else
    {
      ED.Dispatch(Events.BoundsOut, boundsED);

      if (m_PreviousInBounds)
        ED.Dispatch(Events.BoundsOut, boundsED);
    }

    m_PreviousInBounds = inBounds;
  }
}
