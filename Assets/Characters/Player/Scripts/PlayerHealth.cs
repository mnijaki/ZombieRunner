using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

// Player health.
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
  private int initial_health = 100;
  // Starting armor.
  [SerializeField]
  [Range(0,200)]
  [Tooltip("Starting armor of player")]
  private int initial_armor = 100;

  #endregion


  // ---------------------------------------------------------------------------------------------------------------------
  // Private fields                  
  // ---------------------------------------------------------------------------------------------------------------------
  #region  

  // Player.
  private Player player;
  // Player voice.
  private PlayerVoice player_voice;
  // Blood manager.
  private BloodManager blood_manager;
  // Health.
  private int health;
  // Armor.
  private int armor = 0;
  // Info if player is dead.
  private bool is_dead = false;

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
    // Decrease armor.
    damage=ArmorDecrease(damage);
    // Decrease health.
    this.health-=damage;
    // Actualize HUD health.
    HudIcons.Instance.HealthSet((float)this.health/this.initial_health);
    // Show blood effect.
    this.blood_manager.BloodStart(damage);
    // Start default shake of player camera.
    GameObject.FindObjectOfType<PlayerCameraShake>().ShakeDefStart();
    // If health < 1.
    if(this.health<1)
    {
      // Change flag.
      this.is_dead=true;
      // Play death clip.
      this.player_voice.VoicePlay(this.player_voice.death_clip,0.0F);
      // Disable player controller (so player cannot move after being killed).
      this.GetComponent<FirstPersonController>().enabled=false;
      // Disable player weapon.
      this.player.WeaponDisable();
      // Send message about player death.
      StartCoroutine(GameManager.Instance.OnPlayerDeath(this.player_voice.death_clip.length));
    }
    // If health >= 1.
    else
    {
      // MN:2018/08/19: Commented because blood system plays own sound.
      // Play hit clip.
      //this.player_voice.VoicePlay(this.player_voice.hit_clip,0.0F);
    }
  } // End of HealthDecrease

  // Reset health to starting value.
  public void HealthReset()
  {
    // Reset health.
    this.health=this.initial_health;
    // Reset armor.
    this.armor=this.initial_armor;
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
    // Get player.
    this.player=this.GetComponent<Player>();
    // Get voice.
    this.player_voice=this.transform.Find("PlayerVoice").GetComponent<PlayerVoice>();
    // Get blood manager.
    this.blood_manager=GameObject.FindObjectOfType<BloodManager>();
    // Set starting health and armor in HUD.
    HudIcons.Instance.HealthSet(1);
    HudIcons.Instance.ArmorSet(1);
  } // End of Start

  // Decrease armor.
  private int ArmorDecrease(int damage)
  {
    // If player have no armor.
    if(this.armor<=0)
    {
      // Return damage.
      return damage;
    }
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
    // Actualize HUD armor.
    HudIcons.Instance.ArmorSet((float)this.armor/this.initial_armor);
    // Return damage.
    return damage;
  } // End of ArmorDecrease

  #endregion

} // End of PlayerHealth