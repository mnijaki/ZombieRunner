using UnityEngine;
using System.Collections;

// Player voice manager.
public class PlayerVoice : MonoBehaviour
{
  // ****************************************************************************************************************** \\
  //                                              Public fields                  
  // ****************************************************************************************************************** \\
  #region

  // Suitable landing area for helicopter found clip.
  [Tooltip("Suitable landing area for helicopter found clip")]
  public AudioClip landing_area_found_clip;
  // Radio call for helicopter clip.
  [Tooltip("Radio call for helicopter clip")]
  public AudioClip heli_radio_call_clip;
  // Player clear clip.
  [Tooltip("Player clear clip")]
  public AudioClip clear_clip;
  // Player hit clip.
  [Tooltip("Player hit clip")]
  public AudioClip hit_clip;
  // Player death clip.
  [Tooltip("Player death clip")]
  public AudioClip death_clip;

  #endregion

  // ****************************************************************************************************************** \\
  //                                            Serialized fields                  
  // ****************************************************************************************************************** \\
  #region

  // What happened clip.
  [SerializeField]
  [Tooltip("What happened clip")]
  private AudioClip what_happened_clip;

  #endregion


  // ****************************************************************************************************************** \\
  //                                              Private fields                  
  // ****************************************************************************************************************** \\
  #region

  // Audio source.
  private AudioSource voice;

  #endregion


  // ****************************************************************************************************************** \\
  //                                              Public methods                  
  // ****************************************************************************************************************** \\
  #region

  // Play audio clip on 'voice' audio source.
  public void VoicePlay(AudioClip clip, float delay)
  {
    StartCoroutine(VoicePlayWithDelay(clip,delay));
  } // VoicePlay
  
  #endregion


  // ****************************************************************************************************************** \\
  //                                              Private methods                  
  // ****************************************************************************************************************** \\
  #region

  // Initialization.
  private void Start()
  {
    // Get voice audio source.
    this.voice=this.GetComponent<AudioSource>();
    // Play what happened clip.
    VoicePlay(this.what_happened_clip,6);
  } // End of Start

  // Play audio clip on 'voice' audio source with delay.
  private IEnumerator VoicePlayWithDelay(AudioClip clip,float delay)
  {
    // If no clip then exit from function.
    if(clip==null)
    {
      yield break;
    }
    // Wait for seconds.
    yield return new WaitForSeconds(delay);
    // Play clip.
    this.voice.clip=clip;
    this.voice.Play();
  } // End of VoicePlayWithDelay

  #endregion

} // End of PlayerVoice