import IssueList from '../components/IssueList'
import ProjectSelector from '../components/ProjectSelector'
import Layout from '../components/Layout'
import { Grid } from '@mui/material'
import { Box } from '@mui/system'
import { Paper } from '@mui/material'

function IssuesPage() {
  return (
    <Layout>
      <Box m={1}>
        <Grid container spacing={1}>
          <Grid item xs={3} lg={2}>
            <Paper elevation={0} variant='outlined'>
              <Box m={1}>
                <ProjectSelector />
              </Box>
            </Paper>
          </Grid>
          <Grid item xs={9} lg={10}>
            <Paper elevation={0} variant='outlined'>
              <Box m={1}>
                <IssueList />
              </Box>
            </Paper>
          </Grid>
        </Grid>
      </Box>
    </Layout>
  )
}

export default IssuesPage
