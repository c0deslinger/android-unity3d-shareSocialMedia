using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ButtonShare : MonoBehaviour {
	
	List<Transform> animations;
	string url;
	public bool isExitButton;
	public static string currentPage = "1";

	public enum Media{
		FACEBOOK,
		TWITTER,
		WHATSAPP
	};

	public Media media;

	void OnMouseOver(){
		if (Input.GetMouseButtonDown(0)){
			if (media == Media.FACEBOOK) {
				url = "https://www.facebook.com/sharer.php?u=https://play.google.com/store/apps/details?id=" + Application.identifier;
				StartCoroutine (playSoundThenLoadURL (url, this.gameObject.transform));
			} else if (media == Media.TWITTER) {
				url = "https://www.twitter.com/home?status=https://play.google.com/store/apps/details?id=" + Application.identifier;
				StartCoroutine (playSoundThenLoadURL (url, this.gameObject.transform));
			} else if (media == Media.WHATSAPP) {
				shareWhatsapp ();
			}
		}
	}	

	IEnumerator playSoundThenLoadURL(string url, Transform button) { 
		StartCoroutine (Animate (button, 0.1f, 0.15f));
		this.gameObject.GetComponent<AudioSource>().Play();
		yield return new WaitForSeconds(this.gameObject.GetComponent<AudioSource>().clip.length);
		Application.OpenURL(url);
	}

	//Animates a button
	IEnumerator Animate(Transform button, float scaleFactor, float time)
	{
		this.gameObject.GetComponent<AudioSource>().Play ();
		Vector3 originalScale = button.localScale;
		float rate = 1.0f / time;
		float t = 0.0f;
		float d = 0;
		while (t < 1.0f)
		{
			t += Time.deltaTime * rate;
			button.localScale = originalScale + (originalScale * (scaleFactor * Mathf.Sin(d * Mathf.Deg2Rad)));
			d = 180 * t;
			yield return new WaitForEndOfFrame();
		}
		button.localScale = originalScale;
	}

	void shareWhatsapp(){
		if(!Application.isEditor)
		{
			AndroidJavaClass intentClass = new AndroidJavaClass ("android.content.Intent");
			AndroidJavaObject intentObject = new AndroidJavaObject ("android.content.Intent");
			intentObject.Call<AndroidJavaObject> ("setAction", intentClass.GetStatic<string> ("ACTION_SEND"));
			intentObject.Call<AndroidJavaObject> ("setType", "text/plain");
			intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), "Let's Play "+Application.productName+" games!");
			intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), "Hi, I just played "+Application.productName+"! this game is very fun, let's play with me!\n" +
					"Download the game on play store at "+"\nhttps://play.google.com/store/apps/details?id="+Application.identifier);
			AndroidJavaClass unity = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
			AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject> ("currentActivity");
			currentActivity.Call ("startActivity", intentObject);
		}
	}
	
}
