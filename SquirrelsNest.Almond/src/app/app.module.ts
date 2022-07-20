import {NgModule} from '@angular/core'
import {BrowserAnimationsModule} from '@angular/platform-browser/animations'
import {BrowserModule} from '@angular/platform-browser'
import {GraphQLModule} from './graphql.module'
import {HttpClientModule} from '@angular/common/http'

import {AppComponent} from './app.component'
import {AppRoutingModule} from './app.routing.module'
import {ContainerComponent} from './container/container.component'
import {HeaderComponent} from './header/header.component'
import {IssuesPageComponent} from './Issues/issues-page/issues-page.component'
import {IssueListComponent} from './Issues/issue-list/issue-list.component'
import {MaterialModule} from './material.module'
import {ProjectsPageComponent} from './Projects/projects-page/projects-page.component'
import {ProjectListComponent} from './Projects/project-list/project-list.component'
import {UsersPageComponent} from './Users/users-page/users-page.component'


@NgModule( {
  declarations: [
    AppComponent,
    HeaderComponent,
    IssuesPageComponent,
    IssueListComponent,
    ProjectsPageComponent,
    ProjectListComponent,
    UsersPageComponent,
    ContainerComponent
  ],
  imports: [
    AppRoutingModule,
    BrowserAnimationsModule,
    BrowserModule,
    HttpClientModule,
    GraphQLModule,
    MaterialModule
  ],
  providers: [],
  bootstrap: [AppComponent]
} )
export class AppModule {
}
