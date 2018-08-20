using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.Characters.ThirdPerson;
using System.Linq;

// Game manager.
public class GameManager : MonoBehaviour
{
  // ---------------------------------------------------------------------------------------------------------------------
  // Public fields                  
  // ---------------------------------------------------------------------------------------------------------------------
  #region

  // Single static instance of GameManager (Singelton pattern).
  public static GameManager Instance
  {
    get
    {
      return GameManager._instance;
    }
  }

  #endregion


  // ---------------------------------------------------------------------------------------------------------------------
  // Serialized fields                  
  // ---------------------------------------------------------------------------------------------------------------------
  #region
    
  // Player.
  [SerializeField]
  [Tooltip("Player")]
  private Player player;
  // Landing area prefab.
  [SerializeField]
  [Tooltip("Landing area prefab")]
  private GameObject landing_area_prefab;
  // Array of enemies prefabs.
  [SerializeField]
  [Tooltip("Array of enemies prefabs")]
  private Enemy[] enemies_prefabs;
  // Number of enemies to spawn at start of the level.
  [SerializeField]
  [Range(0,50)]
  [Tooltip("Number of enemies to spawn at start of the level")]
  private int enemies_to_spawn=7;
  // Time of enemies to spawn at start of the level.
  [SerializeField]
  [Range(0.0F,15.0F)]
  [Tooltip("Time of enemies to spawn at start of the level")]  
  private int enemies_time_to_spawn=3;
  // Fade panel.
  [SerializeField]
  [Tooltip("Fade panel")]
  private GameObject fade_panel;
  // Level info text.
  [SerializeField]
  [Tooltip("Level info text")]
  private GameObject lvl_info_txt;
  // Mini map.
  [SerializeField]
  [Tooltip("Mini map")]
  private GameObject mini_map;
  // Enemy body dissapear time.
  [SerializeField]
  [Range(0.0F,30.0F)]
  [Tooltip("Enemy body dissapear time.")]
  private float enemy_body_dissapear_time=5.0F;

  #endregion


  // ---------------------------------------------------------------------------------------------------------------------
  // Private fields                  
  // ---------------------------------------------------------------------------------------------------------------------
  #region

  // Single static instance of GameManager (Singelton pattern).
  private static GameManager _instance;  
  // Array of enemies spawn points.
  private Transform[] enemies_spawn_points;
  // Parent of the enemies.
  private Transform enemies_parent;
  // Flag if there will be change of level.
  private bool is_lvl_changing=false;

  #endregion


  // ---------------------------------------------------------------------------------------------------------------------
  // Public methods                  
  // ---------------------------------------------------------------------------------------------------------------------
  #region

  // Event - on helicpoter was called.
  public void OnHeliWasCalled()
  {
    // Spawn second wave of enemies.
    for(int i=0; i<Instance.enemies_to_spawn; i++)
    {
      Instantiate(Instance.enemies_prefabs[Random.Range(0,Instance.enemies_prefabs.Length)],
                  Instance.enemies_spawn_points[Random.Range(0,Instance.enemies_spawn_points.Length)].position,
                  Quaternion.identity,
                  Instance.enemies_parent).GetComponent<AICharacterControl>().target=Instance.player.transform;
    }
  } // End OnHeliWasCalled

  // Event - on player boarded helicopter.
  public void OnPlayerBoardedHeli(float heli_hor_movement_time)
  {
    // End level.
    StartCoroutine(LvlEnd(heli_hor_movement_time+2.0F));
  } // End of OnPlayerBoardedHeli

  // Event - on player death.
  public IEnumerator OnPlayerDeath(float duration)
  {
    // Change flag.
    Instance.is_lvl_changing=true;
    // Yield (this yield exist to give necessery time to the calling script to perform it duties).
    yield return new WaitForSeconds(duration);
    // ------------------------------------------------------------------
    // Fade out.
    // ------------------------------------------------------------------
    // Actualize duration.
    duration=3.0F;
    // Fade out.
    StartCoroutine(FadeOut(duration));
    // ------------------------------------------------------------------
    // Wait.
    // ------------------------------------------------------------------
    // Yield (synchronizing with fade).
    yield return new WaitForSeconds(duration);
    // ------------------------------------------------------------------
    // Unlock cursor.
    // ------------------------------------------------------------------
    // Unlock cursor.
    Instance.player.GetComponent<FirstPersonController>().MouseLookGet().SetCursorLock(false);
    // ------------------------------------------------------------------
    // Load lose screen.
    // ------------------------------------------------------------------
    // Load lose screen.
    LevelManager.Instance.LoseLoad(0.0F);
  } // End of OnPlayerDeath

  // Event - on enemy death.
  public IEnumerator OnEnemyDeath(float duration, GameObject enemy)
  {
    // Yield (this yield exist to give necessery time to the calling script to perform it duties).
    yield return new WaitForSeconds(duration);
    // Yield for time of dissapearing enemy body.
    yield return new WaitForSeconds(this.enemy_body_dissapear_time);
    // Destroy enemy game object.
    GameObject.Destroy(enemy);
    // Spawn new enemy.
    EnemySpawn();

    // TO_DO: pooling (spawn at point) with correct reset of enemy values (eg.nav mesh, etc).

  } // End of OnEnemyDeath

  #endregion


  // ---------------------------------------------------------------------------------------------------------------------
  // Private methods                  
  // ---------------------------------------------------------------------------------------------------------------------
  #region

  // Awake (used to initialize any variables or game state before the game starts).
  private void Awake()
  {
    if(GameManager._instance==null)
    {
      GameManager._instance=this;
    }
    else
    {
      GameObject.Destroy(this.gameObject);
    }
  } // End of Awake

  // Initialize.
  private void Start()
  {
    // Start game.
    StartCoroutine(StartGame(10.0F));    
  } // End of Start

  // Update.
  private void Update()
  {
    // If user hit 'Escape' button and level is not changing (becouse interrupting 
    // loading of next level could lead to some bugs).
    if((Input.GetKeyDown(KeyCode.Escape))&&(!Instance.is_lvl_changing))
    {
      // Open pause menu.
      PauseMenuManager.Instance.Pause();
    }
  } // End of Update

  // Initialization of landing area.
  private void LandingAreaInit()
  {
    // Get landing area spawn points parent.
    GameObject landing_area_spawn_points_parent=GameObject.FindGameObjectWithTag("landing_area_spawn_points");
    // Get array of helicopter landing area spawn points (exclude parent).
    Transform[] landing_area_spawn_points=landing_area_spawn_points_parent.GetComponentsInChildren<Transform>().
                                          Where(x => x.gameObject.transform.parent!=landing_area_spawn_points_parent.transform.parent).ToArray();
    // Instantiate landing area.
    Instantiate(Instance.landing_area_prefab,
                landing_area_spawn_points[Random.Range(1,landing_area_spawn_points.Length)].position,
                Quaternion.identity,
                Instance.transform.parent);
  } // End of LandingAreaInit

  // Initialization of enemies.
  private void EnemiesInit()
  {
    // Get enemies spawn points parent.
    GameObject enemies_spawn_points_parent=GameObject.FindGameObjectWithTag("enemies_spawn_points");
    // Get enemies spawn points (exclude parent).
    Instance.enemies_spawn_points=enemies_spawn_points_parent.GetComponentsInChildren<Transform>().
                                  Where(x => x.gameObject.transform.parent != enemies_spawn_points_parent.transform.parent).ToArray();
    // Create enemies parent.
    Instance.enemies_parent=new GameObject("Enemies").transform;
    // Spawn enemies with delay.
    StartCoroutine(EnemiesSpawnWithDelay(Instance.enemies_time_to_spawn));    
  } // EnemiesInit

  // Spawn enemies with delay.
  private IEnumerator EnemiesSpawnWithDelay(float delay)
  {
    // Wait for seconds.
    yield return new WaitForSeconds(delay);
    // Spawn enemies.
    for(int i=0; i<Instance.enemies_to_spawn; i++)
    {
      // Spawn enemy.
      EnemySpawn();
    }
  } // End of EnemiesSpawnWithDelay

  // Spawn enemy.
  private void EnemySpawn()
  {
    Instantiate(Instance.enemies_prefabs[Random.Range(0,Instance.enemies_prefabs.Length)],
                  Instance.enemies_spawn_points[Random.Range(0,Instance.enemies_spawn_points.Length)].position,
                  Quaternion.identity,
                  Instance.enemies_parent).GetComponent<AICharacterControl>().target=Instance.player.transform;
  } // End of EnemySpawn

  // Start game.
  private IEnumerator StartGame(float duration)
  {
    // ---------------------------------------------------------------------------
    // Level info.
    // ---------------------------------------------------------------------------
    // Yield to show level info.
    yield return new WaitForSeconds(4);
    // Hide level info text.
    Instance.lvl_info_txt.SetActive(false);
    // ---------------------------------------------------------------------------
    // Instantiate game objects.
    // ---------------------------------------------------------------------------
    // Initialization of landing area.
    LandingAreaInit();
    // Initialization of enemies.
    EnemiesInit();
    // ---------------------------------------------------------------------------
    // Respawn player.
    // ---------------------------------------------------------------------------
    // Respawn player at random spawn point.
    Instance.player.Respawn();
    // ---------------------------------------------------------------------------
    // Fade in.
    // ---------------------------------------------------------------------------   
    // Time left.
    float time_left =0.0F;
    // Create black color with alpha=1;
    Color black=new Color(0,0,0,255);
    // Loop until time is reached.
    while(time_left<1)
    {      
      // Actualize time left.
      time_left+=Time.deltaTime/duration;
      // Actualize alpha color.
      black.a=1-time_left;
      // Set color of panel.
      Instance.fade_panel.GetComponent<Image>().color=black;
      // Yield
      yield return null;
    }
    // Disable fade panel.
    Instance.fade_panel.SetActive(false);
    // ---------------------------------------------------------------------------
    // Show mini map and weapon.
    // ---------------------------------------------------------------------------
    // Show mini map.
    this.mini_map.SetActive(true);
    // Enable player weapon.
    this.player.WeaponEnable();
  } // End of StartGame

  // Fade out.
  private IEnumerator FadeOut(float duration)
  {
    // Hide mini map.
    this.mini_map.SetActive(false);
    // Enable fade panel.
    Instance.fade_panel.SetActive(true);
    // Progress.
    float progress=0.0F;
    // Create black color with alpha set as transparent;
    Color black=new Color(0,0,0,255);
    // Loop until progress < 1.
    while(progress<1)
    {
      // Actualize progress.
      progress+=Time.deltaTime/duration;
      // Actualize alpha value (to be more solid).
      black.a=progress;
      // Set color of panel.
      Instance.fade_panel.GetComponent<Image>().color=black;
      // Yield
      yield return null;
    }
  } // End of FadeOut

  // End level.
  private IEnumerator LvlEnd(float duration)
  {
    // Change flag.
    Instance.is_lvl_changing=true;
    // Fade out.
    StartCoroutine(FadeOut(duration));
    // Yield (synchronizing with fade).
    yield return new WaitForSeconds(duration);
    // Unlock cursor.
    Instance.player.GetComponent<FirstPersonController>().MouseLookGet().SetCursorLock(false);
    // Load next level.
    LevelManager.Instance.LvlLoadNext(0.0F);
  } // End of LvlEnd

  #endregion

} // End of GameManager