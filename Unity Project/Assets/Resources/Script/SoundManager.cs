using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
	[SerializeField] private List<AudioClip> 	mAudioClipList 		= new List<AudioClip>();	// List of Clips
	[SerializeField] private List<AudioSource>	mAudioSourceList	= new List<AudioSource>();	// List of Source
	private GUIText		mTimeLeft;
	private int			mMinutes;
	private int			mSeconds;
	private string		mTime;
	#region Singleton
	private static SoundManager mInstance;
	public static SoundManager Instance
	{
		get
		{
			if(mInstance == null)	mInstance =	Resources.Load("SoundManager") as SoundManager;
			return mInstance;
		}
	}
	#endregion

	#region Unity Function
	private void Awake()
	{
		if (mInstance == null)		
		{	
			mInstance = this;
		}
		else if(mInstance != this)		
		{
			if(mInstance.gameObject != this.gameObject)	Destroy(gameObject);
			else 										Destroy(this);
		}
		DontDestroyOnLoad(this.gameObject);

		mTimeLeft = gameObject.GetComponentInChildren<GUIText>();
		mTimeLeft.enabled = false;
		mTimeLeft.text = "";
	}
	private void Start()
	{
		if(GameManager.Instance.Type == ScreenType.main) PlayBGM("Pamgaea");
		if(GameManager.Instance.Type == ScreenType.game) PlayBGM("Free");
	}
	private void Update()
	{
		if(GameManager.Instance.Type == ScreenType.game)
		{
			if(!mAudioSourceList[0].isPlaying && !Global.mPause)
			{
				PointsManager.Instance.UpdateScore();
				GameManager.Instance.LoadLevel(GameManager.Instance.LevelToLoad,ScreenType.main);
			}
		}
	}
	#endregion

	#region Class Function
	public void SetVolume()
	{
		SetVolume(SoundType.music);
		SetVolume(SoundType.sfx);
	}
	public void SetVolume(SoundType _type)
	{
		if(_type == SoundType.music)
		{
			if(Global.mMusicOn) mAudioSourceList[0].volume = 1;
			else 				mAudioSourceList[0].volume = 0;
		}
		else if(_type == SoundType.sfx)
		{
			// we skip the first as it is the BGM
			for(int i=1;i<mAudioSourceList.Count;i++)
			{
				if(Global.mSFXOn)	mAudioSourceList[i].volume = 1;
				else 				mAudioSourceList[i].volume = 0;
			}
		}
	}

	public void PlayBGM(string _clipName)
	{
		for(int i=0;i<mAudioClipList.Count;i++)
		{
			if(mAudioClipList[i].name == _clipName)
			{
				mAudioSourceList[0].clip = mAudioClipList[i];
				if(Application.loadedLevel == 1) mAudioSourceList[0].loop = false;
				else 							 mAudioSourceList[0].loop = true;
				mAudioSourceList[0].Play();
			}
		}
	}

	// Play Sound
	public void Play(string _clipName)
	{
		for(int i=0;i<mAudioSourceList.Count;i++)
		{
			if(mAudioSourceList[i].clip.name == _clipName)
			{
				mAudioSourceList[i].Play();
				return;
			}
		}
		throw new System.ArgumentException("Invalid Clip Name: " + _clipName);
	}
	public void SetGUI(bool _flag)		{	mTimeLeft.enabled = _flag;	}

	#endregion

	#region UI
	private void OnGUI()
	{

		if(mTimeLeft.enabled)
		{
			mMinutes = Mathf.FloorToInt((mAudioSourceList[0].clip.length - mAudioSourceList[0].time) / 60);
			mSeconds = (int)(mAudioSourceList[0].clip.length - mAudioSourceList[0].time) % 60;
			mTime 	 = string.Format("{0:0}:{1:00}",mMinutes,mSeconds);
			mTimeLeft.text = mTime;

		}
	}
	#endregion
}

