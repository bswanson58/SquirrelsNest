import IssueList from '../components/IssueList'
import ProjectSelector from '../components/ProjectSelector'
import Layout from '../components/Layout'
import SplitScreen from '../components/shared/SplitScreen'

function IssuesPage() {
  return (
    <Layout>
      <SplitScreen leftWeight={1} rightWeight={3}>
        <ProjectSelector/>
        <IssueList/>
      </SplitScreen>
    </Layout>
  )
}

export default IssuesPage
