using UnityEngine;
using GooglePlayGames.BasicApi;
using GooglePlayGames;

namespace ArtboxGames
{
	public class Leadersboard : MonoBehaviour
	{

		private bool isLogin = false;

		// Use this for initialization
		void Start()
		{
			DontDestroyOnLoad(gameObject);
			Invoke(nameof(InitAndSignin), 1f);
		}

		void InitAndSignin()
		{
			PlayGamesPlatform.DebugLogEnabled = true;
			PlayGamesPlatform.Activate();
			PlayGamesPlatform.Instance.Authenticate(OnSignInResult);
		}

		private void OnSignInResult(SignInStatus signInStatus)
		{
			if (signInStatus == SignInStatus.Success)
			{
				Debug.Log("=== GPG Authenticated. Hello");
			}
			else
			{
				Debug.Log("*** GPG Failed to authenticate with " + signInStatus);
			}
		}

		public void ReportScore(long score)
		{
			Social.ReportScore(score, "CgkI6_3Lkb4fEAIQAQ", (bool success) =>
			{
				// handle success or failure
				if (success)
				{
					//Debug.Log ("==== time reporting succes");
				}
				else
				{
					//Debug.Log ("==== time reporting failed");
				}
			});
		}

		public void ShowLeadersboard()
		{
			if (Social.localUser.authenticated)
			{
				Social.ShowLeaderboardUI();
			}
			else
			{
				InitAndSignin();
			}
		}
	}
}