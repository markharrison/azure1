using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Azure1
{
    public class AppConfig
    {
        private string _AdminPWVal;
        private string _GoogleIdVal;
        public AppConfig(IConfiguration _config)
        {
            _AdminPWVal = _config.GetValue<string>("AdminPW");
            _GoogleIdVal = _config.GetValue<string>("GoogleId");
        }
        public string AdminPW
        {
            get => this._AdminPWVal;
            set => this._AdminPWVal = value;
        }
        public string GoogleId
        {
            get => this._GoogleIdVal;
            set => this._GoogleIdVal = value;
        }
    }
}