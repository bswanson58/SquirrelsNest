import { ClProject } from "./GraphQlEntities"

class ProjectList {
    mProjects: ClProject[]

    constructor( projects: ClProject[]) {
        this.mProjects = projects
    }
}

let noProjects = new ProjectList([])

export { ProjectList, noProjects }