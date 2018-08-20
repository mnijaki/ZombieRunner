using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Weapon manager.
public class Weapon:MonoBehaviour
{
  // ---------------------------------------------------------------------------------------------------------------------
  // Public fields                  
  // ---------------------------------------------------------------------------------------------------------------------
  #region

  // Weapon type.
  [Tooltip("Weapon type")]
  public WeaponType weapon_type;

  #endregion


  // ---------------------------------------------------------------------------------------------------------------------
  // Private fields                  
  // ---------------------------------------------------------------------------------------------------------------------
  #region

  // Reference to the 'LineRenderer' component which will display laser line.
  private LineRenderer laser_line;
  // Laser draw duration.
  private WaitForSeconds laser_draw_duration;
  // End position of weapon.
  private Transform weapon_end;
  // Reference to the audio source which will play shooting sound effect.
  private AudioSource audio_source;
  // Reference to the first person camera.
  private Camera fpc_camera;
  // Float to store the time the player will be allowed to fire again, after firing.
  private float next_fire_time;
  // Animator.
  private Animator anim;

  #endregion


  // ---------------------------------------------------------------------------------------------------------------------
  // Public methods                  
  // ---------------------------------------------------------------------------------------------------------------------
  #region

  // Get weapon weight.
  public int WeightGet()
  {
    return this.weapon_type.weight;
  } // End of WeightGet

  #endregion


  // ---------------------------------------------------------------------------------------------------------------------
  // Private methods                  
  // ---------------------------------------------------------------------------------------------------------------------
  #region

  // Initialization.
  private void Start()
  {
    // Get laser line.
    this.laser_line = GetComponent<LineRenderer>();
    // Get laser draw duration.
    this.laser_draw_duration = new WaitForSeconds(this.weapon_type.beam_draw_duration);
    // Get audio source.
    this.audio_source = GetComponent<AudioSource>();
    // Get first person camera.
    this.fpc_camera = GetComponentInParent<Camera>();
    // Get animator.
    this.anim=this.gameObject.GetComponentInChildren<Animator>();
  } // End of Start

  // Update (called once per frame).
  private void Update()
  {
    // If the player has pressed the fire button and if enough time has elapsed since he last fired.
    if((Input.GetButtonDown("Fire1")) && (Time.time>this.next_fire_time))
    {
      // If no weapon end.
      if(this.weapon_end==null)
      {
        // Get weapon end.
        this.weapon_end=GameObject.FindGameObjectWithTag("weapon_end").transform;
      }
      // Update the time when player can fire next time.
      this.next_fire_time = Time.time + this.weapon_type.fire_rate;
      // Play fire audio.
      this.audio_source.Play();

      // MN:2018/08/18: If you have problems with correct drawing of laser that could happend becaouse of phantom
      // game object with given tag. If thath so just run 'DebugDestroyObjectsWithTag()' method.
      // Secon option is to replace correct line of code with this down below:
      // this.laser_line.SetPosition(0,this.transform.Find("LaserGun(Clone)").transform.Find("Model").transform.Find("WeaponEnd").transform.position);

      // TO_DO: add porjectiles, burs fire, if waeapon changed, change weapon type, change laser draw duration, weapon_end...

      // Create a vector at the center of camera's viewport.
      Vector3 fpc_camera_center = this.fpc_camera.ViewportToWorldPoint(new Vector3(0.5F,0.5F,0.0F));
      // Declare a raycast hit to store information about what raycast has hit.
      RaycastHit hit;
      // Set the start position for visual effect (like explosion, shells, etc) for weapon end position.
      this.laser_line.SetPosition(0,this.weapon_end.position);
      // If raycast has hit something.
      if(Physics.Raycast(fpc_camera_center,this.fpc_camera.transform.forward,out hit,this.weapon_type.range))
      {
        // Set the end position of laser line. 
        this.laser_line.SetPosition(1,hit.point);
        // Get enemy health component.
        EnemyHealth enemy_health = hit.collider.GetComponent<EnemyHealth>();
        // If there is enemy health component.
        if(enemy_health != null)
        {
          // Decrease enemy health.
          enemy_health.HealthDecrease(this.weapon_type.damage);
          // Add force to the rigidbody thath raycast has hit ('hit.normal' is the outward direction of the surface that raycast hit).
          hit.rigidbody.AddForce(-hit.normal * this.weapon_type.hit_force);
        }
      }
      // If raycast din't hit anything (e.g. shooting into the sky).
      else
      {
        // Set the end of the line to a position directly in front of the camera at the distance of weapon range.
        this.laser_line.SetPosition(1,fpc_camera_center + (this.fpc_camera.transform.forward*this.weapon_type.range));
      }
      // Draw laser line on and off.
      StartCoroutine(FireEffectDraw());
    }
  } // End of Update

  // Draw laser line on and off.
  private IEnumerator FireEffectDraw()
  {
    // Turn on our line renderer.
    this.laser_line.enabled = true;
    // Run fire animation.
    this.anim.SetTrigger("fire");
    // Wait for seconds.
    yield return this.laser_draw_duration;
    // Turn off our line renderer.
    this.laser_line.enabled = false;
  } // End of FireEffectDraw

  // Destroy objects tagged by given string (Only for purpouse of debug. Unity have some bug with animation and
  // prefab with tags - it will create non visable in hierarchy game object with tag. If that happens jus run this
  // function and all should works fine).
  private void DebugDestroyObjectsWithTag(string tag)
  {
    // Get objects.
    var objs = GameObject.FindGameObjectsWithTag(tag);
    // Loop over objects.
    for(int i=objs.Length-1; i>-1; i--)
    {
      // Write message to console.
      Debug.Log("Destroying object ="+objs[i]);
      // Destroy object
      Destroy(objs[i]);
    }
  } // End of DebugDestroyObjectsWithTag

  #endregion

} // End of Weapon