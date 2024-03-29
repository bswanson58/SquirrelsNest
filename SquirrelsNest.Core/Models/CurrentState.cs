﻿using LanguageExt;
using SquirrelsNest.Common.Entities;

namespace SquirrelsNest.Core.Models {
    public class CurrentState {
        public  Option<SnProject>   Project { get; }
        public  Option<SnUser>      User { get; }

        public CurrentState( Option<SnProject> project, Option<SnUser> user ) {
            Project = project;
            User = user;
        }
    }
}
