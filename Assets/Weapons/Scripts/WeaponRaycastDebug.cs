using UnityEngine;

// Draw weapon raycast gizmo (only for debug purpuose).
public class WeaponRaycastDebug : MonoBehaviour
{
  // ---------------------------------------------------------------------------------------------------------------------
  // Serialized fields                  
  // ---------------------------------------------------------------------------------------------------------------------
  #region

  // Distance in Unity units over which the 'Debug.DrawRay()' will be drawn.
  [SerializeField]
  [Range(10.0F,500.0F)]
  [Tooltip("Distance in Unity units over which the 'Debug.DrawRay()' will be drawn")]
  public float wepaon_range = 50.0F;

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
    // Get first person controller camera.
    this.fpc_camera = this.gameObject.GetComponentInParent<Camera>();
  } // End of Start

  // Update (called once per frame).
  private void Update()
  {
    // Create a vector at the center of our camera's viewport
    Vector3 line_origin = this.fpc_camera.ViewportToWorldPoint(new Vector3(0.5f,0.5f,0.0f));
    // Draw a green line in the from the point 'line_origin in the direction of 'fpc_camera.transform.forward * weaponRange'.
    // Only visable in scene view (to see it in game view enable necessery gizmos).
    Debug.DrawRay(line_origin,this.fpc_camera.transform.forward * this.wepaon_range,Color.green);
  } // End of Update

  #endregion

} // End of WeaponRaycastDebug