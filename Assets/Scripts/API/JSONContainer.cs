using System;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

namespace APISystem
{
    public class JSONContainer
    {
        private JSONNode _parsedRawData;
        private List<DataModel> _sanitizedData = new List<DataModel>();

        public JSONContainer(string jsonFile)
        {
            if (jsonFile != null)
            {
                try
                {
                    _parsedRawData = JSON.Parse(jsonFile);
                    SanitizeData();
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
            else
            {
                Debug.LogError("JSON File is Null!");
            }
        }

        public JSONObject PullData(string wearableIdSearchKey)
        {
            DataModel searchResult = _sanitizedData.Find(entry => entry.wearableId == wearableIdSearchKey);
            return searchResult.fileMeta;
        }

        private void SanitizeData()
        {
            JSONArray _rootRawData;
            _rootRawData = _parsedRawData.AsArray;
            foreach (JSONObject entry in _rootRawData)
            {
                DataModel sanitizedEntry;
                sanitizedEntry.wearableId = entry["wearableId"].Value;
                sanitizedEntry.fileMeta = entry["fileMeta"].AsObject;
                _sanitizedData.Add(sanitizedEntry);
            }
        }
        
        
        
    }
}