using System.Runtime.Serialization;
using System.Threading.Tasks;
using CoreWCF;

namespace Contract
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