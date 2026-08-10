using UnityEngine;
using UnityEngine.UI;
using TMPro;


[RequireComponent(typeof(RectTransform))]
public class BarGauge : MonoBehaviour
{
  // Value units per frame unit
  public static float s_UnitSize = 16;

  [SerializeField]
  EventChannel m_EventChannel;
  [SerializeField]
  RectTransform m_Bar;
  [SerializeField]
  RectTransform m_DeltaBar;
  [SerializeField]
  RectTransform m_Frame;
  [SerializeField]
  TMP_Text m_Label;
  [SerializeField]
  float m_BarColorAnimationDuration = 0.1f;
  [SerializeField]
  RectTransform m_FrameUnitPrefab;

  RectTransform m_RectTransform;
  Image m_DeltaBarImage;
  float m_BarColorAnimationTimer = -1;
  Color m_DeltaBarDefaultColor;
  float m_SetupTargetBarWidth;
  float m_SetupAnimationDuration;
  float m_SetupAnimationTimer = -1;

  float MeterWidth => m_RectTransform.rect.width;
  float FrameUnitWidth => m_FrameUnitPrefab.rect.width;
  bool BarColorAnimating => m_BarColorAnimationTimer > 0;
  Color DeltaBarClearColor => new(m_DeltaBarDefaultColor.r,
                                  m_DeltaBarDefaultColor.g,
                                  m_DeltaBarDefaultColor.b,
                                  0);
  bool SetupAnimating => m_SetupAnimationTimer >= 0;
  int FrameUnitCount => m_Frame.childCount;

  void Awake()
  {
    m_RectTransform = (RectTransform)transform;
    m_DeltaBarImage = m_DeltaBar.GetComponent<Image>();
    m_DeltaBarDefaultColor = m_DeltaBarImage.color;

    ResetFrame();
  }


  void OnEnable()
  {
    m_EventChannel.AddListener(Events.GaugeSetup, OnGaugeSetup);
    m_EventChannel.AddListener(Events.GaugeValueChanged, OnGaugeValueChanged);
    m_EventChannel.AddListener(Events.GaugeChangeStarted, OnGaugeChangeStarted);
    m_EventChannel.AddListener(Events.GaugeUpdate, OnGaugeUpdate);
    m_EventChannel.AddListener(Events.GaugeChangeEnded, OnGaugeChangeEnded);
  }


  void Update()
  {
    var dt = Time.deltaTime;

    if (BarColorAnimating)
      AnimateBarColor(dt);
    if (SetupAnimating)
      AnimateSetup(dt);
  }


  void ResetFrame()
  {
    m_Frame.DestroyAllChildren();
    SetBarLength(m_Bar, 0, 0);
    SetBarLength(m_DeltaBar, 0, 0);
  }


  void OnGaugeSetup(GaugeEventData gaugeED)
  {
    BeginSetupAnimation(gaugeED.m_MaxValue, gaugeED.m_AnimationDuration);
  }


  void OnGaugeValueChanged(GaugeEventData gaugeED)
  {
    var deltaBarValue = Mathf.Max(gaugeED.m_StartingValue, gaugeED.m_EndingValue);
    SetBarLength(m_DeltaBar, deltaBarValue, gaugeED.m_MaxValue);
    // TODO:
    //   When I get my action system in, use it to lerp the bar's width instead of snapping it
    SetBarLength(m_Bar, gaugeED.m_EndingValue, gaugeED.m_MaxValue);

    BeginAnimatingBarColor();
  }


  void OnGaugeChangeStarted(GaugeEventData gaugeED)
  {
    var deltaBarValue = Mathf.Max(gaugeED.m_StartingValue, gaugeED.m_EndingValue);
    SetBarLength(m_DeltaBar, deltaBarValue, gaugeED.m_MaxValue);
    // TODO:
    //   When I get my action system in, use it to lerp the bar's width instead of snapping it
    SetBarLength(m_Bar, gaugeED.m_EndingValue, gaugeED.m_MaxValue);
  }


  void OnGaugeUpdate(GaugeEventData gaugeED)
  {
    SetBarLength(m_DeltaBar, gaugeED.m_CurrentValue, gaugeED.m_MaxValue);
  }


  void OnGaugeChangeEnded(GaugeEventData gaugeED)
  {
    
  }


  void BeginAnimatingBarColor()
  {
    m_BarColorAnimationTimer = m_BarColorAnimationDuration;
    SetDeltaBarColor(m_DeltaBarDefaultColor);
  }


  void AnimateBarColor(float dt)
  {
    var t = m_BarColorAnimationTimer / m_BarColorAnimationDuration;
    SetDeltaBarColor(Color.Lerp(DeltaBarClearColor, m_DeltaBarDefaultColor, t));

    m_BarColorAnimationTimer -= dt;

    if (m_BarColorAnimationTimer <= 0)
      EndAnimatingBarColor();
  }


  void EndAnimatingBarColor()
  {
    SetDeltaBarColor(DeltaBarClearColor);
  }


  void SetDeltaBarColor(Color color)
  {
    m_DeltaBarImage.color = color;
  }


  void BeginSetupAnimation(float maxValue, float duration)
  {
    var targetFrameCount = (int)(maxValue / s_UnitSize);
    m_SetupTargetBarWidth = targetFrameCount * FrameUnitWidth;
    m_SetupAnimationDuration = duration;
    m_SetupAnimationTimer = 0;
  }


  void AnimateSetup(float dt)
  {
    var t = m_SetupAnimationTimer / m_SetupAnimationDuration;
    var width = m_SetupTargetBarWidth * t;
    var frameUnitSpan = (int)(width / FrameUnitWidth);

    if (frameUnitSpan + 1 > FrameUnitCount)
      AddFrameUnit();

    m_Bar.sizeDelta = Vector2.right * width;
    m_DeltaBar.sizeDelta = Vector2.right * frameUnitSpan * FrameUnitWidth;
    
    m_SetupAnimationTimer += dt;

    if (m_SetupAnimationTimer >= m_SetupAnimationDuration)
      EndSetupAnimation();
  }


  void EndSetupAnimation()
  {
    SetBarLength(m_Bar, 1, 1);
    SetBarLength(m_DeltaBar, 1, 1);
    m_SetupAnimationTimer = -1;
  }


  void SetBarLength(RectTransform bar, float current, float max)
  {
    // If max is 0, it's assumed that the intention is to set width to 0
    var width = max == 0 ? 0 : MeterWidth * current / max;
    bar.sizeDelta = Vector2.right * width;
  }


  void AddFrameUnit()
  {
    Instantiate(m_FrameUnitPrefab, m_Frame);
  }


  void OnDisable()
  {
    m_EventChannel.RemoveListener(Events.GaugeSetup, OnGaugeSetup);
    m_EventChannel.RemoveListener(Events.GaugeValueChanged, OnGaugeValueChanged);
    m_EventChannel.RemoveListener(Events.GaugeChangeStarted, OnGaugeChangeStarted);
    m_EventChannel.RemoveListener(Events.GaugeUpdate, OnGaugeUpdate);
    m_EventChannel.RemoveListener(Events.GaugeChangeEnded, OnGaugeChangeEnded);
  }
}
