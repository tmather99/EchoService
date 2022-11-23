using CoreWCF;

namespace Contract
{
    [ServiceContract]
    public interface ICalculate2
    {
        [OperationContract]
        double Add2(double A, double B);
    }
}