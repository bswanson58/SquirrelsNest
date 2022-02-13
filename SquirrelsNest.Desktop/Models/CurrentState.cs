using LanguageExt;
using SquirrelsNest.Common.Entities;

namespace SquirrelsNest.Desktop.Models {
    internal class CurrentState {
        public  Option<SnProject>   Project { get; }

        public CurrentState( Option<SnProject> project ) {
            Project = project;
        }
    }
}
