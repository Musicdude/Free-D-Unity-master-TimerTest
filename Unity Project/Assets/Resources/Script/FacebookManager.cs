using UnityEngine;
using System.Collections;

public class FacebookManager : MonoBehaviour 
{
	[SerializeField] private TextMesh mText;
	#region Singleton
	private static FacebookManager mInstance;
	public static FacebookManager Instance
	{
		get
		{
			if(mInstance == null)	GameObject.Find("FBManager").GetComponent<FacebookManager>();
			return mInstance;
		}
		
	}
	#endregion

	#region Unity Function
	private void Awake()
	{
		if(mInstance == null)								mInstance = this;
		else if(mInstance.gameObject != this.gameObject)	Destroy(this.gameObject);
		else 												Destroy(this);

		FB.Init(SetInit,OnHideUnity);
		DontDestroyOnLoad(gameObject);
	}
	#endregion

	#region Class Function
	public void Login()
	{
		FB.Login("email",AuthCallback);
	}
	public void PostFeed()
	{
		FB.Feed(
			link: "https://example.com/myapp/?storyID=thelarch",
			linkName: "The Larch",
			linkCaption: "I thought up a witty tagline about larches",
			linkDescription: "There are a lot of larch trees around here, aren't there?",
			picture: "https://example.com/myapp/assets/1/larch.jpg",
			callback: LogCallBack
			);
	}
	#endregion

	#region Facebook Functions
	private void SetInit()
	{
		enabled = true;
	}
	private void OnHideUnity(bool _isGameShown)
	{
		if(!_isGameShown)	Time.timeScale = 0;
		else 				Time.timeScale = 1;
	}
	private void AuthCallback(FBResult _result)
	{
		if(FB.IsLoggedIn)
		{
			// do something
			Debug.Log(FB.UserId);
		}
		else
		{
			Debug.Log("User cancelled Login");
		}
	}
	private void LogCallBack(FBResult _reponse)
	{
		Debug.Log("call login: " + FB.UserId);
		Debug.Log("login result: " + _reponse.Text);
		if (_reponse.Error != null)
		{
			Debug.LogError(_reponse.Error);
			return;
		}
	}
	#endregion
}
