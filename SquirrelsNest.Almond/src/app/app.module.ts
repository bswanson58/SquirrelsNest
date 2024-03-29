import {NgModule} from '@angular/core'
import {FormsModule} from '@angular/forms'
import {MatIconModule} from '@angular/material/icon'
import {BrowserAnimationsModule} from '@angular/platform-browser/animations'
import {BrowserModule, Title} from '@angular/platform-browser'
import {FlexLayoutModule} from '@angular/flex-layout'
import {StoreModule} from '@ngrx/store'
import {StoreDevtoolsModule} from '@ngrx/store-devtools'
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
import {ProjectTransferService} from './Projects/project.transfer.service'
import {ProjectsPageComponent} from './Projects/projects-page/projects-page.component'
import {ProjectListComponent} from './Projects/project-list/project-list.component'
import {RegisterComponent} from './Auth/register/register.component'
import {SidenavComponent} from './Navigation/sidenav/sidenav.component'
import {ProjectService} from './Projects/projects.service'
import {appReducers} from './Store/app.reducer'
import {MessageReporter} from './UI/message.reporter'
import {VariableDirective} from './UI/ng-var.directive'
import {UserDataFacade} from './UserData/user.data.facade'
import {UserDataManager} from './UserData/user.data.manager'
import {UserDataService} from './UserData/user.data.service'
import {UsersFacade} from './Users/user.facade'
import {UserMutationsService} from './Users/user.mutations.service'
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
import {UserHeaderComponent} from './Users/user-header/user-header.component'
import {UserEditDialogComponent} from './Users/user-edit-dialog/user-edit-dialog.component'
import {UserCreateDialogComponent} from './Users/user-create-dialog/user-create-dialog.component'
import {UserEditRolesDialogComponent} from './Users/user-edit-roles-dialog/user-edit-roles-dialog.component'
import {UserEditPasswordDialogComponent} from './Users/user-edit-password-dialog/user-edit-password-dialog.component'
import {MessageDialogComponent} from './UI/message-dialog/message-dialog.component'
import {ProjectImportDialogComponent} from './Projects/project-import-dialog/project-import-dialog.component'
import {ProjectCreateDialogComponent} from './Projects/project-create-dialog/project-create-dialog.component'
import {CreateTemplateDialogComponent} from './Projects/create-template-dialog/create-template-dialog.component'
import {HomePageComponent} from './UI/home-page/home-page.component'
import {ErrorPanelComponent} from './UI/error-panel/error-panel.component'
import {ServiceActivityPanelComponent} from './UI/service-activity-panel/service-activity-panel.component'


@NgModule( {
  declarations: [
    AppComponent,
    ComponentEditDialogComponent,
    ComponentItemComponent,
    ComponentsListComponent,
    ConfirmDialogComponent,
    CreateTemplateDialogComponent,
    ErrorPanelComponent,
    HeaderComponent,
    HomePageComponent,
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
    MessageDialogComponent,
    ProjectsPageComponent,
    ProjectCommandsComponent,
    ProjectCreateDialogComponent,
    ProjectEditDialogComponent,
    ProjectDetailComponent,
    ProjectImportDialogComponent,
    ProjectListComponent,
    ProjectUsersListComponent,
    RegisterComponent,
    ServiceActivityPanelComponent,
    SidenavComponent,
    UsersPageComponent,
    WorkflowEditDialogComponent,
    WorkflowListComponent,
    WorkflowItemComponent,
    UserCreateDialogComponent,
    UserDetailComponent,
    UserEditDialogComponent,
    UserEditPasswordDialogComponent,
    UserEditRolesDialogComponent,
    UserHeaderComponent,
    UserListComponent,
    VariableDirective,
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
    StoreModule.forRoot( appReducers ),
    StoreDevtoolsModule.instrument()
  ],
  providers: [
    AuthFacade,
    AuthGuard,
    AuthService,
    IssuesFacade,
    IssueService,
    MessageReporter,
    ProjectConstants,
    ProjectFacade,
    ProjectDetailsService,
    ProjectService,
    ProjectTransferService,
    Title,
    UsersFacade,
    UserDataFacade,
    UserDataManager,
    UserDataService,
    UserService,
    UserMutationsService,
  ],
  bootstrap: [AppComponent]
} )
export class AppModule {
}
