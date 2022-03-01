namespace SquirrelsNest.Core.ProjectTemplates {
    public interface IProjectCreator {
        IEnumerable<IProjectTemplate>   GetAvailableTemplates();
        void                            CreateProject( IProjectTemplate fromTemplate );
    }
}
