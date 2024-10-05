import './App.scss'
import { Outlet } from 'react-router-dom'
import { ToastContainer } from 'react-toastify'
import { UserProvider } from '../../context/useAuth'
import Navbar from './navbar/Navbar'

function App() {

  return (
    <div className="">
      <UserProvider>
        <Navbar />
        <div className="">
          <Outlet />
        </div>
        <ToastContainer />
      </UserProvider>
    </div>
  )
}

export default App
