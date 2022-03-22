import { BrowserRouter as Router, Route, Routes } from 'react-router-dom'
import IssuesPage from '../pages/IssuesPage'
import ProjectPage from '../pages/ProjectPage'
import Footer from './Footer'
import Header from './Header'

function Application() {
    return (
        <>
            <Router>
                <Header/>
                <Routes>
                    <Route path='/' element={<IssuesPage/>} />
                    <Route path='/projects' element={<ProjectPage/>}/>
                </Routes>
                <Footer/>
            </Router>
        </>
    )
  }

export default Application