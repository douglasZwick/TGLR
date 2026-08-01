using UnityEngine;


[ExecuteAlways]
public class CameraViewportRectSetup : MonoBehaviour
{
  [SerializeField]
  Camera m_Camera;
  [SerializeField]
  RectTransform m_Hud;
  [SerializeField]
  Canvas m_Canvas;


  void Update()
  {
    UpdateViewport();
  }


  void UpdateViewport()
  {
    if (m_Camera == null || m_Hud == null || m_Canvas == null) return;

    Canvas.ForceUpdateCanvases();

    var hudPixelHeight = m_Hud.rect.height * m_Canvas.scaleFactor;
    float hudFraction = hudPixelHeight / Screen.height;

    m_Camera.rect = new Rect(
      0, hudFraction,
      1, 1 - hudFraction);
  }
}
