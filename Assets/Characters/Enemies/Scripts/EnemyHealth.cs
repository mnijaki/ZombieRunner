using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

// Enemy health characteristic.
public class EnemyHealth : MonoBehaviour
{
  // ---------------------------------------------------------------------------------------------------------------------
  // Serialized fields                  
  // ---------------------------------------------------------------------------------------------------------------------
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
  // Enemy blood particles system.
  [SerializeField]
  [Tooltip("Enemy blood particles system")]
  private ParticleSystem blood;

  #endregion


  // ---------------------------------------------------------------------------------------------------------------------
  // Private fields                  
  // ---------------------------------------------------------------------------------------------------------------------
  #region  

  // Audio source.
  private AudioSource audio_source;

  #endregion


  // ---------------------------------------------------------------------------------------------------------------------
  // Public methods       
  // ---------------------------------------------------------------------------------------------------------------------
  #region  

  // Decrease health.
  public void HealthDecrease(int val)
  {
    // Decrease health.
    this.health-=val;
    // Play blood effect.
    this.blood.Play();
    // If health < 1.
    if(this.health<1)
    {
      // Play death clip.
      AudioSource.PlayClipAtPoint(this.death_clip,this.transform.position);
      // Disable audio source.
      this.audio_source.enabled=false;
      // Disable AI.
      this.GetComponent<AICharacterControl>().enabled=false;
      // Disable third person.
      this.GetComponent<ThirdPersonCharacter>().enabled=false;
      // Disable enemy.
      this.GetComponent<Enemy>().enabled=false;
      // Disable navmesh.
      this.GetComponent<NavMeshAgent>().enabled=false;
      // Disable rigidbody.
      this.GetComponent<Rigidbody>().isKinematic=true;
      // Time of death animation.
      float time_of_death_anim = 0.0F;

      // TO_DO:  
      //         corutine should have duration of death animation (voice is not necessery because is set as clip at point).
      //         this.GetComponent<Animator>().SetTrigger("Death");
      // Send message about enemy death.
      StartCoroutine(GameManager.Instance.OnEnemyDeath(time_of_death_anim,this.gameObject));
    }
    // If health >= 1.
    else
    {
      // Play hit clip.
      AudioSource.PlayClipAtPoint(this.hit_clip,this.transform.position);      
    }
  } // End of HealthDecrease

  #endregion


  // ---------------------------------------------------------------------------------------------------------------------
  // Private methods       
  // ---------------------------------------------------------------------------------------------------------------------
  #region  

  // Initialization.
  private void Start()
  {
    // Get audio source.
    this.audio_source=this.GetComponent<AudioSource>();
  } // End of Start

  #endregion

} // End of EnemyHealth