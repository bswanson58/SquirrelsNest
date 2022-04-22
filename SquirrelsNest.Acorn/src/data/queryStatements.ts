import {gql} from 'graphql-request'

export const AllProjectsQuery = gql`
    query ProjectsQuery($skip: Int!, $take: Int!, $order:[ClProjectSortInput!], $where:ClProjectFilterInput) {
        projectList(
            skip: $skip, 
            take: $take, 
            order: $order, 
            where: $where ) {
            pageInfo {
                hasNextPage
                hasPreviousPage
            }
            items {
                id
                name
                description
                issuePrefix
            }
            totalCount
        }
    }`

export const IssuesQuery = gql`
    query IssuesForProjectQuery($skip: Int!, $take: Int!, $projectId: ID!, $order:[ClIssueSortInput!]) {
        issueList(
            skip: $skip
            take: $take
            projectId: $projectId
            order: $order ) {
            pageInfo {
                hasNextPage
                hasPreviousPage
            }
            items {
                id
                issueNumber
                title
                description
                isFinalized
                component {
                    name
                }
                assignedTo {
                    name
                }
                workflowState {
                    name
                }
                issueType {
                    name
                }
            }
            totalCount
        }
    }`
