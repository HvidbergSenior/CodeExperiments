﻿using Microsoft.AspNetCore.Mvc.Testing;

namespace Insight.Tests.End2End
{
    public class WebAppFactory<T> : WebApplicationFactory<T> where T : class
    {
        public WebAppFactory()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");
        }

    }
}