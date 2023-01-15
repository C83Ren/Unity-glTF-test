using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using MEC;
using SimpleJSON;
using UnityEngine.Serialization;

namespace APISystem
{
    public class APIController : MonoBehaviour
    {
        private static APIController _instance;

        [FormerlySerializedAs("baseUrl")] [Header("Server Connection")] [SerializeField]
        private string baseApiUrl;

        [SerializeField] private string baseAssetsUrl;


        private string _json;
        private string _assetDir;
        private JSONNode _pulledData;

        private string ApiUrl(string api)
        {
            var url = baseApiUrl + api;
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
            CallAPI();
        }

        public string GetAssetURL()
        {
            if (_assetDir != null)
            {
                return baseAssetsUrl + _assetDir;
            }

            return null;
        }

        private void PullAssetLink()
        {
            try
            {
                _assetDir = _pulledData["assetBundleUrl"].Value;
                Debug.Log(_assetDir);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private void SortJson()
        {
            JSONContainer container = new JSONContainer(_instance._json);
            _pulledData = container.PullData("test-orang-lari v0.0.1");
        }

        private void CallAPI()
        {
            Timing.RunCoroutine(CallAPIGetWearable());
        }

        private IEnumerator<float> CallAPIGetWearable()
        {
            var api = "/v1/wearable";
            using UnityWebRequest request = UnityWebRequest.Get(ApiUrl(api));
            Debug.Log("Get:" + api);
            yield return Timing.WaitUntilDone(request.SendWebRequest());
            Debug.Log(request.isDone);

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(request.error);
            }
            else
            {
                try
                {
                    _instance._json = request.downloadHandler.text;
                    Debug.Log(_instance._json);
                    SortJson();
                    PullAssetLink();
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }
    }
}