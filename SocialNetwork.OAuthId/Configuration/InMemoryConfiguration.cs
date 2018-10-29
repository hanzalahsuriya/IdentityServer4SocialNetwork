using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SocialNetwork.OAuthId.Configuration
{
    public class InMemoryConfiguration
    {
        public static IEnumerable<ApiResource> ApiResources()
        {
            return new[]
            {
                new ApiResource()
                {
                    ApiSecrets = new List<Secret>(),
                    UserClaims = new List<string> { "email1" },
                    Scopes = new List<Scope>()
                    {
                        new Scope()
                        {
                            Name = "socialnetwork",
                            DisplayName = "Social Network",
                            ShowInDiscoveryDocument = true,
                            UserClaims = new[]
                            {
                                "email1"
                            }
                        },
                        new Scope()
                        {
                            Name = "socialnetwork_readonly",
                            DisplayName = "Social Network (ReadOnly)",
                            ShowInDiscoveryDocument = true
                        }
                    }
                }
            };
        }

        public static IEnumerable<IdentityResource> IdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
            };
        }

        public static IEnumerable<Client> Clients()
        {
            return new[]
            {
                new Client()
                {
                    ClientId = "socialnetwork",
                    ClientSecrets = new[]
                    {
                        new Secret("client_secret".Sha256())
                    },
                    Claims = new List<Claim> {
                        new Claim("IsAdmin", "sdsadas")
                    },
                  
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AllowedScopes = new[] { "socialnetwork" },
                },
                new Client()
                {
                    ClientId = "socialnetwork_implicit",
                    ClientSecrets = new[]
                    {
                        new Secret("client_secret".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowedScopes = new[] 
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "socialnetwork"
                    },
                    // where to redirect to after login
                    RedirectUris = { "http://localhost:5002/signin-oidc" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },
                    AllowAccessTokensViaBrowser = true
                },

                new Client()
                {
                    ClientId = "socialnetwork_hybrid",
                    ClientSecrets = new[]
                    {
                        new Secret("client_secret".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    AllowedScopes = new[]
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "socialnetwork"
                    },
                    // where to redirect to after login
                    RedirectUris = { "http://localhost:5002/signin-oidc" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },
                    AllowAccessTokensViaBrowser = true,
                    AllowOfflineAccess = true
                }
            };
        }

        public static IEnumerable<TestUser> Users()
        {
            return new[]
            {
                new TestUser()
                {
                    SubjectId = "1",
                    Username = "hanzalah@gmail.com",
                    Password = "password",
                    Claims = new[]
                    {
                        new Claim("email","hanzalah@gmail.com")
                    }
                }
            };
        }
    }
}
