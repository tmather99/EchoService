using System.ServiceModel;
using System.Threading.Tasks;

namespace Contracts
{
    [ServiceContract]
    public interface ICalculate
    {
        [OperationContract]
        Task<int> Add(int A, int B);

        [OperationContract]
        Task<int> Substract(int A, int B);

        [OperationContract]
        Task<int> multiply(int A, int B);
    }
}