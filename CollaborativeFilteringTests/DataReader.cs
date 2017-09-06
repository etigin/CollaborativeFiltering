using System;
using System.IO;
using Newtonsoft.Json;

namespace CollaborativeFilteringTests
{
    public class DataReader
    {
        public static string ReadFileContent(string path)
        {
            var result = string.Empty;
            using (var fileStream = new FileStream(path, FileMode.Open))
            using (StreamReader reader = new StreamReader(fileStream))
            {
                result = reader.ReadToEnd();
            }

            return result;
        }

        public static T DeserializeData<T>(string data)
        {
            T result;
            try
            {
                result = JsonConvert.DeserializeObject<T>(data);
            }
            catch (Exception)
            {
                result = default(T);
            }

            return result;
        }
    }
}
