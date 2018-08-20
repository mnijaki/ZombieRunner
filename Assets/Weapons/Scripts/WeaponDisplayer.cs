using UnityEngine;

// Dsiplay weapon.
public class WeaponDisplayer:MonoBehaviour
{
  // ---------------------------------------------------------------------------------------------------------------------
  // Private fields                  
  // ---------------------------------------------------------------------------------------------------------------------
  #region

  // Weapon type (scriptable object).
  private WeaponType weapon_type;

  #endregion


  // ---------------------------------------------------------------------------------------------------------------------
  // Private methods                  
  // ---------------------------------------------------------------------------------------------------------------------
  #region

  // Initialization.
  private void Start()
  {
    // Get weapon type.
    this.weapon_type=this.GetComponent<Weapon>().weapon_type;

    // MN:2018/08/18: Use this constructuion if you don't want to change default position of weapon in prefab...
    // Instantiate(this.weapon_type.model,this.weapon_type.trans.position,this.weapon_type.trans.rotation,this.transform);

    // Display weapon.
    Instantiate(this.weapon_type.model,this.transform);
  } // End of Start

  // Event - on weapon type changed.
  private void OnWeaponTypeChanged()
  {
    // TO_DO:MN:2018/08/20: Need to be implemented.
  } // End of OnWeaponTypeChanged

  #endregion

} // End of WeaponDisplayer