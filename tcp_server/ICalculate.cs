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

        [OperationContract]
        double Action(Inputs A); 
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