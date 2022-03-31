import {
  Home as HomeIcon,
  BarChartOutlined as DashboardIcon,
  AccountBoxRounded as UserIcon,
  SettingsOutlined as SettingsIcon,
  ListAlt as ListIcon,
} from '@mui/icons-material'

import IssuesPage from '../pages/IssuesPage'
import ProjectPage from '../pages/ProjectPage'
import DefaultPage from '../pages/DefaultPage'
import RegisterPage from '../pages/RegisterPage'
import { AppRoute } from '../types/AppRoute'
import RegisterUser from '../components/RegisterUser'

const appRoutes: Array<AppRoute> = [
  {
    key: 'router-issues',
    title: 'Issues',
    description: 'Issues List',
    component: IssuesPage(),
    path: '/',
    roleClaim: 'user',
    isEnabled: true,
    icon: HomeIcon,
    appendDivider: true,
  },
  {
    key: 'router-projects',
    title: 'Projects',
    description: 'Project Configuration',
    component: ProjectPage(),
    path: '/projects',
    roleClaim: 'user',
    isEnabled: true,
    icon: DashboardIcon,
  },
  {
    key: 'router-register',
    title: 'Register',
    description: 'Register User',
    component: RegisterPage(),
    path: '/register',
    roleClaim: 'none',
    isEnabled: true,
    icon: DashboardIcon,
  },
  {
    key: 'router-my-account',
    title: 'My Account',
    description: 'My Account',
    path: '/account',
    roleClaim: 'user',
    isEnabled: true,
    icon: UserIcon,
    subRoutes: [
      {
        key: 'router-settings',
        title: 'Settings',
        description: 'Account Settings',
        component: DefaultPage(),
        roleClaim: 'user',
        path: '/account/settings',
        isEnabled: true,
        icon: SettingsIcon,
      },
      {
        key: 'router-preferences',
        title: 'Preferences',
        description: 'Account Preferences',
        component: DefaultPage(),
        path: '/account/preferences',
        roleClaim: 'user',
        isEnabled: true,
        icon: ListIcon,
      },
    ],
  },
]

export default appRoutes
