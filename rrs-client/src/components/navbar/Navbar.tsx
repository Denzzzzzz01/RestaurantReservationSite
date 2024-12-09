import { Link } from "react-router-dom";
import { useAuth } from "../../context/useAuth";
import { useEffect, useState } from "react";
import AddRestaurantModal from "../addRestaurantModal/AddRestaurantModal";
import { getUserRestaurant } from "../../services/RestaurantService";
import { jwtDecode } from "jwt-decode";

const Navbar = () => {
  const { isLoggedIn, user, logout, token } = useAuth();
  const [isAddRestaurantOpen, setIsAddRestaurantOpen] = useState(false);
  const [restaurant, setRestaurant] = useState<{ id: string; name: string } | null>(null);
  const [isManager, setIsManager] = useState(false);


  useEffect(() => {
    const checkRole = () => {
      if (!token) {
        setIsManager(false);
        return;
      }
  
      try {
        const decodedToken = jwtDecode<{ role?: string[] }>(token);
        const roles = decodedToken?.role || [];
        setIsManager(roles.includes("RestaurantManager"));
      } catch (error) {
        console.error("Error decoding token:", error);
        setIsManager(false);
      }
    };
  
    checkRole();
  }, [token]);
  
  useEffect(() => {
    const fetchRestaurant = async () => {
      if (!isLoggedIn() || !isManager) {
        setRestaurant(null);
        return;
      }
  
      try {
        const userRestaurant = await getUserRestaurant();
        setRestaurant(userRestaurant);
      } catch (error) {
        console.error("Failed to fetch restaurant information:", error);
        setRestaurant(null);
      }
    };
  
    fetchRestaurant();
  }, [isLoggedIn, isManager]);

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
              <Link to="/reservations" className="navbar-link">
                My Reservations
              </Link>
              {restaurant ? (
                <Link to={`/restaurants/${restaurant.id}`} className="navbar-link">
                  {restaurant.name}
                </Link>
              ) : (
                <button onClick={() => setIsAddRestaurantOpen(true)}>Add Restaurant</button>
              )}
              <span className="navbar-welcome">Welcome, {user?.username}</span>
              <button
                onClick={() => {
                  setRestaurant(null); 
                  logout();
                }}
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

      <AddRestaurantModal isOpen={isAddRestaurantOpen} onClose={() => setIsAddRestaurantOpen(false)} />
    </nav>
  );
};

export default Navbar;
