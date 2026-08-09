using System;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(RectTransform))]
public class BarGauge : MonoBehaviour
{
  [SerializeField]
  EventChannel m_EventChannel;
  [SerializeField]
  RectTransform m_Bar;
  [SerializeField]
  RectTransform m_DeltaBar;
  [SerializeField]
  float m_AnimationDuration = 0.1f;

  RectTransform m_RectTransform;
  Image m_DeltaBarImage;
  float m_AnimationTimer = 0;
  Color m_DeltaBarDefaultColor;

  float MeterWidth => m_RectTransform.rect.width;
  bool Animating => m_AnimationTimer > 0;
  Color DeltaBarClearColor =>
    new Color(m_DeltaBarDefaultColor.r,
              m_DeltaBarDefaultColor.g,
              m_DeltaBarDefaultColor.b,
              0);


  void Awake()
  {
    m_RectTransform = (RectTransform)transform;
    m_DeltaBarImage = m_DeltaBar.GetComponent<Image>();
    m_DeltaBarDefaultColor = m_DeltaBarImage.color;
  }


  void OnEnable()
  {
    m_EventChannel.AddListener(Events.GaugeValueChanged, OnGaugeValueChanged);
    m_EventChannel.AddListener(Events.GaugeChangeStarted, OnGaugeChangeStarted);
    m_EventChannel.AddListener(Events.GaugeUpdate, OnGaugeUpdate);
    m_EventChannel.AddListener(Events.GaugeChangeEnded, OnGaugeChangeEnded);
  }


  void Update()
  {
    if (Animating)
      Animate(Time.deltaTime);
  }


  void OnGaugeValueChanged(GaugeEventData gaugeED)
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

    BeginAnimating();
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


  void BeginAnimating()
  {
    m_AnimationTimer = m_AnimationDuration;
    SetDeltaBarColor(m_DeltaBarDefaultColor);
  }


  void Animate(float dt)
  {
    var t = m_AnimationTimer / m_AnimationDuration;
    SetDeltaBarColor(Color.Lerp(DeltaBarClearColor, m_DeltaBarDefaultColor, t));

    m_AnimationTimer -= dt;

    if (m_AnimationTimer <= 0)
      EndAnimating();
  }


  void EndAnimating()
  {
    SetDeltaBarColor(DeltaBarClearColor);
  }


  void SetDeltaBarColor(Color color)
  {
    m_DeltaBarImage.color = color;
  }


  void OnDisable()
  {
    m_EventChannel.RemoveListener(Events.GaugeValueChanged, OnGaugeValueChanged);
    m_EventChannel.RemoveListener(Events.GaugeChangeStarted, OnGaugeChangeStarted);
    m_EventChannel.RemoveListener(Events.GaugeUpdate, OnGaugeUpdate);
    m_EventChannel.RemoveListener(Events.GaugeChangeEnded, OnGaugeChangeEnded);
  }
}
