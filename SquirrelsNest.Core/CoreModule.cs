﻿using Autofac;
using FluentValidation;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Core.CompositeBuilders;
using SquirrelsNest.Core.Environment;
using SquirrelsNest.Core.Interfaces;
using SquirrelsNest.Core.Platform;
using SquirrelsNest.Core.Validators;

namespace SquirrelsNest.Core {
    public class CoreModule : Module {
        protected override void Load( ContainerBuilder builder ) {
            builder.RegisterType<ApplicationConstants>().As<IApplicationConstants>();
            builder.RegisterType<ApplicationEnvironment>().As<IEnvironment>();

            builder.RegisterType<FileWriter>().As<IFileWriter>().SingleInstance();

            builder.RegisterType<IssueBuilder>().As<IIssueBuilder>().SingleInstance();
            builder.RegisterType<ProjectBuilder>().As<IProjectBuilder>().SingleInstance();

            builder.RegisterType<CompositeProjectValidator>().As<IValidator<CompositeProject>>();
        }
    }
}
