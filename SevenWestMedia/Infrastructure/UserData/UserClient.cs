using Flurl.Http;
using Flurl.Http.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Domain;
using System.Collections.Generic;
using Common.ConfigOptions;
using System;
using Newtonsoft.Json.Linq;
using System.Net.Http.Json;
using Newtonsoft.Json.Schema;
using Common.Helpers;
using Newtonsoft.Json.Schema.Generation;
using System.Security.Principal;
using Newtonsoft.Json;

namespace Infrastructure.UserData
{
    public class UserClient : IUserClient
    {
        private readonly IFlurlClient _flurlClient;
        private readonly ILogger<UserClient> _logger;
        private readonly IOptions<EndPointConfig> _endPointConfig;

        public UserClient(IFlurlClientFactory flurlClientFactory,
                            ILogger<UserClient> logger,
                            IOptions<EndPointConfig> endPointConfig) 
        {
            _logger = logger;
            _endPointConfig = endPointConfig;
            _flurlClient = flurlClientFactory.Get(_endPointConfig.Value.UserApiEndPoint);
        }
        public async Task<IEnumerable<User>> GetDataAsync()
        {
            try
            {
                IFlurlResponse flurlResponse = await _flurlClient.Request().AllowAnyHttpStatus().GetAsync();

                string responseBody = await flurlResponse.ResponseMessage.Content.ReadAsStringAsync();
             
                bool validationResult = JsonHelper.Validate<User[]>(responseBody);

                if (validationResult)
                {
                    IEnumerable<User> userStream = await flurlResponse.GetJsonAsync<IEnumerable<User>>();

                    return userStream;
                }
                else
                {

                    _logger.LogError($"Invalid Json");

                    return Enumerable.Empty<User>();
                }

            }

            catch (JSchemaReaderException ex)
            {
                _logger.LogError($"{ex.Message}");

                return Enumerable.Empty<User>();
            }
            catch (JsonReaderException ex)
            {
                _logger.LogError($"{ex.Message}");

                return Enumerable.Empty<User>();
            }
            catch (FlurlHttpException ex)
            {


                _logger.LogError($"{ex.Message}");
                return Enumerable.Empty<User>();

            }
            catch (Exception ex)
            {

                _logger.LogError($"{ex.Message}");

                throw;
            }

        }
    }
}
