using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace RestExample.Infrastructure
{
    public class NotModifiedStatusCode : StatusCodeResult
    {
        public NotModifiedStatusCode()
            : base((int)HttpStatusCode.NotModified)
        {
        }

        public static NotModifiedStatusCode Instance { get; private set; } = new NotModifiedStatusCode();
    }
}
