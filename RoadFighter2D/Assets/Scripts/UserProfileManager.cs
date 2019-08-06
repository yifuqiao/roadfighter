using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UserProfileManager : MonoBehaviour
{
	private static UserProfileManager s_instance;
	public static UserProfileManager Instance
	{
        get
		{
            if(s_instance == null)
			{
                GameObject go = new GameObject("UserProfileManager");
                s_instance = go.AddComponent<UserProfileManager>();
                DontDestroyOnLoad(go);
			}
            return s_instance;
		}
	}

    private void Awake()
    {
        //Application.absoluteURL
    }

    public int GetUserGameCoin()
    {
        return PlayerPrefs.GetInt("user_game_coin_count", 10);
    }


    public void ModifyUserCoin(int amount)
    {
        int newAmount = GetUserGameCoin() + amount;
        PlayerPrefs.SetInt("user_game_coin_count", newAmount);
        PlayerPrefs.Save();
    }

    public string Md5Sum(string strToEncrypt)
    {
        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        byte[] bytes = ue.GetBytes(strToEncrypt);

        // encrypt bytes
        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);

        // Convert the encrypted bytes back to a string (base 16)
        string hashString = "";

        for (int i = 0; i < hashBytes.Length; i++)
        {
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }

        return hashString.PadLeft(32, '0');
    }

    public void GetUserCode()
    {
        StartCoroutine(GetUserCodeCall());
    }

    private const string TOKEN = "eee38b40f8c67e261a9a9c13871aa8ba";
    IEnumerator GetUserCodeCall()
    {
        string md5 = string.Empty;
        md5 = Md5Sum(Md5Sum(TOKEN) + "." + Md5Sum(System.DateTime.Now.Second.ToString()));

        WWWForm form = new WWWForm();
        form.AddField("user_token", 12345); // from url
        form.AddField("time", System.DateTime.Now.Second);
        form.AddField("token", md5);

        using (UnityWebRequest www = UnityWebRequest.Post("https://www.qqbctoken.com/index/game/Get_user_code", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string jsonResult = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
                Debug.Log(jsonResult);
            }
        }
    }
}
