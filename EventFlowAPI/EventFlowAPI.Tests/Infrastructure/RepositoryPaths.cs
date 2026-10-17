namespace EventFlowAPI.Tests.Infrastructure
{
    public class RepositoryPaths
    {
        public string SolutionDirectory { get; }
        public string ApiProjectDirectory { get; }

        public RepositoryPaths()
        {
            var directory = new DirectoryInfo(AppContext.BaseDirectory);
            while (directory != null && !File.Exists(Path.Combine(directory.FullName, "EventFlowAPI.slnx")))
            {
                directory = directory.Parent;
            }

            if (directory == null)
            {
                throw new InvalidOperationException("Cannot find EventFlowAPI.slnx above the test output directory.");
            }

            SolutionDirectory = directory.FullName;
            ApiProjectDirectory = Path.Combine(SolutionDirectory, "EventFlowAPI");
        }
    }
}
