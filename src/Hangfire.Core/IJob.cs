using Hangfire.Filters;

namespace Hangfire
{
    public interface IJob
    {
        void Run();
    }

    public interface IJob<out T>
    {
        T Run();
    }
}