using System;
using Contract;
using Serilog;

namespace TCP_Server
{
    //[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    class CalculateService : ICalculate, ICalculate2
    {
        public double Add(double A, double B)
        {
            var C = A + B; 
            Log.Information("{0} {1} {2} = {3}", A, nameof(Add), B, C); 
            return C; 
        }

        public double Add2(double A, double B)
        {
            var C = A + B;
            Log.Information("Add2 ---------- {0} {1} {2} = {3}", A, nameof(Add), B, C);
            return C;
        }

        public double multiply(double A, double B)
        {
            var C = A * B;
            Log.Information("{0} {1} {2} = {3}", A, nameof(multiply), B, C);
            return C;
        }

        public double Substract(double A, double B)
        {
            var C = A - B;
            Log.Information("{0} {1} {2} = {3}", A, nameof(Substract), B, C);
            return C;
        }
    }
}
