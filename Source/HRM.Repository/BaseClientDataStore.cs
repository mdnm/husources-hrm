using System;
using System.Linq;
using System.Threading.Tasks;
using Hanssens.Net;
using HRM.Model;
using HRM.Model.Entities;
using Microsoft.Extensions.Logging;

namespace HRM.Repository
{
    /// <summary>
    /// Stores and retrieves information about the client application 
    /// such as login credentials, messages, settings and so on
    /// in an SQLite database
    /// </summary>
    public class BaseClientDataStore : IClientDataStore
    {
        #region Public Members
        public string Key => "loggedId";
        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public BaseClientDataStore()
        {
        }

        #endregion

        #region Interface Implementation

        /// <summary>
        /// Determines if the current user has logged in credentials
        /// </summary>
        public bool HasCredentials()
        {
            return GetLoginCredentials() != null;
        }

        /// <summary>
        /// Gets the stored login credentials for this client
        /// </summary>
        /// <returns>Returns the login credentials if they exist, or null if none exist</returns>
        public Guid? GetLoginCredentials()
        {
            Guid? credentials = null;
            try
            {
                using (var storage = new LocalStorage())
                {
                    if (storage.Count > 0)
                    {
                        credentials = storage.Get<Guid>(Key);
                    }
                }
            }
            catch
            {
                // Null catch because sometimes the storage dont disposes correctly
            }

            return credentials;
        }

        /// <summary>
        /// Stores the given login credentials to the backing data store
        /// </summary>
        /// <param name="userId">The login credentials to save</param>
        /// <returns>Returns a task that will finish once the save is complete</returns>
        public void SaveLoginCredentials(Guid userId)
        {
            using (var storage = new LocalStorage())
            {
                // Clear all entries
                storage.Clear();

                // Add new one
                storage.Store(Key, userId);
                storage.Persist();
            }
        }

        /// <summary>
        /// Removes all login credentials stored in the data store
        /// </summary>
        /// <returns></returns>
        public void ClearAllLoginCredentials()
        {
            using (var storage = new LocalStorage())
            {
                storage.Clear();
            }
        }
        #endregion
    }
}
