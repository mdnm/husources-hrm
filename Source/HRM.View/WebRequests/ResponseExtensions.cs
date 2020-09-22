using HRM.Model;
using System.Threading.Tasks;
using static HRM.View.DI;

namespace HRM.View
{
    /// <summary>
    /// Extension methods for the <see cref="ResponseExtensions"/> class
    /// </summary>
    public static class ResponseExtensions
    {
        /// <summary>
        /// Checks the response for any errors, displaying them if there are any
        /// </summary>
        /// <param name="response">The response to check</param>
        /// <returns>Returns true if there was an error, or false if all was OK</returns>
        public static async Task<bool> HandleErrorIfFailedAsync(this Response response)
        {
            // If there was response, good data, or a response with no error message...
            if (response != null && !response.Error)
            {
                // All was OK, so return false for no error
                return false;
            }

            // Default error message
            var message = "Erro interno desconhecido";

            if (!string.IsNullOrEmpty(response.Message))
                // Set message to servers response
                message = response.Message;


            // Display error
            await UI.ShowMessage(new MessageBoxDialogViewModel
            {
                Title = "Erro",
                Message = message
            });

            // Return that we had an error
            return true;
        }
    }
}
