using UnityEngine;

// Player camera manager
public class PlayerCamera : MonoBehaviour
{
  // ---------------------------------------------------------------------------------------------------------------------
  // Private fields                  
  // ---------------------------------------------------------------------------------------------------------------------
  #region

  // Camera.
  private Camera player_camera;
  // Default field of view.
  private float def_fov;

  #endregion


  // ---------------------------------------------------------------------------------------------------------------------
  // Private methods                  
  // ---------------------------------------------------------------------------------------------------------------------
  #region

  // Initialization.
  private void Start()
  {
    // Get player camera.
    this.player_camera=this.GetComponent<Camera>();
    // Get default field of view.
    this.def_fov=this.player_camera.fieldOfView;
  } // End of Start

  // Update (called once per frame).
  private void Update()
  {
    // If user pressed zoom button.
    if(Input.GetButton("player_camera_zoom"))
    {
      // Set camera field of view.
      this.player_camera.fieldOfView=this.def_fov/1.8F;
    }
    // If user unpressed zoom.
    else
    {
      // Set camera default field of view.
      this.player_camera.fieldOfView=this.def_fov;
    }
  } // End of Update

  #endregion

} // End of PlayerCamera