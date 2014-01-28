using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ButtonManager : MonoBehaviour 
{
	#region Variables
	[SerializeField] private List<ButtonChangeDifficulty>	mButtonDifficultyList	= new List<ButtonChangeDifficulty>();
	[SerializeField] private List<ButtonControl>			mAndroidControlList		= new List<ButtonControl>();
	private bool mEnabled;
	#endregion

	#region Singleton
	private static ButtonManager mInstance;
	public static ButtonManager Instance
	{
		get
		{
			if(mInstance == null)	GameObject.Find("ButtonManager").GetComponent<ButtonManager>();
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
			if(mInstance.gameObject != this.gameObject)	Destroy(this.gameObject);
			else 										Destroy(this);
		}
		mEnabled = true;
	}

	private void Update()
	{
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		if(OnHoverHook	 != null) OnHoverHook(ray);
		// Buttons are clickable only if enabled
		if(mEnabled)
		{
			if(OnClickHook	 != null) OnClickHook(ray);
			if(OnReleaseHook != null) OnReleaseHook(ray);
		}
		if(OnExitHook  	 != null) OnExitHook(ray);
	}
	#endregion

	#region Class Function
	// changing the difficulty colors
	public void UpdateDifficultyButtons()
	{
		for(int i=0;i<mButtonDifficultyList.Count;i++)
		{
			if(Global.mCurrentDifficulty == mButtonDifficultyList[i].Type)	mButtonDifficultyList[i].Selected = true;
			else 															mButtonDifficultyList[i].Selected = false;
		}
	}
	public void UpdateAndroidControlButtons()
	{
	#if UNITY_ANDROID || UNITY_IPHONE
		if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			for(int i=0;i<mAndroidControlList.Count;i++)
			{
				if(Global.mControlType == mAndroidControlList[i].Type)	mAndroidControlList[i].Selected = true;
				else 													mAndroidControlList[i].Selected = false;
			}
		}
	#elif UNITY_EDITOR || UNITY_STANDALONE
		if(Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
		{
			for(int i=0;i<mAndroidControlList.Count;i++)
			{
				mAndroidControlList[i].Disable();
			}
		}
	#endif
	}

	public bool EnableButton
	{
		get { return mEnabled;  }
		set	{ mEnabled = value;	}
	}
	#endregion

	#region Delegate
	public delegate void OnClickDelegate(Ray _ray);
	public event OnClickDelegate OnClickHook;
	public delegate void OnHoverDelegate(Ray _ray);
	public event OnHoverDelegate OnHoverHook;
	public delegate void OnExitDelegate(Ray _ray);
	public event OnExitDelegate OnExitHook;
	public delegate void OnReleaseDelegate(Ray _ray);
	public event OnReleaseDelegate OnReleaseHook;
	#endregion
}
