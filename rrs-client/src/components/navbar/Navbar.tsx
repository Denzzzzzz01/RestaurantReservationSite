import { Link, useNavigate } from "react-router-dom";
import { useAuth } from "../../context/useAuth";
import { useEffect, useState } from "react";
import AddRestaurantModal from "../addRestaurantModal/AddRestaurantModal";
import { getUserRestaurant } from "../../services/RestaurantService";
import "./Navbar.scss";
import { jwtDecode } from "jwt-decode";

const Navbar = () => {
  const { isLoggedIn, user, logout, token } = useAuth();
  const [isAddRestaurantOpen, setIsAddRestaurantOpen] = useState(false);
  const [restaurant, setRestaurant] = useState<{ id: string; name: string } | null>(null);
  const [isManager, setIsManager] = useState(false);
  const navigate = useNavigate();


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


  const handleAddRestaurantClick = () => {
    if (!isLoggedIn()) {
      navigate("/login", { state: { from: location.pathname } });
      return;
    }

    setIsAddRestaurantOpen(true);
  };

  return (
    <nav className="navbar">
      <div className="navbar-container">
        <div className="navbar-left">
          <Link to="/" className="navbar-logo">
            <h1>RestaurantReservations</h1>
          </Link>
        </div>
        
        <div className="navbar-right">

          <Link to="/" className="navbar-btn">
              HOME
          </Link>

          {restaurant ? (
            <Link to={`/restaurants/${restaurant.id}`} className="navbar-btn">
              {restaurant.name}
            </Link>
          ) : (
            <button className="navbar-btn" onClick={handleAddRestaurantClick}>
              RESTAURANT
            </button>

          )}

          <Link to="/reservations" className="navbar-btn">
            RESERVATIONS
          </Link>

          <Link to="/" className="navbar-btn">
              MAIL
          </Link>

          {isLoggedIn() ? (
            <>
              <span className="navbar-btn">
                {user?.username}
              </span>

              <button
                onClick={() => {
                  setRestaurant(null);
                  logout();
                }}
                className="navbar-btn navbar-logout-btn"
              >
                LOGOUT
              </button>
            </>
          ) : (
            <>
              <Link to="/login" className="navbar-btn">
                LOGIN
              </Link>   
              <Link to="/register" className="navbar-btn">
                REGISTER
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
