using Consul;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ConsulRegistHelper
{
    public abstract class AbstractConsulDispatcher
    {
        protected ConsulRegisterOption _consulRegisterOptions;
        protected KeyValuePair<string, AgentService>[] _CurrentAgentServiceDictionary;
        public AbstractConsulDispatcher(IOptionsMonitor<ConsulRegisterOption> consulClientOption) {
            _consulRegisterOptions = consulClientOption.CurrentValue;
        }
        /// <summary>
        /// 负载均衡获取地址
        /// </summary>
        /// <param name="mappingUrl">映射后的地址例如http://SeriveB/test</param>
        /// <returns></returns>
        public string GetAddress(string mappingUrl) { 
           Uri uri = new Uri(mappingUrl);
           var serviceName = uri.Host;
           string addressPort = this.chooseAddress(serviceName);
           string scheme = uri.Scheme;
           string path = uri.PathAndQuery;
           return $"{uri.Scheme}://{addressPort}{uri.PathAndQuery}";
        }
        protected virtual string chooseAddress(string serviceName) {
            ConsulClient client = new ConsulClient(c =>
            {
                c.Address = new Uri($"http://localhost:8500");
            });
            AgentService agentService = null;

            //获取consul 所有服务清单
           var response = client.Agent.Services().Result.Response;
           var dictionary = response.Where(s => s.Value.Service.Equals(serviceName, StringComparison.OrdinalIgnoreCase)).ToArray();
            var entrys = client.Health.Service(serviceName).Result.Response;
            List<KeyValuePair<string, AgentService>> serviceList = new List<KeyValuePair<string, AgentService>>();
            for (int i = 0; i < entrys.Length; i++)
            {
                serviceList.Add(new KeyValuePair<string, AgentService>(i.ToString(), entrys[i].Service));
            } 
            this._CurrentAgentServiceDictionary = serviceList.ToArray();
            int index = this.GetIndex();
            agentService = this._CurrentAgentServiceDictionary[index].Value;

            return $"{agentService.Address}:{agentService.Port}";
        }
        protected abstract int GetIndex();
    }
}
