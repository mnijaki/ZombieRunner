using UnityEngine;
using UnityEngine.AI;

// Enemy health characteristic.
public class EnemyHealth : MonoBehaviour
{
  // ****************************************************************************************************************** \\
  //                                                  Serialized fields                  
  // ****************************************************************************************************************** \\
  #region

  // Health.
  [SerializeField]
  [Tooltip("Health of enemy")]
  private int health = 100;
  // Enemy hit clip.
  [SerializeField]
  [Tooltip("Enemy hit clip")]
  private AudioClip hit_clip;
  // Enemy death clip.
  [SerializeField]
  [Tooltip("Enemy death clip")]
  private AudioClip death_clip;

  #endregion


  // ****************************************************************************************************************** \\
  //                                                  Private fields                  
  // ****************************************************************************************************************** \\
  #region  

  // Audio source.
  private AudioSource audio_source;

  #endregion


  // ****************************************************************************************************************** \\
  //                                                  Public methods       
  // ****************************************************************************************************************** \\
  #region  

  // Decrease health.
  public void HealthDecrease(int val)
  {
    // Decrease health.
    this.health-=val;
    // TO_DO: add blood particles.
    //
    // If health < 1.
    if(this.health<1)
    {
      // Play death clip.
      AudioSource.PlayClipAtPoint(this.death_clip,this.transform.position);
      // Disable audio source.
      this.audio_source.enabled=false;
      // Disable navmesh.
      this.GetComponent<NavMeshAgent>().enabled=false;

      // TO_DO:  play death anim (mayby adding hight gravity to rigidbdy will be sufficient for anim)
      //         corutine should have duration of death animation (voice is not necessery because is set as clip at point). 
      // Send message about enemy death.
      StartCoroutine(GameManager.Instance.OnEnemyDeath(5,this.gameObject));
    }
    // If health >= 1.
    else
    {
      // Play hit clip.
      AudioSource.PlayClipAtPoint(this.hit_clip,this.transform.position);
    }
  } // End of HealthDecrease

  #endregion


  // ****************************************************************************************************************** \\
  //                                                  Private methods       
  // ****************************************************************************************************************** \\
  #region  

  // Initialization.
  private void Start()
  {
    // Get audio source.
    this.audio_source=this.GetComponent<AudioSource>();
  } // End of Start

  #endregion

} // End of EnemyHealth