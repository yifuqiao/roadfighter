using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class UserProfileManager : MonoBehaviour
{
    private const string TOKEN = "d3c440e7ec8870bf50318d40fb8c35de";
    private string m_dymanicToken = string.Empty;
    private static UserProfileManager s_instance;
    public string UserName
    {
        private set; get;
    }
    public bool UserProfileRequested
    {
        private set; get;
    }

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
        var array = Application.absoluteURL.Split('?');
        if (m_dymanicToken.Length > 1)
            m_dymanicToken = array[array.Length - 1];
        else
            m_dymanicToken = TOKEN;
        UserProfileRequested = false;
    }

    public int GetUserGameCoin()
    {
        if(PlayerPrefs.HasKey("lastLogin"))
        {
            var now = DateTime.Now;
            var lastLogin = DateTime.Parse(PlayerPrefs.GetString("lastLogin"));

            if(now.Subtract(lastLogin).Hours>24)
            {
                ModifyUserCoin(+10);
            }
        }

        return PlayerPrefs.GetInt("user_game_coin_count", 10);
    }


    public void ModifyUserCoin(int amount)
    {
        int newAmount = GetUserGameCoin() + amount;
        PlayerPrefs.SetInt("user_game_coin_count", newAmount);
        PlayerPrefs.SetString("lastLogin", System.DateTime.Now.ToString());
        PlayerPrefs.Save();
    }

    public void GetUserCode()
    {
        StartCoroutine(GetUserCodeCall());
    }

    static string GetMd5Hash(MD5 md5Hash, string input)
    {
        // Convert the input string to a byte array and compute the hash.
        byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

        // Create a new Stringbuilder to collect the bytes
        // and create a string.
        StringBuilder sBuilder = new StringBuilder();

        // Loop through each byte of the hashed data 
        // and format each one as a hexadecimal string.
        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }

        // Return the hexadecimal string.
        return sBuilder.ToString();
    }

    // Verify a hash against a string.
    static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
    {
        // Hash the input.
        string hashOfInput = GetMd5Hash(md5Hash, input);

        // Create a StringComparer an compare the hashes.
        StringComparer comparer = StringComparer.OrdinalIgnoreCase;

        if (0 == comparer.Compare(hashOfInput, hash))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    IEnumerator GetUserCodeCall()
    {
        long ticks = DateTime.UtcNow.Ticks - DateTime.Parse("01/01/1970 00:00:00").Ticks;
        ticks /= 10000000; //Convert windows ticks to seconds
        var timestamp = (int)ticks;

        var md5Instance = MD5.Create();
        string md5_token = GetMd5Hash(md5Instance, "eee38b40f8c67e261a9a9c13871aa8ba");
        string md5_time = GetMd5Hash(md5Instance, timestamp.ToString());
        string md5_total = GetMd5Hash(md5Instance, md5_token + md5_time);

        /*Debug.LogWarning("verify result = " + VerifyMd5Hash(md5Instance, md5_token + md5_time, md5_total));

        Debug.LogWarning("user_token = " + TOKEN);
        Debug.LogWarning("time " + timestamp);

        Debug.LogWarning("token = " + md5_total);
        Debug.LogWarning("md5_token = " + md5_token);
        Debug.LogWarning("md5_time = " + md5_time);
        Debug.LogWarning("md5_total = " + md5_total);
        */
        WWWForm form = new WWWForm();
        form.AddField("user_token", m_dymanicToken); // from url
        form.AddField("time", timestamp);
        form.AddField("token", md5_total);
        form.AddField("Access-Control-Allow-Credentials", "true");
        form.AddField("Access-Control-Allow-Headers", "Accept, X-Access-Token, X-Application-Name, X-Request-Sent-Time");
        form.AddField("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
        form.AddField("Access-Control-Allow-Origin", "*");

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
//                Debug.Log(jsonResult);
                JSONObject obj = new JSONObject(jsonResult);
                var msgObj = obj.GetField("msg");
                if(msgObj.HasField("username"))
                    UserName = msgObj.GetField("username").str;

            }
        }

        UserProfileRequested = true;
    }
}
