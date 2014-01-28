using UnityEngine;
using System.Collections;

/* <summary>
 * The Player Controller will have all the required component
 * that a player should have. It is also a singleton class.
 * </summary>
 */

[RequireComponent (typeof(PlayerMovement))]
[RequireComponent (typeof(UIHealthBar))]
public class PlayerController : MonoBehaviour
{
	#region Variables
	[SerializeField] private Transform 		mModelTransform;	// Model Transformation
	[SerializeField] private UIHealthBar	mHealth;			// Health of the Player
	#endregion

	#region Singleton
	private static PlayerController mInstance;		// the Player Controller Instance
	public static PlayerController Instance			// Getting the player Instance
	{
		get
		{
			if(mInstance == null)	
				mInstance = Resources.Load("Player") as PlayerController;
			return mInstance;
		}
	}
	#endregion

	#region Unity Function
	private void Awake()
	{
		// Checking if there are any duplicates
		if(mInstance == null)			mInstance = this;
		else if(mInstance != this)		
		{
			if(mInstance.gameObject != this.gameObject)	Destroy(gameObject);
			else 										Destroy(this);
		}
	}
	private void Start()
	{
	mHealth = gameObject.GetComponent<UIHealthBar>();	// Get the Health Script
		mHealth.DestroyGameObject += DestroyGameObject;		// Attach the Function
	}
	// Update is called once per frame
	private void Update () 	
	{
		if (UpdateMovementHook != null) UpdateMovementHook();	/* Firing the Event	*/	
	}
	#endregion

	#region Class Function
	public Transform GetModelTransform()		{	return mModelTransform;			}
	public void AddHealth(float _value)			{	mHealth.AddHealth(_value);		}
	public void SubtractHealth(float _value)	{	mHealth.SubtractHealth(_value);	}

	private void DestroyGameObject() 
	{
		Debug.Log("You Died!");
		EffectManager.Instance.PlayExplosion(2.0f,gameObject);
		PointsManager.Instance.UpdateScore();
		StartCoroutine( GameManager.Instance.LoadLevel(GameManager.Instance.LevelToLoad,ScreenType.main) );
	}
	#endregion

	#region Delegate
	public delegate void UpdateMovementDelegate();			// The function type
	public event UpdateMovementDelegate UpdateMovementHook;	// The hook that we will be using to at other script
	#endregion
}
