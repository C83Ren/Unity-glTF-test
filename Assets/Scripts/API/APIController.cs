namespace APISystem
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Networking;
    using MEC;

    public class APIController:MonoBehaviour
    {
        private static APIController _instance;

        [Header("Server Connection")]
        [SerializeField] private string baseUrl;

        private string json;

        private string ApiUrl(string api)
        {
            var url = baseUrl + api;
            return url;
        }

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
            }
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            
        }

        public IEnumerator<float> CallAPIGetWearable()
        {
            var api = "/v1/wearable";
            using (UnityWebRequest request = UnityWebRequest.Get(ApiUrl(api)))
            {
                Debug.Log("Get:" + api);
                yield return Timing.WaitUntilDone(request.SendWebRequest());

                if (request.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.Log(request.error);
                }
                else
                {
                    try
                    {
                        json = request.downloadHandler.text.ToString();
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                    
                }
            }
        }
    }
}
