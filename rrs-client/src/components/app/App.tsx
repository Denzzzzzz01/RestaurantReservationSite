import './App.scss'
import { Outlet } from 'react-router-dom'
import { ToastContainer } from 'react-toastify'
import { UserProvider } from '../../context/useAuth'
import Navbar from '../navbar/Navbar'
import { setupAxiosInterceptors } from '../../utils/axiosInterceptors'

function App() {
  setupAxiosInterceptors();

  return (
    <div className="app-container">
      <UserProvider>
        <Navbar />
        <div className="main-content">
          <Outlet />
        </div>
        <ToastContainer />
      </UserProvider>
    </div>
  )
}

export default App
