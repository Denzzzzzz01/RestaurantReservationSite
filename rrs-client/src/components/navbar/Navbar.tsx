import { Link } from "react-router-dom";
import { useAuth } from "../../context/useAuth";
import { useState } from "react";
import AddRestaurantModal from "../addRestaurantModal/AddRestaurantModal";

const Navbar = () => {
  const { isLoggedIn, user, logout } = useAuth();
  const [isAddRestaurantOpen, setIsAddRestaurantOpen] = useState(false);

  return (
    <nav className="navbar">
      <div className="navbar-container">
        <div className="navbar-left">
          <Link to="/" className="navbar-logo">
            <h1>ProjectManagementSite</h1>
          </Link>
        </div>
        <div className="navbar-right">
          {isLoggedIn() ? (
            <>
              <button
                onClick={() => setIsAddRestaurantOpen(true)}
              >
                Add Restaurant
              </button>
              <span className="navbar-welcome">
                Welcome, {user?.username}
              </span>
              <button
                onClick={logout}
                className="navbar-btn navbar-logout-btn"
              >
                Logout
              </button>
            </>
          ) : (
            <>
              <Link to="/login" className="navbar-link">
                Login
              </Link>
              <Link to="/register" className="navbar-link">
                Register
              </Link>
            </>
          )}
        </div>
      </div>
      
      <AddRestaurantModal
        isOpen={isAddRestaurantOpen}
        onClose={() => setIsAddRestaurantOpen(false)}
      />
    </nav>
  );
};

export default Navbar;
