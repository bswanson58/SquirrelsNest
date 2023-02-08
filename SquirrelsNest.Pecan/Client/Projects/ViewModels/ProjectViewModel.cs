using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Client.Projects.ViewModels {
    internal class ProjectViewModel {
        private bool                mIsMouseOver;
        public  SnCompositeProject  Project { get; }

        public string Name => Project.Name;
        public string Description => Project.Description;

        public ProjectViewModel( SnCompositeProject project ) {
            Project = project;
            mIsMouseOver = false;
        }

        public void OnMouseEnter() => mIsMouseOver = true;
        public void OnMouseLeave() => mIsMouseOver = false;

        public string MouseOverHighlight => mIsMouseOver ? "mouse-highlight" : "";
    }
}
