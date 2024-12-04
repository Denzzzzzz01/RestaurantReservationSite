import { Link } from "react-router-dom";
import { useAuth } from "../../context/useAuth";
import { useEffect, useState } from "react";
import AddRestaurantModal from "../addRestaurantModal/AddRestaurantModal";
import { getUserRestaurant } from "../../services/RestaurantService";

const Navbar = () => {
  const { isLoggedIn, user, logout, token } = useAuth();
  const [isAddRestaurantOpen, setIsAddRestaurantOpen] = useState(false);
  const [restaurant, setRestaurant] = useState<{ id: string; name: string } | null>(null);

  useEffect(() => {
    const fetchRestaurant = async () => {
      try {
        if (isLoggedIn()) {
          const userRestaurant = await getUserRestaurant();
          setRestaurant(userRestaurant);
        } else {
          setRestaurant(null);
        }
      } catch (error) {
        console.error("Failed to fetch restaurant information:", error);
        setRestaurant(null);
      }
    };

    fetchRestaurant();
  }, [isLoggedIn, token]); 

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
