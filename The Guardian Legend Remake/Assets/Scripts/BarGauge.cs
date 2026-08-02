using UnityEngine;


[RequireComponent(typeof(RectTransform))]
public class BarGauge : MonoBehaviour
{
  [SerializeField]
  EventChannel m_Dispatcher;
  [SerializeField]
  RectTransform m_Bar;
  [SerializeField]
  RectTransform m_DeltaBar;

  RectTransform m_RectTransform;

  float MeterWidth => m_RectTransform.rect.width;


  void Awake()
  {
    m_RectTransform = (RectTransform)transform;
  }


  void OnEnable()
  {
    m_Dispatcher.AddListener(GaugeEvents.GaugeChangeStarted, OnGaugeChangeStarted);
    m_Dispatcher.AddListener(GaugeEvents.GaugeUpdate, OnGaugeUpdate);
    m_Dispatcher.AddListener(GaugeEvents.GaugeChangeEnded, OnGaugeChangeEnded);
  }


  void OnGaugeChangeStarted(GaugeEventData gaugeED)
  {
    var deltaBarValue = Mathf.Max(gaugeED.m_StartingValue, gaugeED.m_EndingValue);
    var deltaBarFraction = deltaBarValue / gaugeED.m_MaxValue;
    var deltaBarWidth = deltaBarFraction * MeterWidth;
    m_DeltaBar.sizeDelta = Vector2.right * deltaBarWidth;

    var barFraction = gaugeED.m_EndingValue / gaugeED.m_MaxValue;
    var barWidth = barFraction * MeterWidth;
    // TODO:
    //   When I get my action system in, use it to lerp the bar's width instead of snapping it
    m_Bar.sizeDelta = Vector2.right * barWidth;
  }


  void OnGaugeUpdate(GaugeEventData gaugeED)
  {
    var barFraction = gaugeED.m_CurrentValue / gaugeED.m_MaxValue;
    var barWidth = barFraction * MeterWidth;
    m_DeltaBar.sizeDelta = Vector2.right * barWidth;
  }


  void OnGaugeChangeEnded(GaugeEventData gaugeED)
  {
    
  }


  void OnDisable()
  {
    m_Dispatcher.RemoveListener(GaugeEvents.GaugeChangeStarted, OnGaugeChangeStarted);
    m_Dispatcher.RemoveListener(GaugeEvents.GaugeUpdate, OnGaugeUpdate);
    m_Dispatcher.RemoveListener(GaugeEvents.GaugeChangeEnded, OnGaugeChangeEnded);
  }
}
