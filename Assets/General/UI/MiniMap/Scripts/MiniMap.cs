using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
  // *****************************************  
  //             Serialized fields                  
  // *****************************************
  #region
  
  // Player transform.
  [SerializeField]
  [Tooltip("Player transform")]
  private Transform player_trans;
  // Flag if minimap should rotate with player.
  [SerializeField]
  [Tooltip("Flag if minimap should rotate with player")]
  private bool is_roatable=false;

  #endregion


  // *****************************************  
  //             Private fields                  
  // *****************************************
  #region

  // Mini map camera.
  private Camera mini_map_camera;
  // Min and max zoom of camera.
  private float max_zoom = 50.0F;
  private float min_zoom = 200.0F;

  #endregion


  // *****************************************  
  //             Private methods
  // *****************************************
  #region

  // Initialization.
  private void Start()
  {
    // Get mini map camera.
    this.mini_map_camera=this.GetComponent<Camera>();
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
    // If camera shoul rotate
    if(this.is_roatable)
    {
      this.transform.rotation=Quaternion.Euler(90.0F,this.player_trans.eulerAngles.y,0.0F);
    }
  } // End of LateUpdate

  #endregion

} // End of MiniMap