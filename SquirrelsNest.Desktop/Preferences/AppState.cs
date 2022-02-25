using SquirrelsNest.Common.Values;

namespace SquirrelsNest.Desktop.Preferences {
    internal class AppState {
        public  string        UserId { get; set; }
        public  string        ProjectId { get; set; }

        public AppState() {
            UserId = EntityId.Default;
            ProjectId = EntityId.Default;
        }

        public AppState With( EntityId ? userId = null, EntityId ? projectId = null ) {
            return new AppState {
                UserId = userId ?? UserId,
                ProjectId = projectId ?? ProjectId
            };
        }
    }
}
