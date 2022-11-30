using System.Threading.Tasks;
using CoreWCF;

namespace Contract
{
    [ServiceContract]
    public interface ICalculate2
    {
        [OperationContract]
        Task<int> Add2(int A, int B);
    }
}