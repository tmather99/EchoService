using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Bechmark.Contracts
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

        [OperationContract]
        Task<double> Action(Inputs A);
    }

    [DataContract]
    public class Inputs
    {
        [DataMember]
        public double A { get; set; }


        [DataMember]
        public double B { get; set; }

        public enum OperationEnum
        {
            Addition,
            Substraction,
            Multiplication
        }

        [DataMember]
        public OperationEnum Operation { get; set; }
    }
}