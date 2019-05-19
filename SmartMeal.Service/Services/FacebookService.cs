using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SmartMeal.Data.Repository.Interfaces;
using SmartMeal.Models;
using SmartMeal.Models.BindingModels;
using SmartMeal.Models.Models;
using SmartMeal.Models.ModelsDto;
using SmartMeal.Service.Interfaces;

namespace SmartMeal.Service.Services
{
    public class FacebookService : IFacebookService
    {

        private readonly HttpClient _client;
        private readonly IConfiguration _config;
        private readonly IRepository<User> _userRepository;
        private readonly IAuthService _authService;

        public FacebookService(IConfiguration config, IRepository<User> userRepository, IAuthService authService)
        {
            _authService = authService;
            _userRepository = userRepository;
            _client = new HttpClient();
            _config = config;

        }

        public async Task<Response<TokenDto>> Authenticate(FacebookAuthBindingModel model)
        {
            var response = new Response<TokenDto>();

            string FacebookAppId = _config["FacebookAuthSettings:AppId"];
            string FacebookAppSecret = _config["FacebookAuthSettings:AppSecret"];
            var appAccessTokenResponse = await _client.GetStringAsync($"https://graph.facebook.com/oauth/access_token?client_id={FacebookAppId}&client_secret={FacebookAppSecret}&grant_type=client_credentials");
            var appAccessToken = JsonConvert.DeserializeObject<FacebookAppAccessToken>(appAccessTokenResponse);

            var userAccessTokenValidationResponse = await _client.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={model.AccessToken}&access_token={appAccessToken.AccessToken}");
            var userAccessTokenValidation = JsonConvert.DeserializeObject<FacebookUserAccessTokenValidation>(userAccessTokenValidationResponse);

            if (!userAccessTokenValidation.Data.IsValid)
            {
                return null;
            }

            var userInfoResponse = await _client.GetStringAsync($"https://graph.facebook.com/v2.8/me?fields=id,email&access_token={model.AccessToken}");
            var userInfo = JsonConvert.DeserializeObject<FacebookUserData>(userInfoResponse);

            var user = await _userRepository.GetByAsync(x => x.FacebookId == userInfo.FacebookId && x.Email == userInfo.Email, withTracking:true);
            if (user == null)
            {
                user = await _userRepository.GetByAsync(x => x.Email == userInfo.Email, withTracking:true);
                if (user == null)
                {
                    user = Mapper.Map<User>(userInfo);
                    var IsCreated = await _userRepository.CreateAsync(user);
                    if (!IsCreated)
                    {
                        response.AddError(Error.AuthenticationError);
                        return response;
                    }
                }
                else
                {
                    user.FacebookId = userInfo.FacebookId;
                    var IsUpdated = await _userRepository.UpdateAsync(user);
                    if (!IsUpdated)
                    {
                        response.AddError(Error.AuthenticationError);
                        return response;
                    }
                }
            }
            var userBinding = Mapper.Map<UserAuthBindingModel>(user);
            var result = await _authService.CreateToken(userBinding);
            if (result.IsError)
            {
                response.AddError(Error.AuthenticationError);
                return response;
            }

            response.Data = result.Data;

            return response;
        }

        
    }
}
