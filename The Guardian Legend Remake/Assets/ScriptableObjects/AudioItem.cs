using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioItem", menuName = "Audio/AudioItem")]
public class AudioItem : ScriptableObject
{
  public enum SelectionStrategyOptions
  {
    ByIndex,
    Sequential,
    Random,
  }

  public enum OverlapStrategyOptions
  {
    Interrupt,
    Overlap,
    Fail,
    // Limit,
    // Queue,
  }

  [SerializeField]
  private List<AudioClip> m_Clips = new();
  [SerializeField][Range(min: 0, max: 1)]
  private float m_Volume = 1;
  [SerializeField]
  private OverlapStrategyOptions m_OverlapStrategy = OverlapStrategyOptions.Interrupt;
  // [SerializeField, Min(1)]
  // private int m_InstanceLimit = 1;
  [SerializeField]
  private SelectionStrategyOptions m_SelectionStrategy = SelectionStrategyOptions.Sequential;
  [SerializeField]
  // Used for SelectionStrategyOptions.ByIndex only; ItemState has its own index for Sequential
  private int m_Index = 0;

  public IReadOnlyList<AudioClip> Clips => m_Clips;
  public float Volume => m_Volume;
  public SelectionStrategyOptions SelectionStrategy => m_SelectionStrategy;
  public OverlapStrategyOptions OverlapStrategy => m_OverlapStrategy;
  public int Index => m_Index;
  // public int InstanceLimit => m_InstanceLimit;


  public void SetIndex(int newIndex)
  {
    m_Index = PositiveModulo(newIndex, m_Clips.Count);
  }


  void OnValidate()
  {
    if (m_Clips == null || m_Clips.Count <= 0)
    {
      m_Index = 0;
      return;
    }

    SetIndex(m_Index);
  }


  static int PositiveModulo(int n, int b) => (n % b + b) % b;
}
