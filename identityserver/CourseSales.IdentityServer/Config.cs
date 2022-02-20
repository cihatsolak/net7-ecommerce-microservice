// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;

namespace CourseSales.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<ApiResource> ApiResources => new ApiResource[]
        {
            new ApiResource("resource_catalog")
            {
                Scopes =
                {
                    "catalog_fullpermission"
                }
            },
            new ApiResource("resource_photo_stock")
            {
                Scopes =
                {
                    "photo_stock_fullpermission"
                }
            },
            new ApiResource("resource_basket")
            {
                Scopes =
                {
                    "basket_fullpermission"
                }
            },
            new ApiResource("resource_discount")
            {
                Scopes =
                {
                    "discount_fullpermission"
                }
            },
            new ApiResource("resource_order")
            {
                Scopes =
                {
                    "order_fullpermission"
                }
            },
            new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
        };

        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                       new IdentityResources.Email(), //email
                       new IdentityResources.OpenId(), //sub
                       new IdentityResources.Profile(),
                       new IdentityResource()
                       {
                           Name = "Roles",
                           DisplayName = "Roles",
                           Description = "User roles",
                           UserClaims = new[]{"roles"}
                       }
                   };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
              new ApiScope("catalog_fullpermission","Full access to catalog api"),
              new ApiScope("photo_stock_fullpermission","Full access to photo stock api"),
              new ApiScope("basket_fullpermission", "Full access to basket api"),
              new ApiScope("discount_fullpermission", "Full access to discount api"),
              new ApiScope("order_fullpermission", "Full access to order api"),
              new ApiScope(IdentityServerConstants.LocalApi.ScopeName),
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client()
                {
                    ClientName ="Asp.Net Core MVC",
                    ClientId = "WebMvcClient",
                    ClientSecrets =
                    {
                        new Secret("secret.".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes =
                    {
                        "catalog_fullpermission",
                        "photo_stock_fullpermission",
                        IdentityServerConstants.LocalApi.ScopeName
                    }
                },
                new Client()
                {
                    ClientName ="Asp.Net Core MVC",
                    ClientId = "WebMvcClientForUser",
                    ClientSecrets =
                    {
                        new Secret("secret.".Sha256())
                    },
                    AllowOfflineAccess = true,
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes =
                    {
                        "basket_fullpermission",
                        "discount_fullpermission",
                        "order_fullpermission",
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        IdentityServerConstants.LocalApi.ScopeName,
                        "roles"
                    },
                    AccessTokenLifetime = 1 * 60 * 60,
                    RefreshTokenExpiration = TokenExpiration.Absolute,
                    AbsoluteRefreshTokenLifetime = (int)(DateTime.Now.AddDays(60)-DateTime.Now).TotalSeconds,
                    RefreshTokenUsage = TokenUsage.ReUse, //refresh token 1 kere mi kullanilabilir tekrar tekrar kullanılabilir mi?
                }
            };
    }
}