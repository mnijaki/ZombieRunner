using System.Collections;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

// Helicopter manager.
public class Heli:MonoBehaviour
{
  // ---------------------------------------------------------------------------------------------------------------------
  // Serialized fields                  
  // ---------------------------------------------------------------------------------------------------------------------
  #region

  // Helicopter radio audio source.
  [SerializeField]
  [Tooltip("Helicopter radio audio source")]
  private AudioSource radio_audio;
  // Helicopter radio response clip.
  [SerializeField]
  [Tooltip("Helicopter radio response clip")]
  private AudioClip radio_response_clip;
  // Heli is on site clip.
  [SerializeField]
  [Tooltip("Heli is on site clip")]
  private AudioClip heli_on_site_clip;
  // Vertical movement time of helicopter.
  [SerializeField]
  [Range(10.0F,600.0F)]
  [Tooltip("Vertical movement time of helicopter")]  
  private float ver_movement_time = 60.0F;

  #endregion


  // ---------------------------------------------------------------------------------------------------------------------
  // Private fields                  
  // ---------------------------------------------------------------------------------------------------------------------
  #region

  // Helicopter starting point.
  private Vector3 start_point_pos;
  // Landing area position.
  private Vector3 landing_area_pos;
  // Horizontal movement time of helicopter (to put helicopter up or down).
  private float hor_movement_time = 7.0F;
  // Flag if helicopter is flaying to rescue point.
  private bool is_flaying = true;

  #endregion


  // ---------------------------------------------------------------------------------------------------------------------
  // Private methods                  
  // ---------------------------------------------------------------------------------------------------------------------
  #region

  // Start.
  private void Start()
  {
    // Play 'radio response' clip.
    this.radio_audio.clip=this.radio_response_clip;
    this.radio_audio.Play();
    // Get starting position.
    this.start_point_pos=this.transform.position;
    // Get landing area position.
    this.landing_area_pos=GameObject.FindGameObjectWithTag("landing_area").transform.position;
    // Actualize height of landing area (so helicopter will not sink into landing area).
    this.landing_area_pos.y+=0.5F;
    // Set fly position (fixed height).
    Vector3 fly_pos=new Vector3(this.landing_area_pos.x,150,this.landing_area_pos.z);
    // Put down helicopter.
    StartCoroutine(PutDown(this.transform,fly_pos));
  } // End of Start

  // Put helicopter down.
  private IEnumerator PutDown(Transform trans, Vector3 end_pos)
  {
    // Start position.
    Vector3 start_pos=trans.position;
    // Time left.
    float time_left=0.0F;
    // Loop until time is reached (x,z movement).
    while(time_left<1)
    {
      // Actualize time left.
      time_left+=Time.deltaTime/this.ver_movement_time;
      // Move object.
      trans.position = Vector3.Lerp(start_pos,end_pos,time_left);
      // Yield.
      yield return null;
    }
    // Set end position (to nullify float precision).
    trans.position=end_pos;
    // Actualize values.
    start_pos=trans.position;
    end_pos=this.landing_area_pos;
    time_left=0.0F;
    // Loop until time is reached (y movement).
    while(time_left<1)
    {
      // Actualize time left.
      time_left+=Time.deltaTime/this.hor_movement_time;
      // Move object.
      trans.position = Vector3.Lerp(start_pos,end_pos,time_left);
      // Yield.
      yield return null;
    }
    // Set end position (to nullify float precision).
    trans.position=end_pos;
    // Play 'heli_on_site_clip'.
    this.radio_audio.clip=this.heli_on_site_clip;
    this.radio_audio.Play();
    // Change flag.
    this.is_flaying=false;
  } // End PutDown  

  // On collision.
  private void OnTriggerEnter(Collider other)
  {
    // If helicopter is still flaying then exit from function.
    if(this.is_flaying)
    {
      return;
    }
    // If not colision with player then exit from function.
    if(!other.gameObject.CompareTag("player"))
    {
      return;
    }
    // Get player.
    Player player=GameObject.FindObjectOfType<Player>();
    // Get 'FirstPersonController'.
    FirstPersonController fpc=player.GetComponent<FirstPersonController>();
    // Disable gravity of player;
    fpc.GravityMultiplierSet(0);
    // Disable movemnt of player.
    fpc.JumpSpeedSet(0.0F);
    fpc.RunSpeedSet(0.0F);
    fpc.WalkSpeedSet(0.0F);
    // Change player transform parent to helicopter (it will look like player is flying with helicopter).
    player.transform.parent=this.transform;
    // Get player voice.
    PlayerVoice player_voice = GameObject.FindObjectOfType<PlayerVoice>();
    // Play clear clip.
    player_voice.VoicePlay(player_voice.clear_clip,0.0F);
    // Set fly position (fixed height).
    Vector3 fly_pos = new Vector3(this.transform.position.x,150,this.transform.position.z);
    // Start movement.
    StartCoroutine(PutUp(this.transform,fly_pos));
  } // OnTriggerEnter 

  // Put helicopter up.
  private IEnumerator PutUp(Transform trans, Vector3 end_pos)
  {
    // Change flag.
    this.is_flaying=true;
    // Send message that player boarded helicopter.
    GameManager.Instance.OnPlayerBoardedHeli(this.hor_movement_time);
    // Start position.
    Vector3 start_pos = trans.position;
    // Start rotation.
    Quaternion start_rot = trans.rotation;
    // End rotation.
    Quaternion end_rot = new Quaternion();
    end_rot.eulerAngles=new Vector3(0,180,0);
    // Time left.
    float time_left = 0.0F;
    // Loop until time is reached (y movement).
    while(time_left<1)
    {
      // Actualize time left.
      time_left+=Time.deltaTime/this.hor_movement_time;
      // Move object.
      trans.position = Vector3.Lerp(start_pos,end_pos,time_left);
      // Rotate object.
      trans.rotation = Quaternion.Lerp(start_rot,end_rot,time_left);
      // Yield.
      yield return null;
    }
    // Set end position (to nullify float precision).
    trans.position=end_pos;
    // Actualize values.
    start_pos=trans.position;
    end_pos=this.start_point_pos;
    time_left=0.0F;
    // Loop until time is reached (x,z movement).
    while(time_left<1)
    {
      // Actualize time left.
      time_left+=Time.deltaTime/this.ver_movement_time;
      // Move object.
      trans.position = Vector3.Lerp(start_pos,end_pos,time_left);
      // Yield.
      yield return null;
    }
  } // End PutUp

  #endregion

} // End of Heli