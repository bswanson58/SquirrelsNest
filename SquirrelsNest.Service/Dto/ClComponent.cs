﻿using System;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Values;

namespace SquirrelsNest.Service.Dto {
    public class ClComponent : ClBase {
        public string ProjectId { get; }
        public string Name { get; }
        public string Description { get; }

        public ClComponent( string Id, string projectId, string name, string description ) :
        base( Id ) {
            ProjectId = projectId;
            Name = name;
            Description = description;
        }

        private static ClComponent ? mDefaultComponent;

        public static ClComponent Default =>
            mDefaultComponent ??= new ClComponent( EntityId.Default.Value, EntityId.Default.Value, "Unspecified", String.Empty );
    }

    public static class ClComponentEx {
        public static ClComponent ToCl( this SnComponent component ) {
            return new ClComponent( component.EntityId, component.ProjectId, component.Name, component.Description );
        }

        public static SnComponent ToEntity( this ClComponent component ) {
            return new SnComponent( EntityId.Default, String.Empty, component.ProjectId, component.Name, component.Description );
        }
    }
}
