import { Link } from "react-router-dom";
import { RestaurantDto } from "../../models/RestaurantDto";

const RestaurantCard: React.FC<{
    restaurant: RestaurantDto;
    onBookTable: (id: string) => void;
  }> = ({ restaurant, onBookTable }) => (
    <li>
      <div style={{ display: "flex", alignItems: "center", marginBottom: "1rem" }}>
        <img
          src={`${restaurant.logoUrl}`}
          alt={`${restaurant.name} Logo`}
          style={{
            width: "80px",
            height: "80px",
            borderRadius: "8px",
            objectFit: "cover",
            marginRight: "1rem",
          }}
        />
        <div>
          <Link to={`/restaurants/${restaurant.id}`} className="navbar-btn">
            {restaurant.name}
          </Link>

          <p>{restaurant.description}</p>
          
          <p>
            {restaurant.address.street}, {restaurant.address.city},{" "}
            {restaurant.address.country}
          </p>
          <p>
            Opening Hours: {restaurant.openingHour} - {restaurant.closingHour}
          </p>
          <button
            className="btn-book"
            onClick={() => onBookTable(restaurant.id)}
          >
            Book a Table
          </button>
        </div>
      </div>
    </li>
  );

  export default RestaurantCard;