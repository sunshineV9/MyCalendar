using System.Threading.Tasks;

namespace MyCalendar.Mobile.Common.Services.RestService
{
    /// <summary>
    /// Handles REST API calls
    /// </summary>
    public interface IRestService
    {
        /// <summary>
        /// Sends post request to specified endpoint on the backend
        /// </summary>
        /// <param name="requestUri">Ednpoint uri from backend</param>
        /// <param name="serializedObject">Optional, Json string object</param>
        /// <returns>Deserialized object, if response</returns>
        public Task<object> PostAsync(string requestUri, string serializedObject = null);
    }
}
