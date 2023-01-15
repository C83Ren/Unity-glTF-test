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
                    Debug.Log(_parsedRawData);
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

        public JSONNode PullData(string wearableIdSearchKey)
        {
            DataModel searchResult =
                _sanitizedData.Find(entry => entry.wearableName == wearableIdSearchKey); //why not dictionary?
            return searchResult.fileMeta;
        }

        private void SanitizeData()
        {
            JSONArray rootRawData;
            rootRawData = _parsedRawData["data"].AsArray;
            Debug.Log(_parsedRawData["data"].AsArray);
            foreach (JSONNode entry in rootRawData)
            {
                DataModel sanitizedEntry;
                sanitizedEntry.wearableName = entry["wearableName"].Value;
                sanitizedEntry.fileMeta = entry["fileMeta"];
                _sanitizedData.Add(sanitizedEntry);
                Debug.Log(entry);
                Debug.Log(sanitizedEntry.wearableName + "," + entry["fileMeta"]);
            }
        }
    }
}