using Dna;
using HRM.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HRM.Repository
{
    /// <summary>
    /// Extension methods for the <see cref="FrameworkConstruction"/>
    /// </summary>
    public static class FrameworkConstructionExtensions
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public static FrameworkConstruction AddClientDataStore(this FrameworkConstruction construction)
        {
            // Add client data store for easy access/use of the backing data store
            construction.Services.AddTransient<IClientDataStore>(provider => new BaseClientDataStore());

            // Return framework for chaining
            return construction;
        }
    }
}
