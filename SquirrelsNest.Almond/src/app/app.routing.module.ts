import {NgModule} from '@angular/core'
import {RouterModule, Routes} from '@angular/router'
import {AuthGuard} from './Auth/auth.guard'
import {LoginComponent} from './Auth/login/login.component'
import {RegisterComponent} from './Auth/register/register.component'

import {IssuesPageComponent} from './Issues/issues-page/issues-page.component'
import {ProjectsPageComponent} from './Projects/projects-page/projects-page.component'
import {HomePageComponent} from './UI/home-page/home-page.component'
import {UsersPageComponent} from './Users/users-page/users-page.component'

const appRoutes: Routes = [
  { path: '', component: HomePageComponent, title: 'SquirrelsNest' },
  { path: 'home', component: HomePageComponent, title: 'SquirrelsNest' },
  { path: 'projects', component: ProjectsPageComponent, canActivate: [AuthGuard], title: 'Projects' },
  { path: 'issues', component: IssuesPageComponent, canActivate: [AuthGuard], title: 'Issues' },
  { path: 'users', component: UsersPageComponent, canActivate: [AuthGuard], title: 'Users' },
  { path: 'login', component: LoginComponent, title: 'Login' },
  { path: 'register', component: RegisterComponent, title: 'Register' },
]

@NgModule( {
  imports: [
    RouterModule.forRoot( appRoutes ),
  ],
  exports: [RouterModule]
} )
export class AppRoutingModule {
}
