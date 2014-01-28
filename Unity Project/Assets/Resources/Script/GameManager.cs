using UnityEngine;
using System.Collections;

/* <summary>
 * The Game Manager will save and load
 * data as well as holding the difficulty settings
 * and transition from 1 level to another.
 * </summary>
 */
[RequireComponent (typeof(Transition))]
public class GameManager : MonoBehaviour 
{
	[SerializeField] private Difficulty[] mDifficultyList;	// List of Difficulties available
	[SerializeField] private ScreenType mType;				// Screen Type
	[SerializeField] private string mLevelToLoad;			// Level to load
	private Difficulty mCurrentDifficulty;					// Current Difficulty
	private Transition mTransition;							// Transition Controller

	#region Singleton
	private static GameManager mInstance;
	public static GameManager Instance
	{
		get
		{
			if(mInstance == null)	GameObject.Find("GameManager").GetComponent<GameManager>();
			return mInstance;
		}
	
	}
	#endregion

	#region Unity Function
	private void Awake()
	{
		if(mInstance == null) mInstance = this;
		else if(mInstance.gameObject != this.gameObject)	Destroy(this.gameObject);
		else 												Destroy(this);
		DontDestroyOnLoad(this.gameObject);
		mTransition = gameObject.GetComponent<Transition>();
		mTransition.Init();
		mTransition.FadeIn();	
		Type = ScreenType.main;
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}
	private void Start()	{	LoadData();	}
	private void Update()
	{
		if(Input.GetKeyUp(KeyCode.Escape))
		{
			if(mType == ScreenType.game)
			{
				Debug.Log("Quiting Level");
				StartCoroutine( LoadLevel(mLevelToLoad,ScreenType.main) );
			}
			else
			{
				SaveData();
				Application.Quit();
			}
		}
	}
	#endregion

	#region Class Function
	public IEnumerator LoadLevel(string _levelName,ScreenType _type)
	{

		mTransition.FadeOut();
		yield return new WaitForSeconds(2.0f);
		mTransition.FadeIn();
		mType = _type;
		if(mType == ScreenType.game)
		{
			SoundManager.Instance.SetGUI(true);
			SoundManager.Instance.PlayBGM("Free");
		}
		else						
		{
			SoundManager.Instance.SetGUI(false);
			SoundManager.Instance.PlayBGM("Pamgaea");
		}
		Application.LoadLevel(_levelName);
	}
	public Difficulty GetCurrentDifficulty()	{	return mCurrentDifficulty;	}
	public void SetDifficulty(DifficultyType _type)
	{
		foreach(Difficulty temp in mDifficultyList)
		{
			if(temp.mDifficulty == _type)	
			{
				mCurrentDifficulty = temp;
				Global.mCurrentDifficulty = mCurrentDifficulty.mDifficulty;
				if(ButtonManager.Instance != null) ButtonManager.Instance.UpdateDifficultyButtons();
				return;
			}
		}
		throw new System.ArgumentException("Invalid Typing");
	}
	public void ResetScores()
	{
		foreach(Difficulty d in mDifficultyList)	{	d.mHighScore = 0;	}
	}
	public int GetScore(DifficultyType _type)
	{
		foreach(Difficulty d in mDifficultyList)
		{		
			if(d.mDifficulty == _type) 
				return d.mHighScore;		
		}
		throw new System.ArgumentOutOfRangeException("Invalid Type");
	}

	public void SaveData()
	{
		switch(mCurrentDifficulty.mDifficulty)
		{
		case DifficultyType.easy: 	mDifficultyList[0].mHighScore = mCurrentDifficulty.mHighScore;	break;
		case DifficultyType.normal:	mDifficultyList[1].mHighScore = mCurrentDifficulty.mHighScore;	break;
		case DifficultyType.hard:	mDifficultyList[2].mHighScore = mCurrentDifficulty.mHighScore;	break;
		}
		foreach(Difficulty d in mDifficultyList)
		{
			switch(d.mDifficulty)
			{
			case DifficultyType.easy: 	PlayerPrefs.SetInt("EasyHighscore",d.mHighScore);	break;
			case DifficultyType.normal:	PlayerPrefs.SetInt("NormalHighscore",d.mHighScore);	break;
			case DifficultyType.hard:	PlayerPrefs.SetInt("HardHighScore",d.mHighScore);	break;
			}
		}
		PlayerPrefs.SetInt("Music",Global.mMusicOn?1:0);
		PlayerPrefs.SetInt("SFX",Global.mSFXOn?1:0);
		PlayerPrefs.SetString("Difficulty",Global.mCurrentDifficulty.ToString());
		#if UNITY_ANDROID || UNITY_IPHONE
		PlayerPrefs.SetString("AndroidControl",Global.mControlType.ToString());
		#endif
		PlayerPrefs.Save();
		ButtonManager.Instance.UpdateDifficultyButtons();
		ButtonManager.Instance.UpdateAndroidControlButtons();

	}
	public void LoadData()
	{
		PlayerPrefs.DeleteAll();
		for(int i=0; i<mDifficultyList.Length;i++)
		{
			switch(mDifficultyList[i].mDifficulty)
			{
			case DifficultyType.easy: 	mDifficultyList[i].mHighScore = 	PlayerPrefs.GetInt("EasyHighscore");	break;
			case DifficultyType.normal:	mDifficultyList[i].mHighScore = 	PlayerPrefs.GetInt("NormalHighscore");	break;
			case DifficultyType.hard:	mDifficultyList[i].mHighScore = 	PlayerPrefs.GetInt("HardHighScore");	break;
			}
		}

		// Added checks to fix start button error
		if(PlayerPrefs.HasKey("Music")) Global.mMusicOn	= PlayerPrefs.GetInt("Music")==1?true:false;
		else 							Global.mMusicOn	= true;
		if(PlayerPrefs.HasKey("SFX"))	Global.mSFXOn	= PlayerPrefs.GetInt("SFX")==1?true:false;
		else							Global.mSFXOn = true;
		if(PlayerPrefs.HasKey("Difficulty"))	Global.mCurrentDifficulty = (DifficultyType) System.Enum.Parse( typeof( DifficultyType ), PlayerPrefs.GetString("Difficulty"));
		else 									Global.mCurrentDifficulty = DifficultyType.normal;
		#if UNITY_ANDROID || UNITY_IPHONE
		if(PlayerPrefs.HasKey("AndroidControl"))	Global.mControlType = (AndroidControl) System.Enum.Parse( typeof( AndroidControl ), PlayerPrefs.GetString("AndroidControl"));
		else 										Global.mControlType = AndroidControl.drag;
		#endif
		SetDifficulty(Global.mCurrentDifficulty);
		ButtonManager.Instance.UpdateAndroidControlButtons();
		SoundManager.Instance.SetVolume();
	}
	public ScreenType Type
	{
		get	{ return mType;		}
		set	{ mType = value;	}
	}
	public string LevelToLoad { get { return mLevelToLoad; } }
	#endregion
}