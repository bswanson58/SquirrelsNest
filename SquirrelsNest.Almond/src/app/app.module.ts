import {NgModule} from '@angular/core'
import {FormsModule} from '@angular/forms'
import {MatIconModule} from '@angular/material/icon'
import {BrowserAnimationsModule} from '@angular/platform-browser/animations'
import {BrowserModule} from '@angular/platform-browser'
import {FlexLayoutModule} from '@angular/flex-layout'
import {StoreModule} from '@ngrx/store'
import {AuthGuard} from './Auth/auth.guard'
import {AuthService} from './Auth/auth.service'
import {GraphQLModule} from './graphql.module'
import {HttpClientModule} from '@angular/common/http'

import {AppComponent} from './app.component'
import {AppRoutingModule} from './app.routing.module'
import {IssueService} from './Issues/issues.service'
import {HeaderComponent} from './Navigation/header/header.component'
import {IssuesPageComponent} from './Issues/issues-page/issues-page.component'
import {IssueDetailComponent} from './Issues/issue-detail/issue-detail.component'
import {IssueHeaderComponent} from './Issues/issue-header/issue-header.component'
import {IssueListComponent} from './Issues/issue-list/issue-list.component'
import {LoginComponent} from './Auth/login/login.component'
import {MaterialModule} from './material.module'
import {ProjectsPageComponent} from './Projects/projects-page/projects-page.component'
import {ProjectListComponent} from './Projects/project-list/project-list.component'
import {RegisterComponent} from './Auth/register/register.component'
import {SidenavComponent} from './Navigation/sidenav/sidenav.component'
import {ProjectService} from './Projects/projects.service'
import {appReducers} from './Store/app.reducer'
import {UsersPageComponent} from './Users/users-page/users-page.component'
import {IssueDetailSelectorComponent} from './Issues/issue-detail-selector/issue-detail-selector.component'
import {IssueEditDialogComponent} from './Issues/issue-edit-dialog/issue-edit-dialog.component'


@NgModule( {
  declarations: [
    AppComponent,
    HeaderComponent,
    IssuesPageComponent,
    IssueHeaderComponent,
    IssueListComponent,
    IssueDetailComponent,
    IssueDetailSelectorComponent,
    IssueEditDialogComponent,
    LoginComponent,
    ProjectsPageComponent,
    ProjectListComponent,
    RegisterComponent,
    SidenavComponent,
    UsersPageComponent,
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
    AuthGuard,
    AuthService,
    IssueService,
    ProjectService
  ],
  bootstrap: [AppComponent]
} )
export class AppModule {
}
