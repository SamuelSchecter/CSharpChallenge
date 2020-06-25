using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class Json : MonoBehaviour
    {
        public Text titleText, headerText, dataText, errorText;

        /*Get data of json object file, assuming the structure: Title, ColumnHeaders, Data (number of columnsheaders and/or rows may vary)*/
        /*but assuming the structure remains*/
        /// <summary>
        /// Entry point of proyect and button click
        /// </summary>
        public void Start()
        {
            string jsonPath;
            try
            {
                titleText.text = "";
                headerText.text = "";
                dataText.text = "";
                errorText.text = "";
                jsonPath = Application.dataPath + "/StreamingAssets/JsonChallenge.json";
                ParseJson(jsonPath);
            }
            catch (Exception ex)
            {
                errorText.text = ex.Message;
            }

        }

        /// <summary>
        /// function that deserialize a json according to structure [Title, ColumnHeaders, Data]
        /// </summary>
        /// <param name="jsonPath"></param>
        private void ParseJson(string jsonPath)
        {
            object title, colsHeader, data;
            bool status = true;
            string jsonString;
            try
            {
                if (File.Exists(jsonPath))
                {
                    /*Read json content*/
                    jsonString = File.ReadAllText(jsonPath);

                    /*Deserialize json object*/
                    JObject jObject = JObject.Parse(jsonString);

                    /*jobject to dictionary <string, object>*/
                    Dictionary<string, object> objDictionary = jObject.ToObject<Dictionary<string, object>>();

                    /*Get value of json title*/
                    status = objDictionary.TryGetValue("Title", out title);

                    if (status)
                    {
                        /*Get Column headers structure*/
                        status = objDictionary.TryGetValue("ColumnHeaders", out colsHeader);
                        if (status)
                            /*Get data array*/
                            status = objDictionary.TryGetValue("Data", out data);
                        else
                            return;//If there's no match exit subroutine
                    }
                    else
                        return;//If there's no match exit subroutine
                    printJson(title, colsHeader, data);
                }
            }
            catch (Exception ex)
            {
                errorText.text = ex.Message;
            }
        }

        /// <summary>
        /// Print every json object data on screen
        /// </summary>
        /// <param name="title"></param>
        /// <param name="colsHeader"></param>
        /// <param name="data"></param>
        private void printJson(object title, object colsHeader, object data)
        {
            try
            {
                titleText.text = title.ToString();
                
                /*Get each column header value in the json object ColumnHeaders*/
                foreach (JToken token in ((JArray)colsHeader).Children())
                {
                    headerText.text += token + ";\t\t";//Set value of concatenated header text
                }

                /*Parse data json object to an array object (JArray)*/
                JArray a = JArray.Parse(data.ToString());

                /*Get each object of the data array*/
                foreach (JToken token in ((JArray)data).Children())
                {
                    /*Items object to array (JProperty)*/
                    var dataObject = token.Children<JProperty>();

                    /*Get each element of the data object*/
                    foreach (JToken item in dataObject.Values())
                    {
                        dataText.text += item + ";\t\t";//Set value of concatenated data text
                    }
                    dataText.text += "\n";
                }
            }
            catch (Exception ex)
            {
                errorText.text = ex.Message;
            }
        }
    }
}
