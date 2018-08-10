using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

// Class that manage levels.
public class LevelManager:MonoBehaviour
{
  // *****************************************  
  //             Public fields                  
  // *****************************************
  #region

  // Single static instance of 'LevelManager' (Singelton pattern).
  public static LevelManager Instance
  {
    get
    {
      return LevelManager._instance;
    }
  }
  // Enumeration of scenes.
  public enum Scenes { NONE, SPLASH, LOSE, WIN, MAIN_MENU, PAUSE_MENU, OPTIONS, HELP };
  // Enumeration of levels.
  public enum Lvls { NONE, Lvl_01 }

  #endregion


  // *****************************************  
  //             Private fields                  
  // *****************************************
  #region

  // Single static instance of 'LevelManager' (Singelton pattern).
  private static LevelManager _instance;
  // Current level.
  private Lvls cur_lvl=Lvls.NONE;

  #endregion


  // *****************************************  
  //             Public methods                  
  // *****************************************
  #region

  // Quit.
  public void Quit()
  {
    #if UNITY_EDITOR
      UnityEditor.EditorApplication.isPlaying = false;
    #else
      Application.Quit();
    #endif
  } // End of Quit

  // Load splash scene.
  public void SplashLoad(float delay)
  {
    SceneLoad(Scenes.SPLASH,delay);
  } // End of SplashLoad

  // Load lose scene.
  public void LoseLoad(float delay)
  {
    SceneLoad(Scenes.LOSE,delay);
  } // End of LoseLoad

  // Load win scene.
  public void WinLoad(float delay)
  {
    SceneLoad(Scenes.WIN,delay);
  } // End of WinLoad

  // Load main menu scene.
  public void MainMenuLoad(float delay)
  {
    SceneLoad(Scenes.MAIN_MENU,delay);
  } // End of MainMenuLoad

  // Load pause menu scene.
  public void PauseMenuLoad(float delay)
  {
    SceneLoad(Scenes.PAUSE_MENU,delay);
  } // End of PauseMenuLoad

  // Load options scene.
  public void OptionsLoad(float delay)
  {
    SceneLoad(Scenes.OPTIONS,delay);
  } // End of OptionsLoad

  // Load help scene.
  public void HelpLoad(float delay)
  {
    SceneLoad(Scenes.HELP,delay);
  } // End of HelpLoad

  // Load scene.
  public void SceneLoad(Scenes scene,float delay)
  {
    // Load scene with delay.
    StartCoroutine(SceneLoadWithDelay(scene,delay));
  } // End of SceneLoad

  // Load next level.
  public void LvlLoadNext(float delay)
  {
    // Load next level with delay.
    StartCoroutine(LvlLoadNextWithDelay(delay));
  } // End of LvlLoadNext

  #endregion


  // *****************************************  
  //             Private methods                  
  // *****************************************
  #region

  // Awake (used to initialize any variables or game state before the game starts).
  private void Awake()
  {
    if(LevelManager._instance==null)
    {
      LevelManager._instance=this;
    }
    else
    {
      GameObject.Destroy(this.gameObject);
    }
  } // End of Awake  

  // Initialization.
  private void Start()
  {
    // Make sure that game object will not be destroyed after loading next scene.
    GameObject.DontDestroyOnLoad(Instance.gameObject);   
  } // End of Start

  // Load scene with delay.
  private IEnumerator SceneLoadWithDelay(Scenes scene,float delay)
  {
    // If no scene then exit from function.
    if(scene==Scenes.NONE)
    {
      yield break;
    }
    // Wait for 'time' seconds.
    yield return new WaitForSeconds(delay);
    // Actualize counter.
    Instance.cur_lvl=Lvls.NONE;
    // Load scene.
    SceneManager.LoadScene(SceneNameDecode(scene));
  } // End of SceneLoadWithDelay

  // Load next level with delay.
  private IEnumerator LvlLoadNextWithDelay(float delay)
  {
    // If current level is last.
    if((int)Instance.cur_lvl==Lvls.GetNames(typeof(Lvls)).Length-1)
    {
      // Load 'Win' level.
      WinLoad(delay);
      // Exit from function.
      yield break;
    }
    // Wait for 'time' seconds.
    yield return new WaitForSeconds(delay);
    // Actualize current level.
    Instance.cur_lvl++;
    // Load next level.
    SceneManager.LoadScene(LvlNameDecode(Instance.cur_lvl));
  } // End of LvlLoadNextWithDelay

  // Decode scene name.
  private string SceneNameDecode(Scenes scene)
  {
    switch(scene)
    {
      case Scenes.SPLASH:
      {
        return "00_Splash";
      }
      case Scenes.LOSE:
      {
        return "01_Lose";
      }
      case Scenes.WIN:
      {
        return "01_Win";
      }
      case Scenes.MAIN_MENU:
      {
        return "02_MainMenu";
      }
      case Scenes.PAUSE_MENU:
      {
        return "02_PauseMenu";
      }
      case Scenes.OPTIONS:
      {
        return "02_Options";
      }
      case Scenes.HELP:
      {
        return "02_Help";
      }
      default:
      {
        throw new System.Exception("Cannot load scene ["+scene.ToString()+"]");
      }
    }
  } // End of SceneNameDecode

  // Decode level name.
  private string LvlNameDecode(Lvls lvl)
  {
    switch(lvl)
    {
      case Lvls.Lvl_01:
      {
        return "03_Lvl_01";
      }
      default:
      {
        throw new System.Exception("Cannot load level ["+lvl.ToString()+"]");
      }
    }
  } // End of LvlNameDecode

  #endregion

} // End of LevelManager