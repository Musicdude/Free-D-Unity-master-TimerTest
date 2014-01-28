using UnityEngine;
using System.Collections;

[System.Serializable]
public class Transition : MonoBehaviour 
{
	#region Variables
	/***	PUBLIC FIELDS  ***/
	public float 		mStartAlpha			= 1; 		// Alpha start value
	public float 		mFadeDuration		= 2;   		// Default time a fade takes in seconds
	public bool 		mTransitionComplete = false;	// Flag to check if transition completed
	public bool 		mFadeIn			  	= false;	// Flag to check if fading out or in
	
	/*** 	PRIVATE FIELDS	***/
	private float mCurrentAlpha = 1; 				// Current alpha of the texture
	private float mCurrentDuration; 				// Current duration of the fade
	private int   mFadeDirection = -1; 				// Direction of the fade
	private float mTargetAlpha = 0; 				// Fade alpha to
	private float mAlphaDifference = 0; 			// Alpha difference
	[SerializeField] private GUITexture mFader;		// Fader 
	#endregion

	#region Class Function
	/*** 	FADE METHODS	***/
	public void FadeIn(float _duration, float _to)
	{
		mFadeIn	= true;
		mCurrentDuration = _duration;  										// Set fade duration    
		mTargetAlpha = _to; 												// Set target alpha
		mAlphaDifference = Mathf.Clamp01(mCurrentAlpha - mTargetAlpha);		// Difference
		mFadeDirection = -1; 												// Set direction to Fade in
		mTransitionComplete = false;										// Set The Flag
	}
	public void FadeIn()					{	FadeIn(mFadeDuration, 0.0f);   }
	public void FadeIn(float _duration)     {  	FadeIn(_duration, 0);  			}
	
	public void FadeOut(float _duration, float _to)
	{
		mFadeIn = false;
		mCurrentDuration =  _duration; 										// Set fade duration   
		mTargetAlpha = _to; 												// Set target alpha  
		mAlphaDifference = Mathf.Clamp01(mTargetAlpha - mCurrentAlpha); 	// Difference
		mFadeDirection = 1;  												// Set direction to fade out
		mTransitionComplete = false;										// Set The Flag
	}
	public void FadeOut()					{	FadeOut(mFadeDuration, 1);	}    
	public void FadeOut(float _duration)    {   FadeOut(_duration, 1);		}	
	/*** 	SCENE FADEIN	***/
	public void Init()
	{
		mFader.color 	= new Color (255,255,255,mStartAlpha);
		mCurrentAlpha	= mStartAlpha;
	}
	#endregion

	#region Unity Function
	private void Update()
	{   
		// Fade alpha if active
		if ((mFadeDirection == -1 && mCurrentAlpha > mTargetAlpha) ||
		    (mFadeDirection == 1 && mCurrentAlpha < mTargetAlpha))
		{
			// Advance fade by fraction of full fade time
			mCurrentAlpha += (mFadeDirection * mAlphaDifference) * (Time.deltaTime / mCurrentDuration);
			// Clamp to 0-1
			mCurrentAlpha = Mathf.Clamp01(mCurrentAlpha);
		}
		else 
		{	mTransitionComplete = true;	}
		// Draw only if not transculent
		if (mCurrentAlpha > 0)
		{	mFader.color = new Color (255,255,255,mCurrentAlpha);		}
	}
	#endregion
}
