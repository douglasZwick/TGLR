public class SetScrollAcceleration : ScrollEventPayload
{
  public float m_Acceleration;
  public float m_TargetSpeed;  


  public override void Execute()
  {
    ScrollController.SetAcceleration(m_Acceleration, m_TargetSpeed);
  }
}
