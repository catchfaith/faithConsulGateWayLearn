using Consul;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Polly;
using Polly.Timeout;
using Polly.Wrap;
using ServerB;
using Policy = Polly.Policy;

namespace ServerA.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HelloController : ControllerBase
    {
        private AsyncPolicyWrap<bool> asyncPolicy;
        public HelloController() {
            //超时策略
            //设置一个超时时间 一个请求超过一定的时间内没有响应就返回错误信息，这样就不会无休止的等待了。
            var timeout = Policy.TimeoutAsync(1, TimeoutStrategy.Pessimistic, (context, ts, task) => {
                Console.WriteLine("调用超时");
                return Task.CompletedTask;
            });
            //服务熔断
            //由断路器统计业务执行的异常比例 比如说有3个服务A 去调用1个服务B 返回了两个都是异常的，
            //异常率应该是66.6%，我给限定一个异常限定值50%，那么就会熔断这个服务，拦截访问这个服务的一切的请求 在1s内就能返回错误信息了避免了资源的浪费
            var circuitBreakerPolicy = Policy.Handle<Exception>().CircuitBreakerAsync(
                //连续5次异常，断开10s，之后开启半开状态
                exceptionsAllowedBeforeBreaking: 5,
                durationOfBreak: TimeSpan.FromMilliseconds(10),
                onBreak: (ex, breakDelay) =>
                {
                    //熔断以后处理的动作
                },
                onReset: () =>
                {
                    //熔断器关闭时
                },
                onHalfOpen: () =>
                {
                    //熔断时间结束=>断开状态进入半开状态
                }
                );

           asyncPolicy = Policy<bool>
                .Handle<Exception>()
                .FallbackAsync(serviceAFallback(), (x) =>
                {
                    return Task.CompletedTask;
                })
                .WrapAsync(circuitBreakerPolicy)
                .WrapAsync(timeout);
        }
        [HttpGet]
        public async Task<bool> test() {

            Console.WriteLine("hhhhhhhhhhh");
            return await asyncPolicy.ExecuteAsync(() =>
            {
                //dosome
                return Task.FromResult(true);
            });
        }
        private bool serviceAFallback() {
            Console.WriteLine("调用了降级方法");
            return false;
        }
    }
}
