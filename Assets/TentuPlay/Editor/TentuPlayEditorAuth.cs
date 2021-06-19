using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TentuPlay
{
    public class TentuPlayAuthSettings : ScriptableObject //Don't bind this ScriptableObject to .asset file. It should only be instantiated in in-memory.
    {
        public const string tpSettingsAssetName = "TentuPlayAuthSettings";

        private static TentuPlayAuthSettings instance;

        public static TentuPlayAuthSettings Instance
        {
            get
            {
                if (ReferenceEquals(instance, null))
                {
                    instance = Resources.Load(tpSettingsAssetName) as TentuPlayAuthSettings;

                    if (ReferenceEquals(instance, null))
                    {
                        // If not found, autocreate the asset object.
                        instance = CreateInstance<TentuPlayAuthSettings>();
                    }
                }
                return instance;
            }
        }

        [SerializeField]
        private string apiKey = "";

        [SerializeField]
        private string secret = "";

        public static string ApiKey
        {
            get { return Instance.apiKey; }
            set { Instance.apiKey = value; }
        }

        public static string Secret
        {
            get { return Instance.secret; }
            set { Instance.secret = value; }
        }
    }
}