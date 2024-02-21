namespace ConsulRegistHelper
{
    public class ConsulRegisterOption
    {
        public string Address { get; set; }
        public string ServiceHealthCheck { get; set; }
        //consul 的心跳检测和nacos有区别 nacos是主动给服务发心跳 而consul是服务主动给consul发心跳所以需要告诉consul自己的ip和端口
        public string ServiceName { get; set; }
        public string ServiceIP { get; set; }
        public int ServicePort { get; set; }
    }
}