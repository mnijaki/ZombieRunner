using UnityEngine;
using UnityEngine.AI;

// Enemy.
[RequireComponent(typeof(EnemyHealth))]
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
  // ---------------------------------------------------------------------------------------------------------------------
  // Serialized fields                  
  // ---------------------------------------------------------------------------------------------------------------------
  #region

  // Attack damage.
  [SerializeField]
  [Range(5,300)]
  [Tooltip("Attack damage")]
  private int damage = 10;
  // Minimum speed.
  [SerializeField]
  [Range(0.1F,5.0F)]
  [Tooltip("Minimum speed")]
  private float min_speed = 0.5F;
  // Maximum speed.
  [SerializeField]
  [Range(0.1F,5.0F)]
  [Tooltip("Maximum speed")]
  private float max_speed = 2.0F;
  // Minimum angular speed.
  [SerializeField]
  [Range(10.0F,100.0F)]
  [Tooltip("Minimum angular speed")]
  private float min_angular_speed = 80.0F;
  // Maximum angular speed.
  [SerializeField]
  [Range(100.0F,360.0F)]
  [Tooltip("Maximum angular speed")]
  private float max_angular_speed = 120.0F;
  // Minimum acceleration.
  [SerializeField]
  [Range(1.0F,10.0F)]
  [Tooltip("Minimum acceleration")]
  private float min_acceleration = 5.0F;
  // Maximum acceleration.
  [SerializeField]
  [Range(10.0F,50.0F)]
  [Tooltip("Maximum acceleration")]
  private float max_acceleration = 10.0F;

  #endregion


  // ---------------------------------------------------------------------------------------------------------------------
  // Private fields                  
  // ---------------------------------------------------------------------------------------------------------------------
  #region

  // Nav Mesh Agent.
  private NavMeshAgent agent;

  #endregion


  // ---------------------------------------------------------------------------------------------------------------------
  // Public methods                  
  // ---------------------------------------------------------------------------------------------------------------------
  #region

  // Get enemy attack damage.
  public int DamageGet()
  {
    return this.damage;
  } // End of DamageGet

  #endregion


  // ---------------------------------------------------------------------------------------------------------------------
  //  Private methods                  
  // ---------------------------------------------------------------------------------------------------------------------
  #region

  // Initialization.
  private void Start()
  {
    // Get Nav Mesh Agent.
    this.agent=this.GetComponent<NavMeshAgent>();
    // Randomize speed.
    SpeedRandomize();
  } // End of Start

  // Randomize speed.
  private void SpeedRandomize()
  {
    // Actualize speed.
    this.agent.speed=Random.Range(this.min_speed,this.max_speed);
    // Actualize angular speed.
    this.agent.angularSpeed=Random.Range(this.min_angular_speed,this.max_angular_speed);
    // Actualize acceleration.
    this.agent.acceleration=Random.Range(this.min_acceleration,this.max_acceleration);
  } // End of SpeedRandomize

  #endregion

} // End of Enemy