using System.Text.Json;

namespace TransGr8_DD_Test.Helpers
{
    /// <summary>
    /// Json File Reader
    /// </summary>
    public static class JsonFileReader
    {
        /// <summary>
        /// Read json file async
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static async Task<T> ReadAsync<T>(string filePath)
        {
            using FileStream stream = File.OpenRead(filePath);
            return await JsonSerializer.DeserializeAsync<T>(stream);
        }
    }
}
