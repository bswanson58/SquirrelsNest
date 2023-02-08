using System.Collections.Generic;
using SquirrelsNest.Pecan.Client.Store;
using SquirrelsNest.Pecan.Shared.Dto;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Client.Users.Actions {
    public class GetUsersAction {
        public  PageRequest PageRequest { get; }

        public GetUsersAction() {
            PageRequest = new PageRequest( 1, 25 );
        }
    }

    public class GetUsersSuccessAction {
        public  IEnumerable<SnUser>     Users { get; }
        public  PageInformation         PageInformation { get; }

        public GetUsersSuccessAction( IEnumerable<SnUser> userList, PageInformation pageInformation ) {
            Users = userList;
            PageInformation = pageInformation;
        }
    }

    public class GetUsersFailureAction : FailureAction {
        public  GetUsersFailureAction( string message ) :
            base( message ) {
        }
    }
}