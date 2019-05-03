using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SmartMeal.Data.Repository.Interfaces;
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

        public FacebookService(IConfiguration config, IRepository<User> userRepository)
        {
            _userRepository = userRepository;
            _client = new HttpClient();
            _config = config;

        }

        public async Task<FacebookUserData> Authentication(FacebookAuthDto model)
        {
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

            return userInfo;
        }

        public async Task<User> Register(FacebookUserData data)
        {
            var userExist = await _userRepository.AnyExist(x => x.FacebookId == data.Id);

            var newUser = new User()
            {
                Email = data.Email,
                FacebookId = data.Id
            };

            var is_created = await _userRepository.CreateAsync(newUser);

            if (!is_created)
            {
                return null;
            }

            return newUser;
        }

        public async Task<User> GetUser(FacebookUserData model)
        {
            var user = await _userRepository.GetByAsync(x => x.Email == model.Email && x.FacebookId == model.Id);
            
            return user;
        }
    }
}
