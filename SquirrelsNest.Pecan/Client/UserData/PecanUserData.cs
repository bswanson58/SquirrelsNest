using System;
using System.Text.Json.Serialization;

namespace SquirrelsNest.Pecan.Client.UserData {
    public class PecanUserData {
        public string CurrentProjectId { get; set; }

        [JsonConstructor]
        public PecanUserData( string currentProjectId ) {
            CurrentProjectId = currentProjectId;
        }

        public PecanUserData() {
            CurrentProjectId = string.Empty;
        }
    }
}
