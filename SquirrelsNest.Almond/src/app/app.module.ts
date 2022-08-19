import {NgModule} from '@angular/core'
import {FormsModule} from '@angular/forms'
import {MatIconModule} from '@angular/material/icon'
import {BrowserAnimationsModule} from '@angular/platform-browser/animations'
import {BrowserModule} from '@angular/platform-browser'
import {FlexLayoutModule} from '@angular/flex-layout'
import {StoreModule} from '@ngrx/store'
import {AuthFacade} from './Auth/auth.facade'
import {AuthGuard} from './Auth/auth.guard'
import {AuthService} from './Auth/auth.service'
import {GraphQLModule} from './graphql.module'
import {HttpClientModule} from '@angular/common/http'

import {AppComponent} from './app.component'
import {AppRoutingModule} from './app.routing.module'
import {IssuesFacade} from './Issues/issues.facade'
import {IssueService} from './Issues/issues.service'
import {HeaderComponent} from './Navigation/header/header.component'
import {IssuesPageComponent} from './Issues/issues-page/issues-page.component'
import {IssueDetailComponent} from './Issues/issue-detail/issue-detail.component'
import {IssueHeaderComponent} from './Issues/issue-header/issue-header.component'
import {IssueListComponent} from './Issues/issue-list/issue-list.component'
import {LoginComponent} from './Auth/login/login.component'
import {MaterialModule} from './material.module'
import {ProjectConstants} from './Projects/project.const'
import {ProjectDetailsService} from './Projects/project.details.service'
import {ProjectFacade} from './Projects/project.facade'
import {ProjectsPageComponent} from './Projects/projects-page/projects-page.component'
import {ProjectListComponent} from './Projects/project-list/project-list.component'
import {RegisterComponent} from './Auth/register/register.component'
import {SidenavComponent} from './Navigation/sidenav/sidenav.component'
import {ProjectService} from './Projects/projects.service'
import {appReducers} from './Store/app.reducer'
import {UsersFacade} from './Users/user.facade'
import {UserService} from './Users/user.service'
import {UsersPageComponent} from './Users/users-page/users-page.component'
import {IssueDetailSelectorComponent} from './Issues/issue-detail-selector/issue-detail-selector.component'
import {IssueEditDialogComponent} from './Issues/issue-edit-dialog/issue-edit-dialog.component'
import {IssueFooterComponent} from './Issues/issue-footer/issue-footer.component'
import {ConfirmDialogComponent} from './UI/confirm-dialog/confirm-dialog.component'
import {ProjectDetailComponent} from './Projects/project-detail/project-detail.component'
import {ComponentsListComponent} from './Projects/components-list/components-list.component'
import {IssueTypesListComponent} from './Projects/issue-types-list/issue-types-list.component'
import {WorkflowListComponent} from './Projects/workflow-list/workflow-list.component'
import {ProjectUsersListComponent} from './Projects/project-users-list/project-users-list.component'
import {ProjectCommandsComponent} from './Projects/project-commands/project-commands.component'
import {ProjectEditDialogComponent} from './Projects/project-edit-dialog/project-edit-dialog.component'
import {ComponentEditDialogComponent} from './Projects/component-edit-dialog/component-edit-dialog.component'
import {ComponentItemComponent} from './Projects/components-list/component-item/component-item.component'
import {IssueTypeEditDialogComponent} from './Projects/issue-type-edit-dialog/issue-type-edit-dialog.component'
import {WorkflowEditDialogComponent} from './Projects/workflow-edit-dialog/workflow-edit-dialog.component'
import {IssueTypeItemComponent} from './Projects/issue-types-list/issue-type-item/issue-type-item.component'
import {WorkflowItemComponent} from './Projects/workflow-list/workflow-item/workflow-item.component'
import {UserListComponent} from './Users/user-list/user-list.component'
import {UserDetailComponent} from './Users/user-detail/user-detail.component'


@NgModule( {
  declarations: [
    AppComponent,
    ComponentEditDialogComponent,
    ComponentItemComponent,
    ComponentsListComponent,
    ConfirmDialogComponent,
    HeaderComponent,
    IssuesPageComponent,
    IssueFooterComponent,
    IssueHeaderComponent,
    IssueListComponent,
    IssueDetailComponent,
    IssueDetailSelectorComponent,
    IssueEditDialogComponent,
    IssueTypeEditDialogComponent,
    IssueTypeItemComponent,
    IssueTypesListComponent,
    LoginComponent,
    ProjectsPageComponent,
    ProjectCommandsComponent,
    ProjectEditDialogComponent,
    ProjectDetailComponent,
    ProjectListComponent,
    ProjectUsersListComponent,
    RegisterComponent,
    SidenavComponent,
    UsersPageComponent,
    WorkflowEditDialogComponent,
    WorkflowListComponent,
    WorkflowItemComponent,
    UserListComponent,
    UserDetailComponent,
  ],
  imports: [
    AppRoutingModule,
    BrowserAnimationsModule,
    BrowserModule,
    FlexLayoutModule,
    FormsModule,
    HttpClientModule,
    GraphQLModule,
    MaterialModule,
    MatIconModule,
    StoreModule.forRoot( appReducers )
  ],
  providers: [
    AuthFacade,
    AuthGuard,
    AuthService,
    IssuesFacade,
    IssueService,
    ProjectConstants,
    ProjectFacade,
    ProjectService,
    ProjectDetailsService,
    UsersFacade,
    UserService,
  ],
  bootstrap: [AppComponent]
} )
export class AppModule {
}
