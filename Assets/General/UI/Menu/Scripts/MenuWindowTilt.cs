using UnityEngine;

// Tilt window (change it angle depending of mause position).
public class MenuWindowTilt: MonoBehaviour
{
	public Vector2 range = new Vector2(5f, 3f);

	Transform mTrans;
	Quaternion mStart;
	Vector2 mRot = Vector2.zero;

	void Start ()
	{
		mTrans = transform;
		mStart = mTrans.localRotation;
	}

	void Update ()
	{
    
		Vector3 pos = Input.mousePosition;   

    float halfWidth = Screen.width * 0.5f;
		float halfHeight = Screen.height * 0.5f;
		float x = Mathf.Clamp((pos.x - halfWidth) / halfWidth, -1f, 1f);
		float y = Mathf.Clamp((pos.y - halfHeight) / halfHeight, -1f, 1f);
    // Usunig 'Time.unscaledDeltaTime' becouse in pause menu 'Time.scale=0'.
    mRot = Vector2.Lerp(mRot,new Vector2(x,y),Time.unscaledDeltaTime * 5f);
    // Actualize angle of window.
    mTrans.localRotation = mStart * Quaternion.Euler(-mRot.y * range.y, mRot.x * range.x, 0f);
	}

} // ENd of MenuWindowTilt