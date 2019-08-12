using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class WebServiceAPITester : MonoBehaviour
{
    private const string TOKEN = "d3c440e7ec8870bf50318d40fb8c35de";
    public void ReCharge()
    {
        StartCoroutine(ReChargeCall());
    }

    IEnumerator ReChargeCall()
    {
        WWWForm form = new WWWForm();
        form.AddField("user_code", "jason");
        form.AddField("amount", 10);
        form.AddField("orderid", 1);// 订单号
        form.AddField("code", 1); // 谷歌验证码
        form.AddField("time", System.DateTime.Now.Second); // 当前秒
        form.AddField("token", TOKEN); // 连接


        using (UnityWebRequest www = UnityWebRequest.Post("https://www.qqbctoken.com/index/game/Game_recharge", form))
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
        var timestamp =(int)ticks;

        var md5Instance = MD5.Create();
        string md5_token = GetMd5Hash(md5Instance, "eee38b40f8c67e261a9a9c13871aa8ba");
        string md5_time = GetMd5Hash(md5Instance, timestamp.ToString());
        string md5_total = GetMd5Hash(md5Instance, md5_token + md5_time);

        Debug.LogWarning("verify result = " + VerifyMd5Hash(md5Instance, md5_token + md5_time, md5_total));

        Debug.LogWarning("user_token = " + TOKEN);
        Debug.LogWarning("time " + timestamp);

        Debug.LogWarning("token = " + md5_total);
        Debug.LogWarning("md5_token = " + md5_token);
        Debug.LogWarning("md5_time = " + md5_time);
        Debug.LogWarning("md5_total = " + md5_total);

        WWWForm form = new WWWForm();
        form.AddField("user_token", TOKEN); // from url
        form.AddField("time", timestamp);
        form.AddField("token", md5_total);

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

    public static string GetTimestamp( DateTime value)
    {
        return value.ToString("yyyyMMddHHmmssfff");
    }
}
