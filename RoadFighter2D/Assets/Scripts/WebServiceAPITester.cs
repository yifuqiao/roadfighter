using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WebServiceAPITester : MonoBehaviour
{
    private const string TOKEN = "eee38b40f8c67e261a9a9c13871aa8ba";
    public void ReCharge()
    {
        StartCoroutine(ReChargeCall());
    }

    IEnumerator ReChargeCall()
    {
        WWWForm form = new WWWForm();
        form.AddField("user_code", "jason");
        form.AddField("amount", 10);
        form.AddField("orderid", 1);
        form.AddField("code", 1);
        form.AddField("time", System.DateTime.Now.Millisecond);
        form.AddField("token", TOKEN);


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

    IEnumerator GetUserCodeCall()
    {
        WWWForm form = new WWWForm();
        form.AddField("user_token", 12345);
        form.AddField("time", System.DateTime.Now.Millisecond);
        form.AddField("token", TOKEN);


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
