using LanguageExt;
using SquirrelsNest.Common.Entities;

namespace SquirrelsNest.Desktop.Models {
    internal class CurrentState {
        public  Option<SnProject>   Project { get; }
        public  SnUser              User { get; }

        public CurrentState( Option<SnProject> project, SnUser user ) {
            Project = project;
            User = user;
        }
    }
}
