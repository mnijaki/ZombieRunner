using UnityEngine;

// Weapon type.
[CreateAssetMenu(fileName = "New weapon type", menuName = "Weapon type")]
public class WeaponType : ScriptableObject
{
  // ****************************************************************************************************************** \\
  //                                            Public fields                  
  // ****************************************************************************************************************** \\
  #region

  // Weapon damage.
  [Range(10,500)]
  [Tooltip("Weapon damage")]
  public int damage = 10;
  // Weapon range.
  [Range(50,500)]
  [Tooltip("Weapon range")]
  public int range = 100;
  // Fire rate (number of shells fired per second).
  [Range(0.1F,5.0F)]
  [Tooltip("Fire rate (time in seconds how long firing one shell take)")]
  public float fire_rate = 0.1F;
  // Fire rate (number of shells fired per second).
  [Tooltip("Fire rate (time in seconds how long firing one shell take)")]
  public float fire_rate2 = 0.1F;
  // Shell velocity (in meters).
  [Range(50,500)]
  [Tooltip("Shell velocity (in meters)")]
  public int shell_velocity = 50;
  // Clip size.
  [Range(1,100)]
  [Tooltip("Clip size")]
  public int clip_size = 12;
  // Initial ammo.
  [Range(1,1000)]
  [Tooltip("Initial ammo")]
  public int initial_ammo = 30;
  // Recoil force. 
  [Range(1,50)]
  [Tooltip("Recoil force")]
  public int recoil_force = 1;
  // Hit force.
  [Range(1,500)]
  [Tooltip("Hit force")]
  public int hit_force = 100;
  // Laser draw duration.
  [Range(0.1F,1.0F)]
  [Tooltip("Laser draw duration")]
  public float laser_draw_duration = 0.07F;

  #endregion


  // ****************************************************************************************************************** \\
  //                                            Serialized fields                  
  // ****************************************************************************************************************** \\
  #region

  // Model.
  [SerializeField]
  [Tooltip("Model")]
  public GameObject model;
  // Shell projectile.
  [SerializeField]
  [Tooltip("Shell projectile")]
  private GameObject projectile;

  #endregion

} // End of WeaponType