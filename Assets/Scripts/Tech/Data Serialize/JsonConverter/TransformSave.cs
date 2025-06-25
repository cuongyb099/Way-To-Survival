using KatLib.Data_Serialize.JsonConverter;
using Newtonsoft.Json;
using UnityEngine;

namespace KatLib.Data_Serialize
{
    public class TransformSave
    {
        [JsonConverter(typeof(Vector3Converter))]
        public Vector3 Position;
        [JsonConverter(typeof(Vector3Converter))]
        public Vector3 Rotation;
        [JsonConverter(typeof(Vector3Converter))]
        public Vector3 Scale;
    }
}
