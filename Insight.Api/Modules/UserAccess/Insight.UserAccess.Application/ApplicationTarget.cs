﻿using System.Reflection;

namespace Insight.UserAccess.Application
{
    public static class AssemblyReference
    {
        public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
    }
}