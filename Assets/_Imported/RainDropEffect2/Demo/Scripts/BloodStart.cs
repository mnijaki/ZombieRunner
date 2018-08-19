using UnityEngine;
using System.Collections;

public class BloodStart : MonoBehaviour
{

	[SerializeField]
	BloodRainCameraController bloodRainController;

	[SerializeField]
	AudioSource splashInAudio;

	[SerializeField]
	AudioSource splashOutAudio;

	[SerializeField]
	AudioSource damageAudio;

  private void StopAll () 
	{
		bloodRainController.Reset ();
		// You can stop and clear effects by Refresh ()
		splashInAudio.Stop ();
		splashOutAudio.Stop ();
		damageAudio.Stop ();
	}

  public void Blood()
  {
    if(bloodRainController.HP <= 30)
    {
      bloodRainController.Reset();
      bloodRainController.HP = 100;
    }
    else
    {
      damageAudio.Play();
      bloodRainController.Attack(30);
    }
    //StartCoroutine(BloodStop(1.0F));
    // if reset 
    //bloodRainController.Reset ();
  }

  private IEnumerator BloodStop(float delay)
  {
    // Wait for 'time' seconds.
    yield return new WaitForSeconds(delay);
    bloodRainController.Reset();
  } // End of BloodStop

}
