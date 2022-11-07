using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace RestExample.Infrastructure
{
    /// <summary>
    ///  Represents an <see cref="ActionResult"/> that when executed will produce an HTTP response of <see cref="HttpStatusCode.NotModified"/>.
    /// </summary>
    public class NotModifiedStatusCode : StatusCodeResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StatusCodeResult"/> class.
        /// </summary>
        public NotModifiedStatusCode()
            : base((int)HttpStatusCode.NotModified)
        {
        }

        /// <summary>
        /// Provides the singleton instance of <seealso cref="NotModifiedStatusCode"/>.
        /// </summary>
        public static NotModifiedStatusCode Instance { get; private set; } = new NotModifiedStatusCode();
    }
}
