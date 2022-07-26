import {NgModule} from '@angular/core'
import {setContext} from '@apollo/client/link/context'
import {ApolloModule, APOLLO_OPTIONS} from 'apollo-angular'
import {ApolloClientOptions, ApolloLink, InMemoryCache} from '@apollo/client/core'
import {HttpLink} from 'apollo-angular/http'
import {getAuthenticationToken} from './Auth/jwtSupport'

const uri = 'https://localhost:7274/api/' // <-- add the URL of the GraphQL server here

export function createApollo( httpLink: HttpLink ): ApolloClientOptions<any> {
  const basic = setContext( ( operation, context ) => ({
    headers: {
      Accept: 'charset=utf-8'
    }
  }) )

  const auth = setContext( ( operation, context ) => {
    const token = getAuthenticationToken()

    if( token === null ) {
      return {}
    }
    else {
      return {
        headers: {
          Authorization: `bearer ${token}`
        }
      }
    }
  } )

  return {
    link: ApolloLink.from( [basic, auth, httpLink.create( { uri } )] ),
    cache: new InMemoryCache( {
      typePolicies: {
        Query: {
          fields: {
            projectList: {
              keyArgs: false
            },
            issueList: {
              keyArgs: ['$projectId']
            }
          }
        },
      }
    } )
  }
}

@NgModule( {
  exports: [ApolloModule],
  providers: [
    {
      provide: APOLLO_OPTIONS,
      useFactory: createApollo,
      deps: [HttpLink],
    },
  ],
} )
export class GraphQLModule {
}
