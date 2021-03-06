﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Pati.Web.Dtos;
using Pati.Web.ExtensionMethods;
using Pati.Web.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Pati.Web.CustomFilters
{
    public class JwtAuthorize : ActionFilterAttribute
    {
        public string Roles { get; set; }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var token = context.HttpContext.Session.GetString("token");
            
            if (string.IsNullOrWhiteSpace(token))
            {
                context.Result = new RedirectToActionResult("Login", "Home", new { area = "" });
                return;
            }

            /// Get Active User with Token
            using var httpClient = new HttpClient();
            httpClient.BaseAddress =new Uri(StaticVars.BaseAPIAdress);
            httpClient.AddJwtTokenToHeader(token);
            var responseMessage = httpClient.GetAsync("user").Result;
            
            if(responseMessage.StatusCode == HttpStatusCode.OK)
            {
                //Todo: Add user object
                var activeUser = JsonConvert.DeserializeObject<UserDto>(responseMessage.Content.ReadAsStringAsync().Result);
                if (!string.IsNullOrWhiteSpace(Roles))
                {
                    bool canAccess = false;
                    if (Roles.Contains(",")) 
                    {
                        var acceptedRoles = Roles.Split(",");
                        foreach (var role in acceptedRoles)
                        {
                            if (activeUser.UserRole.ToLower().Equals(role.ToLower()))
                            {
                                canAccess = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (activeUser.UserRole.Equals(Roles))
                            canAccess = true;
                    }

                    if (!canAccess)
                    {
                        context.Result = new RedirectToActionResult("AccessDenied", "Account", null);
                    }
                }


            }
            else
            {
                context.Result = new RedirectToActionResult("Index", "Home", new {area="",statusCode = responseMessage.StatusCode.ToString() });
            }


        }
    }
}
