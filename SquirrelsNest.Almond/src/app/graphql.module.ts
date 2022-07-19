import {NgModule} from '@angular/core'
import {ApolloModule, APOLLO_OPTIONS} from 'apollo-angular'
import {ApolloClientOptions, InMemoryCache} from '@apollo/client/core'
import {HttpLink} from 'apollo-angular/http'

const uri = 'https://localhost:7274/api/' // <-- add the URL of the GraphQL server here

export function createApollo( httpLink: HttpLink ): ApolloClientOptions<any> {
  return {
    link: httpLink.create( { uri } ),
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
        ClProjectCollectionSegment: {
          fields: {
            items: {
              merge( existing = [], incoming: any[] ) {
                return [...existing, ...incoming]
              }
            }
          }
        }
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
