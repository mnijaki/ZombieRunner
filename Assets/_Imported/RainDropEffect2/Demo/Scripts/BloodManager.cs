using UnityEngine;
using System.Collections;

// Blood manager.
public class BloodManager : MonoBehaviour
{
  // ---------------------------------------------------------------------------------------------------------------------
  // Serialized fields                  
  // ---------------------------------------------------------------------------------------------------------------------
  #region

  // Blood controller.
  [SerializeField]
  [Tooltip("Blood controller")]
  BloodRainCameraController blood_controller;
  // Damage audio clip.
	[SerializeField]
  [Tooltip("Damage audio clip")]
  AudioSource damage_clip;

  #endregion


  // ---------------------------------------------------------------------------------------------------------------------
  // Public methods
  // ---------------------------------------------------------------------------------------------------------------------
  #region

  // Start blood effect.
  public void BloodStart(int damage)
  {
    // MN:2018/08/19: currently I don't want to reset blood after death so I commented it. 
    //if(this.blood_controller.HP <= damage)
    //{
    //  this.blood_controller.Reset();
    //  this.blood_controller.HP = 100;
    //}
    //else
    //{
    this.damage_clip.Play();
    this.blood_controller.Attack(damage);
    //}
    // MN:2018/08/19: If needed uncomment code belove.
    //StartCoroutine(BloodStop(1.0F));    
  } // End of BloodStart

  #endregion

  // ---------------------------------------------------------------------------------------------------------------------
  // Private methods
  // ---------------------------------------------------------------------------------------------------------------------
  #region

  // Stop all effects.
  private void BloodStopAll() 
	{
    // You can stop and clear effects by 'Refresh()'.
    // Reset blood controler.
    this.blood_controller.Reset();
    // Stop damage audio clip.
    this.damage_clip.Stop();
  } // End of BloodStopAll

  // Stop blood effect.
  private IEnumerator BloodStop(float delay)
  {
    // Wait for 'time' seconds.
    yield return new WaitForSeconds(delay);
    // Stop blood effects.
    this.blood_controller.Reset();
  } // End of BloodStop

  #endregion

} // End of BloodManager