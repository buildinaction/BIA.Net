namespace BIA.Net.Common.Helpers
{
    using System.IO;
    using System.Runtime.Serialization.Json;
    using System.Text;

    /// <summary>
    /// Serialization Helper
    /// </summary>
    public static class SerializationHelper
    {
        /// <summary>
        /// Reads a document stream in the JSON format and returns the deserialized object.
        /// </summary>
        /// <typeparam name="T">object type</typeparam>
        /// <param name="json">json</param>
        /// <returns>object</returns>
        public static T ConvertJsonToObject<T>(string json)
            where T : class, new()
        {
            T deserializedObject = default(T);

            if (!string.IsNullOrWhiteSpace(json))
            {
                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
                {
                    deserializedObject = new T();
                    DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedObject.GetType());
                    deserializedObject = ser.ReadObject(ms) as T;
                }
            }

            return deserializedObject;
        }
    }
}
