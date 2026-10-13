using TMPro;
using UnityEngine;


public class TextDisplay : MonoBehaviour
{
  [SerializeField]
  private EventChannel m_EventChannel;
  [SerializeField]
  private TMP_Text m_DisplayText;
  

  void OnEnable()
  {
    m_EventChannel.AddListener(Events.GaugeSetup, OnGaugeSetup);
    m_EventChannel.AddListener(Events.GaugeValueChanged, OnGaugeValueChanged);
  }


  void OnGaugeSetup(GaugeEventData eventData)
  {
    SetNumber(eventData.m_CurrentValue);
  }


  void OnGaugeValueChanged(GaugeEventData eventData)
  {
    SetNumber(eventData.m_EndingValue);
  }


  void SetNumber(float number)
  {
    m_DisplayText.text = ((int)number).ToString();
  }


  void OnDisable()
  {
    m_EventChannel.RemoveListener(Events.GaugeSetup, OnGaugeSetup);
    m_EventChannel.RemoveListener(Events.GaugeValueChanged, OnGaugeValueChanged);
  }
}
