import { Route } from 'react-router-dom'
import { ComponentType, FC } from 'react';

/**
 * Represents the route of a page.
 */
export type AppRoute = {
  /**
   * The key of the route
   * @type {string}
   * @memberof Route
   * @required
   * @example
   * "dashboard"
   */
  key: string;

  /**
   * The title of the route
   * @type {string}
   * @memberof Route
   * @required
   * @example
   * "My Dashboard"
   */
  title: string;

  /**
   * The description of the route
   * @type {string}
   * @memberof Route
   * @required
   * @example
   * "Go to My Dashboard Page"
   */
  description?: string;

  /**
   * The path of the route
   * @type {string}
   * @memberof Route
   * @required
   * @example
   * "/dashboard"
   */
  path: string;

  /**
   * The component referenced by the route
   * @type {JSX.Element}
   * @memberof Route
   * @required
   * @example
   * "<Dashboard />"
   */
  component?: JSX.Element;

  /**
   * The status of the route
   * @type {boolean}
   * @memberof Route
   * @required
   * @example
   * true
   * @default
   * true
   */
  isEnabled: boolean;

  /**
   * The required role claim for the route
   * @type {string}
   * @memberof Route
   * @required
   * @example
   * 'user'
   * @default
   * ''
   */
  roleClaim: string

  /**
   * The icon that illustrates the route
   * @type {string}
   * @memberof Route
   * @optional
   * @example
   * DashboardIcon
   */
  icon?: ComponentType;

  /**
   * The array of sub routes
   * @type {Route[]}
   * @memberof Route
   * @optional
   * @example
   * "[{} as Route, ...]"
   */
  subRoutes?: AppRoute[];

  /**
   * The divider indicator for the route
   * @type {boolean}
   * @memberof Route
   * @optional
   * @example
   * true
   * @default
   * false
   */
  appendDivider?: boolean;

  /**
   * Indicate of menu item is expanded
   * @type {boolean}
   * @memberof Route
   * @optional
   * @example
   * true
   * @default
   * false
   */
  expanded?: boolean;
};
