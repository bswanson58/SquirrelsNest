import {NgModule} from '@angular/core'
import {RouterModule, Routes} from '@angular/router'
import {AuthGuard} from './Auth/auth.guard'
import {LoginComponent} from './Auth/login/login.component'
import {RegisterComponent} from './Auth/register/register.component'

import {IssuesPageComponent} from './Issues/issues-page/issues-page.component'
import {ProjectsPageComponent} from './Projects/projects-page/projects-page.component'
import {UsersPageComponent} from './Users/users-page/users-page.component'

const appRoutes: Routes = [
  { path: 'projects', component: ProjectsPageComponent, canActivate: [AuthGuard] },
  { path: 'issues', component: IssuesPageComponent, canActivate: [AuthGuard] },
  { path: 'users', component: UsersPageComponent, canActivate: [AuthGuard] },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent }
]

@NgModule( {
  imports: [
    RouterModule.forRoot( appRoutes ),
  ],
  exports: [RouterModule]
} )
export class AppRoutingModule {
}
