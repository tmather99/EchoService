using System;
using System.Threading.Tasks;
using Contract;
using Serilog;

namespace TCP_Server
{
    //[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    class CalculateService : ICalculate, ICalculate2
    {
        public Task<int> Add(int A, int B)
        {
            var C = A + B; 
            Log.Information("{0} {1} {2} = {3}", A, nameof(Add), B, C); 
            return Task.FromResult(C); 
        }

        public Task<int> Add2(int A, int B)
        {
            var C = A + B;
            Log.Information("Add2 ---------- {0} {1} {2} = {3}", A, nameof(Add), B, C);
            return Task.FromResult(C);
        }

        public Task<int> multiply(int A, int B)
        {
            var C = A * B;
            Log.Information("{0} {1} {2} = {3}", A, nameof(multiply), B, C);
            return Task.FromResult(C);
        }

        public Task<int> Substract(int A, int B)
        {
            var C = A - B;
            Log.Information("{0} {1} {2} = {3}", A, nameof(Substract), B, C);
            return Task.FromResult(C);
        }
    }
}
