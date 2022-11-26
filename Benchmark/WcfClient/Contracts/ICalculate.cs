using System.ServiceModel;
using System.Threading.Tasks;

namespace Contracts
{
    [ServiceContract]
    public interface ICalculate
    {
        [OperationContract]
        Task<double> Add(double A, double B);

        [OperationContract]
        Task<double> Substract(double A, double B);

        [OperationContract]
        Task<double> multiply(double A, double B);
    }
}