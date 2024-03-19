using AutoFixture;
using Insight.UserAccess.Domain.Auth;
using Insight.UserAccess.Domain.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insight.UserAccess.Test
{
    internal static class Any
    {
        private static readonly Fixture any = new();

        public static T Instance<T>()
        {
            return any.Create<T>();
        }

        internal static Email Email()
        {
            return Domain.User.Email.Create($"{Instance<string>()}@{Instance<string>()}");
        }

        internal static Guid Guid()
        {
            return Instance<Guid>();
        }

        internal static Password Password()
        {
            return Domain.Auth.Password.Create(Instance<string>());
        }

        internal static UserName UserName()
        {
            return Domain.User.UserName.Create(Instance<string>());
        }
        internal static FirstName FirstName()
        {
            return Domain.User.FirstName.Create(Instance<string>());
        } 
        internal static LastName LastName()
        {
            return Domain.User.LastName.Create(Instance<string>());
        }
        internal static ApplicationUser ApplicationUser()
        {
            return Instance<ApplicationUser>();
        }

        public static Mock<UserManager<TUser>> TestUserManager<TUser>() where TUser : class
        {
            var userStoreMock2 = new Mock<IUserStore<TUser>>();
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var dasMock = new Mock<UserManager<TUser>>(
                userStoreMock2.Object, null, null, null, null, null, null, null, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            return dasMock;
        }
        public static RoleManager<TRole> TestRoleManager<TRole>(IRoleStore<TRole>? store = null) where TRole : class
        {
            var logger = NullLogger<RoleManager<TRole>>.Instance;

            store = store ?? new Mock<IRoleStore<TRole>>().Object;
            var roles = new List<IRoleValidator<TRole>>();
            roles.Add(new RoleValidator<TRole>());
            return new RoleManager<TRole>(store, roles,
                MockLookupNormalizer(),
                new IdentityErrorDescriber(),
                logger);
        }

        public static ILookupNormalizer MockLookupNormalizer()
        {
            var normalizerFunc = new Func<string, string>(i =>
            {
                if (i == null)
                {
                    return string.Empty;
                }
                else
                {
                    return Convert.ToBase64String(Encoding.UTF8.GetBytes(i)).ToUpperInvariant();
                }
            });
            var lookupNormalizer = new Mock<ILookupNormalizer>();
            lookupNormalizer.Setup(i => i.NormalizeName(It.IsAny<string>())).Returns(normalizerFunc);
            lookupNormalizer.Setup(i => i.NormalizeEmail(It.IsAny<string>())).Returns(normalizerFunc);
            return lookupNormalizer.Object;
        }
    }
}
