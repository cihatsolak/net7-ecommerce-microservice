﻿global using CourseSales.Shared.DataTransferObjects;
global using CourseSales.Web.Exceptions;
global using CourseSales.Web.Handlers;
global using CourseSales.Web.Models;
global using CourseSales.Web.Services.Identity;
global using CourseSales.Web.Services.Users;
global using CourseSales.Web.Settings;
global using IdentityModel.Client;
global using Microsoft.AspNetCore.Authentication;
global using Microsoft.AspNetCore.Authentication.Cookies;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.Extensions.Options;
global using Microsoft.IdentityModel.Protocols.OpenIdConnect;
global using System.ComponentModel.DataAnnotations;
global using System.Diagnostics;
global using System.Globalization;
global using System.Net;
global using System.Net.Http.Headers;
global using System.Security.Claims;
global using System.Text.Json;
global using Microsoft.AspNetCore.Authorization;
global using CourseSales.Web.Models.Catalog;
global using CourseSales.Shared.Services;
global using CourseSales.Web.Services.Catalogs;
global using IdentityModel.AspNetCore.AccessTokenManagement;
global using CourseSales.Web.Services.Clients;
global using Microsoft.AspNetCore.Mvc.Rendering;
global using CourseSales.Web.Services.PhotoStocks;