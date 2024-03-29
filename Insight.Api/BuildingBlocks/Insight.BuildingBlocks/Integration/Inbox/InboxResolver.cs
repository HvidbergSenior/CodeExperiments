﻿using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Reflection;

namespace Insight.BuildingBlocks.Integration.Inbox
{
    public class InboxResolver : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var props = type?.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                        .Select(p => base.CreateProperty(p, memberSerialization))
                    .Union(type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                               .Select(f => base.CreateProperty(f, memberSerialization)))
                    .ToList();
            props?.ForEach(p => { p.Writable = true; p.Readable = true; });
            return props!;
        }
    }
}
