using UnityEngine;

// Draw weapon raycast gizmo (only for debug purpuose).
public class WeaponRaycastDebug : MonoBehaviour
{
  // ---------------------------------------------------------------------------------------------------------------------
  // Serialized fields                  
  // ---------------------------------------------------------------------------------------------------------------------
  #region

  // Distance in Unity units over which the Debug.DrawRay will be drawn.
  [SerializeField]
  [Range(10.0F,500.0F)]
  [Tooltip("Distance in Unity units over which the Debug.DrawRay will be drawn")]
  public float weaponRange = 50.0F;

  #endregion


  // ---------------------------------------------------------------------------------------------------------------------
  // Private fields                  
  // ---------------------------------------------------------------------------------------------------------------------
  #region

  // Holds a reference to the first person camera
  private Camera fpc_camera;

  #endregion


  // ---------------------------------------------------------------------------------------------------------------------
  // Private methods                  
  // ---------------------------------------------------------------------------------------------------------------------
  #region

  // Initialization
  private void Start()
  {
    // Get and store a reference to our Camera by searching this GameObject and its parents.
    this.fpc_camera = GetComponentInParent<Camera>();
  } // End of Start

  // Update (called once per frame).
  private void Update()
  {
    // Create a vector at the center of our camera's viewport
    Vector3 lineOrigin = this.fpc_camera.ViewportToWorldPoint(new Vector3(0.5f,0.5f,0.0f));
    // Draw a line in the Scene View  from the point lineOrigin in the direction of fpsCam.transform.forward * weaponRange, using the color green
    // Only visable in scene view (to see it in game view enable necessery gizmos).
    Debug.DrawRay(lineOrigin,this.fpc_camera.transform.forward * this.weaponRange,Color.green);
  } // End of Update

  #endregion

} // End of WeaponRaycastDebug