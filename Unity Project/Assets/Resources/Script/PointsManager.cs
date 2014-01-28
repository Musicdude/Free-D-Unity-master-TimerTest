using UnityEngine;
using System.Collections;

public class PointsManager : MonoBehaviour 
{
	private int mNotePoints;					// Note Points
	private int mCurrentScore;					// Current Score
	[SerializeField] private GUIText mText;
	#region Singleton
	private static PointsManager mInstance;
	public static PointsManager Instance
	{
		get
		{
			if(mInstance == null) mInstance = GameObject.Find("PointsManager").GetComponent<PointsManager>();
			return mInstance;
		}
	}
	#endregion

	#region Unity Function
	private void Awake()
	{
		if(mInstance == null) mInstance = this;
		else if(mInstance.gameObject != this.gameObject)	Destroy (this.gameObject);
		else 												Destroy (this);
	}
	private void Start()
	{
		mNotePoints		= GameManager.Instance.GetCurrentDifficulty().mNotesPoint;
		mCurrentScore	= 0;
		mText			= gameObject.GetComponentInChildren<GUIText>();
		mText.text		= mCurrentScore.ToString();
	}
	#endregion

	#region Class Function
	public void UpdateScore()
	{
		if(GameManager.Instance.GetCurrentDifficulty().mHighScore < PointsManager.Instance.CurrentScore)
		{
			GameManager.Instance.GetCurrentDifficulty().mHighScore = PointsManager.Instance.CurrentScore;
			GameManager.Instance.SaveData();
		}
	}
	public int NotePoints
	{
		get {	return mNotePoints;		}
		set	{	mNotePoints = value;	}
	}
	public int CurrentScore
	{
		get {	return mCurrentScore;	}
		set	{	mCurrentScore = value;	}
	}
	#endregion

	#region UI
	private void OnGUI()	{	mText.text = CurrentScore.ToString();	}
	#endregion
}
