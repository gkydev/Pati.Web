﻿using Pati.Web.Dtos;
using Pati.Web.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pati.Web.ApiServices.Interfaces
{
    public interface IUserApiService
    {
        Task<IResult> RegisterAsync(UserDto userDto);
    }
}
