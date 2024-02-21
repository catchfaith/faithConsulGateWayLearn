using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsulRegistHelper.ConsulExend
{
    /// <summary>
    /// 平均调度
    /// </summary>
    public class AverageDispatcher : AbstractConsulDispatcher
    {
        #region Identity
        private static int _iTotalCount = 0;
        private static int iTotalCount
        {
            get
            {
                return _iTotalCount;
            }
            set
            {
                _iTotalCount = value >= Int32.MaxValue ? 0 : value;
            }
        }

        public AverageDispatcher(IOptionsMonitor<ConsulRegisterOption> consulClientOption) : base(consulClientOption)
        {
        }
        #endregion
        /// <summary>
        /// 平均
        /// </summary>
        /// <returns></returns>
        protected override int GetIndex()
        {
            return new Random(iTotalCount++).Next(0, base._CurrentAgentServiceDictionary.Length);
        }
    }
}
