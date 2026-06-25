public class SetScrollSpeed : ScrollEventPayload
{
  public float m_Speed;


  public override void Execute()
  {
    ScrollController.SetSpeed(m_Speed);
  }
}
