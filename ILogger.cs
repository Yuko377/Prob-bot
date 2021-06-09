
namespace kontur_project
{
    public interface ILogger
    {
        void StartLog();
        void WriteError(long identificator, string condition, string message);
    }
}
