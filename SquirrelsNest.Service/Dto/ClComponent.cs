﻿using System;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Values;

namespace SquirrelsNest.Service.Dto {
    public class ClComponent : ClBase {
        public string ProjectId { get; }
        public string Name { get; }
        public string Description { get; }

        public ClComponent( string entityId, string projectId, string name, string description ) :
        base( entityId ) {
            ProjectId = projectId;
            Name = name;
            Description = description;
        }

        private static ClComponent ? mDefaultComponent;

        public static ClComponent Default =>
            mDefaultComponent ??= new ClComponent( EntityId.Default.Value, EntityId.Default.Value, "Unspecified", String.Empty );
    }

    public static class ComponentExtensions {
        public static ClComponent From( this SnComponent component ) {
            return new ClComponent( component.EntityId, component.ProjectId, component.Name, component.Description );
        }
    }
}
