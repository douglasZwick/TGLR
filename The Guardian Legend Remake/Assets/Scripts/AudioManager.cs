using System.Collections.Generic;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
  static public AudioManager Instance { get; private set; }


  // CONSIDER:
  //   Maybe move the reference to the audio source into this object, to consolidate things so that
  //   I don't have to do a dictionary get twice. The tradeoff is that they don't seem really
  //   semantically linked imho.
  private sealed class ItemState
  {
    public int NextClipIndex = 0;
    // public int ActiveInstanceCount = 0;
  }

  [SerializeField]
  private AudioSource m_AudioSourcePrefab;

  private readonly Dictionary<AudioItem, ItemState> m_ItemStates = new();
  private readonly Dictionary<AudioItem, AudioSource> m_Sources = new();


  void Awake()
  {
    if (Instance != null && Instance != this)
    {
      Destroy(gameObject);

      return;
    }

    Instance = this;
    DontDestroyOnLoad(gameObject);
  }


  public void Play(AudioItem item)
  {
    if (item == null)
      throw new System.ArgumentNullException(nameof(item), $"Cannot play a null AudioItem");

    var source = GetSource(item);

    if (source.isPlaying && item.OverlapStrategy == AudioItem.OverlapStrategyOptions.Fail)
      return;

    var state = GetState(item);
    var clip = SelectClip(item, state);
    
    if (item.OverlapStrategy == AudioItem.OverlapStrategyOptions.Interrupt)
      source.Stop();
    
    source.PlayOneShot(clip);
  }


  private ItemState GetState(AudioItem item)
  {
    if (!m_ItemStates.TryGetValue(item, out var state))
    {
      state = new ItemState();
      m_ItemStates.Add(item, state);
    }

    return state;
  }


  private AudioClip SelectClip(AudioItem item, ItemState state)
  {
    if (item.Clips == null || item.Clips.Count <= 0)
      throw new System.InvalidOperationException($"AudioItem {item} has no clips");

    var index = item.SelectionStrategy switch
    {
      AudioItem.SelectionStrategyOptions.ByIndex => item.Index,
      AudioItem.SelectionStrategyOptions.Sequential => GetNextIndex(item, state),
      AudioItem.SelectionStrategyOptions.Random => GetRandomIndex(item),
      _ => throw new System.InvalidOperationException(
        $"Unknown selection strategy {item.SelectionStrategy}"),
    };

    return item.Clips[index];
  }


  private static int GetNextIndex(AudioItem item, ItemState state)
  {
    var index = state.NextClipIndex;
    state.NextClipIndex = (state.NextClipIndex + 1) % item.Clips.Count;
    return index;
  }


  private static int GetRandomIndex(AudioItem item) =>
    // TODO: use a controlled RNG instead
    Random.Range(0, item.Clips.Count);


  AudioSource GetSource(AudioItem item)
  {
    if (m_Sources.TryGetValue(item, out var source))
      return source;
    
    return CreateSource(item);
  }


  AudioSource CreateSource(AudioItem item)
  {
    var source = Instantiate(m_AudioSourcePrefab, transform);
    source.name = $"{item.name}_Player";
    m_Sources.Add(item, source);

    return source;
  }


  void OnDestroy()
  {
    if (Instance == this)
      Instance = null;
  }
}
