import {NgModule} from '@angular/core'
import {FormsModule} from '@angular/forms'
import {MatIconModule} from '@angular/material/icon'
import {BrowserAnimationsModule} from '@angular/platform-browser/animations'
import {BrowserModule} from '@angular/platform-browser'
import {FlexLayoutModule} from '@angular/flex-layout'
import {GraphQLModule} from './graphql.module'
import {HttpClientModule} from '@angular/common/http'

import {AppComponent} from './app.component'
import {AppRoutingModule} from './app.routing.module'
import {ContainerComponent} from './container/container.component'
import {HeaderComponent} from './header/header.component'
import {IssuesPageComponent} from './Issues/issues-page/issues-page.component'
import {IssueListComponent} from './Issues/issue-list/issue-list.component'
import {LoginComponent} from './Auth/login/login.component'
import {MaterialModule} from './material.module'
import {ProjectsPageComponent} from './Projects/projects-page/projects-page.component'
import {ProjectListComponent} from './Projects/project-list/project-list.component'
import {RegisterComponent} from './Auth/register/register.component'
import {UsersPageComponent} from './Users/users-page/users-page.component'


@NgModule( {
  declarations: [
    AppComponent,
    ContainerComponent,
    HeaderComponent,
    IssuesPageComponent,
    IssueListComponent,
    LoginComponent,
    ProjectsPageComponent,
    ProjectListComponent,
    RegisterComponent,
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
    MatIconModule
  ],
  providers: [],
  bootstrap: [AppComponent]
} )
export class AppModule {
}
