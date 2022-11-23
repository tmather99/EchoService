using System.ServiceModel;
using System.Threading.Tasks;

namespace Bechmark.Contracts
{
    [ServiceContract]
    public interface ICalculate2
    {
        [OperationContract]
        Task<double> Add2(double A, double B);
    }
}