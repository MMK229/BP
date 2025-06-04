using UnityEngine;
using System;
using UnityEngine.Serialization;



namespace io.agora.rtc.demo
{
    [CreateAssetMenu(menuName = "Agora/AppIdInput", fileName = "AppIdInput", order = 1)]
    [Serializable]
    public class AppIdInput : ScriptableObject
    {
        [FormerlySerializedAs("APP_ID")]
        [SerializeField]
        public string appID = "42bef5727d2a468190b11cc1d533a7a7";

        [FormerlySerializedAs("TOKEN")]
        [SerializeField]
        public string token = "007eJxTYPDk2P5T4TlXjG6lXObdl7rpyVwnPxwX07bPmxL35/wTpXgFBhOjpNQ0U3Mj8xSjRBMzC0NLgyRDw+RkwxRTY+NE80Rz16bw9IZARoa8nRwsjAwQCOKzMnik5uTkMzAAAAc4HhE=";

        [FormerlySerializedAs("CHANNEL_NAME")]
        [SerializeField]
        public string channelName = "Hello";
    }
}
