using AvitoMessage.Pages;

namespace AvitoMessage.Operations
{
    public interface IOperation
    {
        Page OperationPage { get; }
        bool Execute();
    }
}
