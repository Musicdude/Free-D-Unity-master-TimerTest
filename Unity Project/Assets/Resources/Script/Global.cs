using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/* <summary>
 * Global Class for some values.
 * </summary>
 */

// Cause I am lazy to type global.type
public enum PlanetType		{ typeA, typeB, typeC, typeD };
public enum NoteType		{ typeA, typeB, typeC, typeD };
public enum DifficultyType	{ easy, normal, hard };
public enum SoundType 		{ music, sfx };
public enum ScreenType		{ main, game };
public enum AndroidControl	{ accelerometer, drag };

public class Global : MonoBehaviour 
{
	#region Variables

	#if UNITY_ANDROID || UNITY_IPHONE
		public static AndroidControl mControlType			= AndroidControl.drag;
	#endif	
	[SerializeField] private List<GameObject> mObjects	= new List<GameObject>();
	[SerializeField] private int mScreenWidth;		// Screen values
	[SerializeField] private int mScreenHeight;		// Screen values

	public static DifficultyType 	mCurrentDifficulty	= DifficultyType.normal; // Default values
	public static bool 				mMusicOn			= true;					 // Default values
	public static bool 				mSFXOn 				= true;					 // Default values
	public static bool 				mPause 				= false;				 // Default values
	#endregion

	#region Singleton
	private static Global mInstance;
	public static Global Instance
	{
		get
		{
			if(mInstance == null)	mInstance = GameObject.Find("Global").GetComponent<Global>();
			return mInstance;
		}
	}
	#endregion

	#region Unity Function
	private void Awake()
	{
		if(mInstance == null) mInstance = this;
		else if(mInstance != this)		
		{
			if(mInstance.gameObject != this.gameObject)	Destroy(gameObject);
			else 										Destroy(this);
		}
		DontDestroyOnLoad(this.gameObject);		// Persistent

		if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			Screen.SetResolution(mScreenWidth,mScreenHeight,true);
			foreach(GameObject g in mObjects)	g.SetActive(true);
		}
		else
			Screen.SetResolution(mScreenWidth,mScreenHeight,false);
	}
	#endregion
}