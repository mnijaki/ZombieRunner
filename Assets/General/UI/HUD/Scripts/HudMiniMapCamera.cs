using UnityEngine;

// Mini map.
public class HudMiniMapCamera: MonoBehaviour
{
  // ---------------------------------------------------------------------------------------------------------------------
  // Serialized fields                  
  // ---------------------------------------------------------------------------------------------------------------------
  #region

  // Flag if minimap should rotate with player.
  [SerializeField]
  [Tooltip("Flag if minimap should rotate with player")]
  private bool is_roatable=false;

  #endregion


  // ---------------------------------------------------------------------------------------------------------------------
  // Private fields                  
  // ---------------------------------------------------------------------------------------------------------------------
  #region

  // Mini map camera.
  private Camera mini_map_camera;
  // Min and max zoom of camera.
  private float max_zoom = 50.0F;
  private float min_zoom = 200.0F;
  // Player transform.
  private Transform player_trans;

  #endregion


  // ---------------------------------------------------------------------------------------------------------------------
  // Private methods
  // ---------------------------------------------------------------------------------------------------------------------
  #region

  // Initialization.
  private void Start()
  {
    // Get mini map camera.
    this.mini_map_camera=this.GetComponent<Camera>();
    // Get player transform.
    this.player_trans=GameObject.FindObjectOfType<Player>().GetComponent<Transform>();
  } // End of Start

  // Update.
  private void Update()
  {
    // If user hit 'mini_map_zoom_in' and camera is not zoomed in to maximum.
    if((Input.GetButton("mini_map_zoom_in"))&&(this.mini_map_camera.orthographicSize>this.max_zoom))
    {
      this.mini_map_camera.orthographicSize--;
    }
    // If user hit 'mini_map_zoom_out' and camera is not zoomed out to minimum.
    if((Input.GetButton("mini_map_zoom_out"))&&(this.mini_map_camera.orthographicSize<this.min_zoom))
    {
      this.mini_map_camera.orthographicSize++;
    }
  } // End of Update

  // Called after update and fixed update (all things that moved should be redrawed at this time).
  private void LateUpdate()
  {
    // Get player position.
    Vector3 pos = this.player_trans.position;
    // Set camera on fixed height.
    pos.y=this.transform.position.y;
    // Actualize position of camera.
    this.transform.position=pos;

    // TO_DO: add clipping of minimap
    //float x = Mathf.Clamp(pos.x,2*this.mini_map_camera.orthographicSize,pos.x);
    //float z = Mathf.Clamp(pos.z,this.mini_map_camera.orthographicSize,pos.z);
    //this.transform.position=new Vector3(x,pos.y,z);

    // If camera should rotate.
    if(this.is_roatable)
    {
      // Rotate camera.
      this.transform.rotation=Quaternion.Euler(90.0F,this.player_trans.eulerAngles.y,0.0F);
    }
  } // End of LateUpdate

  #endregion

} // End of HudMiniMapCamera