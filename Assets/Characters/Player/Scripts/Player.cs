using System.Collections;
using UnityEngine;

// Player manager.
public class Player : MonoBehaviour
{
  // *****************************************  
  //             Serialized fields                  
  // *****************************************
  #region

  // Helicopter prefab.
  [SerializeField]
  [Tooltip("Helicopter prefab")]
  private Heli heli_prefab;
  // Helicopter starting point.
  [SerializeField]
  [Tooltip("Helicopter starting point")]
  private Transform heli_starting_point;

  #endregion


  // *****************************************  
  //             Private fields                  
  // *****************************************
  #region
    
  // Array of spawn points.
  private Transform[] spawn_points;
  // Player voice.
  private PlayerVoice player_voice;
  // Flag if landing area was found.
  private bool landing_area_found = false;
  // Flag if helicopter was called.
  private bool heli_was_called = false;
  // Info if player is dead.
  private bool is_dead = false;

  #endregion


  // *****************************************  
  //             Private methods                  
  // *****************************************
  #region

  // Initialization.
  private void Start()
  {
    // Get spawn points.
    this.spawn_points=GameObject.FindGameObjectWithTag("player_spawn_points").GetComponentsInChildren<Transform>();
    // Get voice.
    this.player_voice=this.transform.Find("PlayerVoice").GetComponent<PlayerVoice>();
    // Respawn player at random spawn point.
    Respawn();
  } // End of Start

  // Update (called once per frame).
  private void Update()
  {
    // If player hit 'h' button and landing area was found and helicopter was not called.
    if((Input.GetButton("heli_call"))&&(this.landing_area_found)&&(!this.heli_was_called))
    {
      // Change flag.
      this.heli_was_called=true;
      // Play radio call.
      this.player_voice.VoicePlay(this.player_voice.heli_radio_call_clip,0.0F);
      // Call helicopter with delay.
      StartCoroutine(HeliCallWithDelay(this.player_voice.heli_radio_call_clip.length+2.0F));
    }
  } // End of Update

  // Respawn player at random spawn point.
  private void Respawn()
  {
    // Respawn player at spawn point. 
    this.transform.position=this.spawn_points[Random.Range(1,this.spawn_points.Length)].transform.position;
    // Reset flags.
    this.is_dead=false;
    this.landing_area_found=false;
    this.heli_was_called=false;
  } // End of Respawn

  // On colission.
  private void OnTriggerEnter(Collider other)
  {
    // If collision with landing area.
    if(other.CompareTag("landing_area"))
    {
      // If landing area was yet not found.
      if(!this.landing_area_found)
      {
        // Change flag.
        this.landing_area_found=true;
        // Play clip informing of finding suitable landing area.
        this.player_voice.VoicePlay(this.player_voice.landing_area_found_clip,0.0F);
        // Deploy flares with delay.
        StartCoroutine(DeployFlaresWithDelay(this.player_voice.landing_area_found_clip.length,other.gameObject));
      }
      // Exit from function.
      return;
    }
    // If player is not dead and collision with enemy.
    if((!this.is_dead)&&(other.CompareTag("enemy")))
    {
      // Change flag.
      this.is_dead=true;
      // Play player death clip.
      this.player_voice.VoicePlay(this.player_voice.player_death_clip,0.0F);
      // Send message about player death.
      StartCoroutine(GameManager.Instance.OnPlayerDeath(this.player_voice.player_death_clip.length));
    }
  } // End of OnTriggerEnter

  // Deploy flares with delay.
  private IEnumerator DeployFlaresWithDelay(float delay, GameObject landing_area)
  {
    // Wait for seconds.
    yield return new WaitForSeconds(delay);
    // Deploy flares.
    foreach(Transform flare in landing_area.transform)
    {
      flare.gameObject.SetActive(true);
    }
  } // End of DeployFlaresWithDelay

  // Call helicopter with delay.
  private IEnumerator HeliCallWithDelay(float delay)
  {
    // Wait for 'time' seconds.
    yield return new WaitForSeconds(delay);
    // Instantiate helicopter.
    Instantiate(this.heli_prefab,this.heli_starting_point.position,Quaternion.identity,this.transform.parent);
    // Send message to game manager that helicopter was called.
    GameManager.Instance.OnHeliWasCalled();
  } // End of HeliCallWithDelay

  #endregion

} // End of Player