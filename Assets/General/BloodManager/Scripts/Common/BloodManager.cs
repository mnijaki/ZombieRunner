using UnityEngine;
using System.Collections;

// Blood manager.
public class BloodManager : MonoBehaviour
{
  // ---------------------------------------------------------------------------------------------------------------------
  // Private fields                  
  // ---------------------------------------------------------------------------------------------------------------------
  #region

  // Blood controller.
  private BloodRainCameraController blood_controller;
  // Damage audio source.
  private AudioSource damage_audio;

  #endregion


  // ---------------------------------------------------------------------------------------------------------------------
  // Public methods
  // ---------------------------------------------------------------------------------------------------------------------
  #region

  // Start blood effect.
  public void BloodStart(int damage)
  {
    this.damage_audio.Play();
    this.blood_controller.Attack(damage);  
  } // End of BloodStart

  #endregion


  // ---------------------------------------------------------------------------------------------------------------------
  // Private methods
  // ---------------------------------------------------------------------------------------------------------------------
  #region

  // Initialization.
  private void Start()
  {
    // Get blood rain camera controller.
    this.blood_controller=this.gameObject.transform.GetChild(0).GetComponent<BloodRainCameraController>();
    // Get damage audio source.
    this.damage_audio=this.GetComponent<AudioSource>();
  } // End of Start

  // Stop all effects (You can stop and clear effects by 'Refresh()').
  private void BloodStopAll() 
	{
    // Reset blood controler.
    this.blood_controller.Reset();
    // Stop damage audio clip.
    this.damage_audio.Stop();
  } // End of BloodStopAll

  // Stop blood effect after delayed time (for example if blood effects should not be permanent).
  private IEnumerator BloodStopWithDelay(float delay)
  {
    // Wait for 'time' seconds.
    yield return new WaitForSeconds(delay);
    // Stop blood effects.
    this.blood_controller.Reset();
  } // End of BloodStopWithDelay

  #endregion

} // End of BloodManager