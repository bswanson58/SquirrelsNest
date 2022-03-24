import IssueList from '../components/IssueList'
import ProjectSelector from '../components/ProjectSelector'
import UserInfo from '../components/UserInfo'
import Grid from '@mui/material/Grid'
import Stack from '@mui/material/Stack'
import Layout from '../components/Layout'

function IssuesPage() {
  return (
    <Layout>
      <Grid container spacing={2}>

        <Grid item sm={3} xs={12}>
          <ProjectSelector />
        </Grid>

        <Grid item sm={9} xs={12}>
          <IssueList />
        </Grid>

      </Grid>
    </Layout>
  )
}

export default IssuesPage
