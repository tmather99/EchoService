using System.ServiceModel;
using System.Threading.Tasks;

namespace Contracts
{
    [ServiceContract]
    public interface ICalculate2
    {
        [OperationContract]
        Task<double> Add2(double A, double B);
    }
}