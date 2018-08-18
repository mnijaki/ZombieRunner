using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

// Player health characteristic.
public class PlayerHealth : MonoBehaviour
{
  // ---------------------------------------------------------------------------------------------------------------------
  // Serialized fields                  
  // ---------------------------------------------------------------------------------------------------------------------
  #region

  // Starting health.
  [SerializeField]
  [Range(100,500)]
  [Tooltip("Starting health of player")]
  private int starting_health = 100;  

  #endregion


  // ---------------------------------------------------------------------------------------------------------------------
  // Private fields                  
  // ---------------------------------------------------------------------------------------------------------------------
  #region  

  // Player voice.
  private PlayerVoice player_voice;
  // Info if player is dead.
  private bool is_dead = false;
  // Health.
  private int health;
  // Armor.
  private int armor = 0;

  #endregion


  // ---------------------------------------------------------------------------------------------------------------------
  // Public methods       
  // ---------------------------------------------------------------------------------------------------------------------
  #region  

  // Decrease health.
  public void HealthDecrease(int damage)
  {
    // If player is dead then exit from function.
    if(this.is_dead)
    {
      return;
    }
    // If player have armor.
    if(this.armor>0)
    {
      // If armor is greater than 2/3 of damage value.
      if(this.armor > (damage*2/3))
      {
        // Reduce armor.
        this.armor-=damage*2/3;
        // Actualize damage.
        damage=damage/3;
      }
      // If armor is not greater than 2/3 of damage value.
      else
      {
        // Actualize damage.
        damage-=this.armor;
        // Reduce armor to 0.
        this.armor=0;
      }
      // Decrease health.
      this.health-=damage;
    }
    // If player don't have armor.
    else
    {
      // Decrease health.
      this.health-=damage;
    }
    // TO_DO: add blood effect on screen.
    //
    // If health < 1.
    if(this.health<1)
    {
      // Change flag.
      this.is_dead=true;
      // Play death clip.
      this.player_voice.VoicePlay(this.player_voice.death_clip,0.0F);
      // Disable player controller (so player cannot move after being killed).
      this.GetComponent<FirstPersonController>().enabled=false;

      // Disable 
      // TO_DO: shake camera
      //        show more blood
      //        fade in

      // Send message about player death.
      StartCoroutine(GameManager.Instance.OnPlayerDeath(this.player_voice.death_clip.length));
    }
    // If health >= 1.
    else
    {
      // Play hit clip.
      this.player_voice.VoicePlay(this.player_voice.hit_clip,0.0F);
    }
  } // End of HealthDecrease

  // Reset health to starting value.
  public void HealthReset()
  {
    // Reset health.
    this.health=this.starting_health;
    // Reset armor.
    this.armor=0;
    // Reset flag.
    this.is_dead=false;
  } // End of HealthReset

  #endregion


  // ---------------------------------------------------------------------------------------------------------------------
  // Private methods       
  // ---------------------------------------------------------------------------------------------------------------------
  #region  

  // Initialization.
  private void Start()
  {
    // Get voice.
    this.player_voice=this.transform.Find("PlayerVoice").GetComponent<PlayerVoice>();
  } // End of Start

  #endregion

} // End of PlayerHealth