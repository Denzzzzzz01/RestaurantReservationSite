import { Link } from "react-router-dom";
import { useAuth } from "../../context/useAuth";
import { useState, useEffect } from "react";
import AddRestaurantModal from "../addRestaurantModal/AddRestaurantModal";
import { jwtDecode } from "jwt-decode";

const Navbar = () => {
  const { isLoggedIn, user, logout, token } = useAuth();
  const [isAddRestaurantOpen, setIsAddRestaurantOpen] = useState(false);
  const [isRestaurantManager, setIsRestaurantManager] = useState(false);

  useEffect(() => {
    if (token) {
      try {
        const decoded: { role?: string[] } = jwtDecode(token);
        const hasRole = decoded.role?.includes("RestaurantManager") || false;
        setIsRestaurantManager(hasRole);
      } catch (error) {
        console.error("Error decoding token:", error);
        setIsRestaurantManager(false);
      }
    } else {
      setIsRestaurantManager(false);
    }
  }, [token]);
  
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
              {isRestaurantManager ? (
                <button>MyRestaurant</button>
              ) : (
                <button onClick={() => setIsAddRestaurantOpen(true)}>
                  Add Restaurant
                </button>
              )}
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
