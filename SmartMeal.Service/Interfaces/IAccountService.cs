﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SmartMeal.Models.ModelsDto;

namespace SmartMeal.Service.Interfaces
{
    public interface IAccountService
    {
        Task<bool> CreateUserAsync(RegisterDto user);
    }
}
