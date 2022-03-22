import IssueList from '../components/IssueList'
import ProjectSelector from '../components/ProjectSelector'
import UserInfo from '../components/UserInfo'

function IssuesPage() {
  return (
      <>
        <UserInfo/>
        <ProjectSelector/>
        <IssueList/>
      </>
  )
}

export default IssuesPage