using HRM.Model.Entities;
using System;
using System.Threading.Tasks;

namespace HRM.Model
{
    /// <summary>
    /// Stores and retrieves information about the client application 
    /// such as login credentials, messages, settings and so on
    /// </summary>
    public interface IClientDataStore
    {
        /// <summary>
        /// Determines if the current user has logged in credentials
        /// </summary>
        bool HasCredentials();

        /// <summary>
        /// Gets the stored login credentials for this client
        /// </summary>
        /// <returns>Returns the login credentials if they exist, or null if none exist</returns>
        Guid? GetLoginCredentials();

        /// <summary>
        /// Stores the given login credentials to the backing data store
        /// </summary>
        /// <param name="userId">The login credentials to save</param>
        /// <returns>Returns a task that will finish once the save is complete</returns>
        void SaveLoginCredentials(Guid userId);

        /// <summary>
        /// Removes all login credentials stored in the data store
        /// </summary>
        /// <returns></returns>
        void ClearAllLoginCredentials();
    }
}
