using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace OpsMain.Client.Shared.Component
{
    internal static class AttributeAuthorizeDataCache
    {
        private static readonly ConcurrentDictionary<Type, IAuthorizeData[]?> _cache = new ConcurrentDictionary<Type, IAuthorizeData[]?>();

        public static IAuthorizeData[] GetAuthorizeDataForType(Type type)
        {
            if (!_cache.TryGetValue(type, out var result))
            {
                result = ComputeAuthorizeDataForType(type);
                _cache[type] = result; 
            }
            return result;
        }

        private static IAuthorizeData[] ComputeAuthorizeDataForType(Type type)
        {
            // Allow Anonymous skips all authorization
            var allAttributes = type.GetCustomAttributes(inherit: true);
            List<IAuthorizeData> authorizeDatas = null;
            for (var i = 0; i < allAttributes.Length; i++)
            {
                if (allAttributes[i] is IAllowAnonymous)
                {
                    return null;
                }

                if (allAttributes[i] is IAuthorizeData authorizeData)
                {
                    authorizeDatas ??= new List<IAuthorizeData>();
                    authorizeDatas.Add(authorizeData);
                }
            }

            return authorizeDatas?.ToArray();
        }
    }
}
