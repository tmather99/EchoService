using System.Runtime.Serialization;
using CoreWCF;

namespace Contract
{
    [ServiceContract]
    public interface ICalculate
    {
        [OperationContract]
        double Add(double A, double B);

        [OperationContract]
        double Substract(double A, double B);

        [OperationContract]
        double multiply(double A, double B);
    }
}