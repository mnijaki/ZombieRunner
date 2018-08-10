using UnityEngine;

// Level(scene) changer
public class LevelChanger : MonoBehaviour
{
  // *****************************************  
  //             Serialized fields                  
  // *****************************************
  #region

  // Scene to load.
  [SerializeField]
  [Tooltip("Scene to load")]
  public LevelManager.Scenes scene;
  // Delay of loading scene.
  [SerializeField]
  [Range(0.0F,15.0F)]
  [Tooltip("Delay of loading scene")]
  private float delay = 0.0F;

  #endregion


  // *****************************************  
  //             Public methods                  
  // *****************************************
  #region

  // Quit.
  public void Quit()
  {
    LevelManager.Instance.Quit();
  } // End of Quit

  // Load splash scene.
  public void SplashLoad(float delay)
  {
    LevelManager.Instance.SplashLoad(delay);
  } // End of SplashLoad

  // Load lose scene.
  public void LoseLoad(float delay)
  {
    LevelManager.Instance.LoseLoad(delay);
  } // End of LoseLoad

  // Load win scene.
  public void WinLoad(float delay)
  {
    LevelManager.Instance.WinLoad(delay);
  } // End of WinLoad

  // Load main menu scene.
  public void MainMenuLoad(float delay)
  {
    LevelManager.Instance.MainMenuLoad(delay);
  } // End of MainMenuLoad

  // Load pause menu scene.
  public void PauseMenuLoad(float delay)
  {
    LevelManager.Instance.PauseMenuLoad(delay);
  } // End of PauseMenuLoad

  // Load options scene.
  public void OptionsLoad(float delay)
  {
    LevelManager.Instance.OptionsLoad(delay);
  } // End of OptionsLoad

  // Load help scene.
  public void HelpLoad(float delay)
  {
    LevelManager.Instance.HelpLoad(delay);
  } // End of HelpLoad

  #endregion


  // *****************************************  
  //             Private methods                  
  // *****************************************
  #region

  // Initialization.
  private void Start()
  {
    // Load scene.
    LevelManager.Instance.SceneLoad(this.scene,this.delay);
  } // End of Start

  #endregion

} // End of LevelChanger