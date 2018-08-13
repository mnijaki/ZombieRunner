using UnityEngine;

// Dsiplay weapon.
public class WeaponDisplayer:MonoBehaviour
{
  // ****************************************************************************************************************** \\
  //                                            Private fields                  
  // ****************************************************************************************************************** \\
  #region

  // Weapon type.
  private WeaponType weapon_type;

  #endregion


  // ****************************************************************************************************************** \\
  //                                            Private methods                  
  // ****************************************************************************************************************** \\
  #region

  // Initialization.
  private void Start()
  {
    // Get weapon type.
    this.weapon_type=this.GetComponent<Weapon>().weapon_type;
    // Display weapon.
    Instantiate(this.weapon_type.model,new Vector3(2.0F,0.0F,4.2F),Quaternion.Euler(0.0F,90.0F,0.0F),this.transform);
  } // End of Start

  #endregion

  // TO_DO: show weapon info in hud
  //        make function that display info if weapon was changed

} // End of WeaponDisplayer