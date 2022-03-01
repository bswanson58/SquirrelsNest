using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.LiteDb.Database;

namespace SquirrelsNest.LiteDb.Providers {
    internal class ComponentDeleter : ComponentProvider {
        private readonly IIssueProvider mIssueProvider;

        public ComponentDeleter( IDatabaseProvider databaseProvider, IIssueProvider issueProvider )
            : base( databaseProvider ) {
            mIssueProvider = issueProvider;
        }

        private Either<Error, Unit> UpdateIssues( Seq<SnIssue> list ) {
            return list
                .Map( issue => mIssueProvider.UpdateIssue( issue ).Result )
                .Sequence()
                .Map( _ => Unit.Default );
        }

        public new Either<Error, Unit> DeleteComponent( SnComponent component ) {
            return mIssueProvider.GetIssues().Result
                .Map( list => from i in list where i.ComponentId.Equals( component.EntityId ) select i )
                .Map( list => from i in list select i.With( SnComponent.Default ))
                .Bind( list => UpdateIssues( list.ToSeq()))
                .Do( _ => DeleteComponent( component ));
        }
    }
}
