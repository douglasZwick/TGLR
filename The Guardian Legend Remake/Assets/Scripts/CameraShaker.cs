using UnityEngine;
using UnityEngine.InputSystem;


public class CameraShaker : MonoBehaviour
{
  [SerializeField]
  private Transform m_ShakeNode;
  [SerializeField]
  // How much trauma should be removed per second
  float m_TraumaReductionRate = 1.0f;
  [SerializeField]
  float m_NoiseFrequency = 5.0f;
  [SerializeField]
  float m_TraumaExponent = 2.5f;
  [SerializeField]
  float m_MaxTrauma = 1.0f;
  [SerializeField]
  // How far the camera should translate at max trauma
  float m_MaxTranslationalAmplitude = 1.0f;
  [SerializeField]
  // How far the camera should rotate at max trauma
  float m_MaxRotationalAmplitude = 10.0f;
  [SerializeField]
  float m_RBand = 0;
  [SerializeField]
  float m_ThetaBand = 100;
  [SerializeField]
  float m_RotationBand = 200;

  private float m_Trauma;
  private float m_Timer = 0;

  public bool Shaking => m_Trauma > 0;
  public float TranslationalAmplitude =>
    Mathf.Pow(m_Trauma, m_TraumaExponent) * m_MaxTranslationalAmplitude;
  public float RotationalAmplitude =>
    Mathf.Pow(m_Trauma, m_TraumaExponent) * m_MaxRotationalAmplitude;


  void Awake()
  {
    if (m_ShakeNode == null)
      m_ShakeNode = transform;
  }


  void Update()
  {
    var dt = Time.deltaTime;

    if (Shaking)
      Shake(dt);

    m_Timer += dt * m_NoiseFrequency;
  }


  public void BeginShaking(float trauma)
  {
    AddTrauma(trauma);
  }


  private void Shake(float dt)
  {
    var randomPoint = (Vector3)GetRandomPointInCircle(TranslationalAmplitude, m_Timer);
    var randomAngle = GetRandomAngle(RotationalAmplitude, m_Timer);

    m_ShakeNode.localPosition = randomPoint;
    m_ShakeNode.localEulerAngles = Vector3.forward * randomAngle;

    m_Trauma -= dt * m_TraumaReductionRate;

    if (m_Trauma < 0)
      EndShaking();
  }


  private void EndShaking()
  {
    m_Trauma = 0;
    m_ShakeNode.localPosition = Vector3.zero;
    m_ShakeNode.localEulerAngles = Vector3.zero;

    // If I want to do anything on shake ended, then I should
    // create an event for that and invoke it here
  }


  private void AddTrauma(float trauma) => m_Trauma = Mathf.Min(m_Trauma + trauma, m_MaxTrauma);


  private Vector2 GetRandomPointInCircle(float amplitude, float time)
  {
    var r = amplitude * Mathf.Sqrt(GetRandom01(time, m_RBand));
    var theta = 2 * Mathf.PI * (2 * GetRandom01(time, m_ThetaBand) - 1);
    var x = r * Mathf.Cos(theta);
    var y = r * Mathf.Sin(theta);

    return new Vector2(x, y);
  }


  private float GetRandomAngle(float amplitude, float time)
  {
    return 2 * amplitude * GetRandom01(time, m_RotationBand) - amplitude;
  }


  private float GetRandom01(float time, float band)
  {
    return Mathf.PerlinNoise(time, band);
  }
}
