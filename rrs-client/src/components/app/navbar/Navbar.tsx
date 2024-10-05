import { Link } from 'react-router-dom';
import { useAuth } from '../../../context/useAuth';

type Props = {}

const Navbar = (props: Props) => {
    const { isLoggedIn, user, logout } = useAuth();

    return (
      <nav className='bg-gray-200 p-4 flex-shrink-0'>
        <div className='flex justify-between items-center'>
          <div>
            <Link to="/"><h1>ProjectManagementSite</h1></Link>
          </div>
          {isLoggedIn() ? (
            <div className='flex space-x-4'>
              <div>Welcome, {user?.username}</div>
              <a
                onClick={logout}
                className='cursor-pointer'
              >
                Logout
              </a>
            </div>
          ) : (
            <div className='flex space-x-4'>
              <Link to="/login">
                Login
              </Link>
              <Link to="/register">
                Register
              </Link>
            </div>
          )}
        </div>
      </nav>
    );
}

export default Navbar;
