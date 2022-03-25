import { GraphQLClient, ClientContext } from 'graphql-hooks'
import { PropsWithChildren } from 'react';

const client = new GraphQLClient({
    url: 'https://localhost:7274/api'
});

function GraphQlContext( props: PropsWithChildren<{}>) {
    return(
        <ClientContext.Provider value={ client }>
            {props.children}
        </ClientContext.Provider>
    );
}

export default GraphQlContext