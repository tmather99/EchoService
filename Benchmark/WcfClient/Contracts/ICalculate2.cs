using System.ServiceModel;
using System.Threading.Tasks;

namespace Contracts
{
    [ServiceContract]
    public interface ICalculate2
    {
        [OperationContract]
        Task<int> Add2(int A, int B);
    }
}