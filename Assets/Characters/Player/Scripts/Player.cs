using System.Collections;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

// Player manager.
[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(Weapon))]
public class Player : MonoBehaviour
{
  // ---------------------------------------------------------------------------------------------------------------------
  // Serialized fields                  
  // ---------------------------------------------------------------------------------------------------------------------
  #region

  // Initial walk speed of player.
  [SerializeField]
  [Range(1.0F,20.0F)]
  [Tooltip("Initial walk speed of player")]
  private float initial_walk_speed=7.0F;
  // Initial run speed of player.
  [SerializeField]
  [Range(1.0F,30.0F)]
  [Tooltip("Initial run speed of player")]
  private float initial_run_speed=14.0F;
  // Initial jump speed of player.
  [SerializeField]
  [Range(1.0F,20.0F)]
  [Tooltip("Initial jump speed of player")]
  private float initial_jump_speed = 10.0F;
  // Helicopter prefab.
  [SerializeField]
  [Tooltip("Helicopter prefab")]
  private Heli heli_prefab;
  // Helicopter starting point.
  [SerializeField]
  [Tooltip("Helicopter starting point")]
  private Transform heli_starting_point;
  // Player weapon game object.
  [SerializeField]
  [Tooltip("Player weapon game object")]
  private GameObject weapon_go;
  // Weapon aim.
  [SerializeField]
  [Tooltip("Weapon aim")]
  private GameObject weapon_aim;

  #endregion


  // ---------------------------------------------------------------------------------------------------------------------
  // Private fields                  
  // ---------------------------------------------------------------------------------------------------------------------
  #region

  // Array of spawn points.
  private Transform[] spawn_points;
  // Player voice.
  private PlayerVoice player_voice;
  // Player health.
  private PlayerHealth health;  
  // Player weapon.
  private Weapon weapon;
  // First person controller.
  private FirstPersonController fpc;
  // Flag if landing area was found.
  private bool landing_area_found = false;
  // Flag if helicopter was called.
  private bool heli_was_called = false;

  #endregion


  // ---------------------------------------------------------------------------------------------------------------------
  // Public methods                  
  // ---------------------------------------------------------------------------------------------------------------------
  #region

  // Enable weapon.
  public void WeaponEnable()
  {
    // Enable player weapon.
    this.weapon_go.SetActive(true);
    // Enable player weapon aim.
    this.weapon_aim.SetActive(true);
  } // End of WeaponEnable

  // Disable weapon.
  public void WeaponDisable()
  {
    // Disable player weapon.
    this.weapon_go.SetActive(false);
    // Disable player weapon aim.
    this.weapon_aim.SetActive(false);
  } // End of WeaponDisable

  // Respawn player at random spawn point.
  public void Respawn()
  {
    // Enable player controler (was disabled so he cannot move while level info was being showed).
    this.GetComponent<FirstPersonController>().enabled=true;
    // Respawn player at spawn point. 
    this.transform.position=this.spawn_points[Random.Range(1,this.spawn_points.Length)].transform.position;
    // Reset flags.    
    this.landing_area_found=false;
    this.heli_was_called=false;
    // Reset health.
    this.health.HealthReset();
    // Actualize speed of player.
    SpeedAct();
  } // End of Respawn

  #endregion


  // ---------------------------------------------------------------------------------------------------------------------
  // Private methods                  
  // ---------------------------------------------------------------------------------------------------------------------
  #region

  // Initialization.
  private void Start()
  {
    // Get spawn points.
    this.spawn_points=GameObject.FindGameObjectWithTag("player_spawn_points").GetComponentsInChildren<Transform>();
    // Get voice.
    this.player_voice=this.transform.Find("PlayerVoice").GetComponent<PlayerVoice>();
    // Get health.
    this.health=this.GetComponent<PlayerHealth>();
    // Get weapon.
    this.weapon=this.weapon_go.GetComponent<Weapon>();
    // Get first person controller.
    this.fpc=this.GetComponent<FirstPersonController>();
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

  // Actualize speed of player.
  private void SpeedAct()
  {
    // Get speed factor (max carrying weight of player is 100 kg).
    float factor = (100.0F-this.weapon.WeightGet()) / 100.0F;
    // Actualize walk speed.
    this.fpc.WalkSpeedSet(this.initial_walk_speed * factor);
    // Actualize run speed.
    this.fpc.RunSpeedSet(this.initial_run_speed * factor);
    // Actualize jump speed.
    this.fpc.JumpSpeedSet(this.initial_jump_speed * factor);
  } // End of SpeedAct

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
    // If collision with enemy.
    if(other.CompareTag("enemy"))
    {
      // Decrease health.
      this.health.HealthDecrease(other.GetComponent<Enemy>().DamageGet());
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