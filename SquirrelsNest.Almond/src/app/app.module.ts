import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import {RouterModule, Routes} from '@angular/router'

import { AppComponent } from './app.component';
import { HeaderComponent } from './header/header.component';
import { IssuesPageComponent } from './Issues/issues-page/issues-page.component';
import { IssueListComponent } from './Issues/issue-list/issue-list.component';
import {MaterialModule} from './material.module'
import { ProjectsPageComponent } from './Projects/projects-page/projects-page.component';
import { ProjectListComponent } from './Projects/project-list/project-list.component';
import { UsersPageComponent } from './Users/users-page/users-page.component';
import { ContainerComponent } from './container/container.component';
import { GraphQLModule } from './graphql.module';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

const appRoutes : Routes = [
  { path: 'projects', component: ProjectsPageComponent },
  { path: 'issues', component: IssuesPageComponent },
  { path: 'users', component: UsersPageComponent },
]

@NgModule({
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
    BrowserModule,
    HttpClientModule,
    RouterModule.forRoot( appRoutes ),
    BrowserAnimationsModule,
    GraphQLModule,
    MaterialModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
