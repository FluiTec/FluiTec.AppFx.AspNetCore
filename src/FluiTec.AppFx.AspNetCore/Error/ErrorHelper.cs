using System;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FluiTec.AppFx.AspNetCore.Error
{
    /// <summary>	An error helper. </summary>
    public static class ErrorHelper
    {
        /// <summary>	Gets an error. </summary>
        /// <param name="context">		 	The context. </param>
        /// <param name="logger">		 	The logger. </param>
        /// <param name="handlingAction">	The handling action. (For example sending a mail)</param>
        /// <returns>	The error. </returns>
        /// <remarks>
        ///     This method is intended to be used in the controller invoked by
        ///     <see cref="FluiTec.AppFx.AspNetCore.Configuration.ErrorOptions" />.ErrorHandlingPath.
        ///     It should first call this method (and probably send a mail using the handlingAction)
        ///     and afterwars - display a view with the exception-data.
        /// </remarks>
        public static ErrorMessage GetError(HttpContext context, ILogger logger, Action<ErrorMessage> handlingAction)
        {
            // get exception-data
            var exceptionFeature = context.Features.Get<IExceptionHandlerPathFeature>();
            var exception = exceptionFeature?.Error;
            var route = exceptionFeature?.Path;

            var message = new ErrorMessage {Exception = exception, Route = route};

            // only execute if there was a real exception
            if (exception == null) return message;
            logger.LogError(0, exception, "Unhandled exception");

            try
            {
                handlingAction.Invoke(message);
            }
            catch (Exception e)
            {
                logger.LogError(0, e, "Unhandled exception in error-handling api");
            }

            return message;
        }
    }
}