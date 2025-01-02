import { Link } from "react-router-dom";
import { RestaurantDto } from "../../models/RestaurantDto";
import { useState } from "react";
import './RestaurantCard.scss';

const RestaurantCard: React.FC<{
  restaurant: RestaurantDto;
  onBookTable: (id: string) => void;
}> = ({ restaurant, onBookTable }) => {
  const [isDescriptionExpanded, setDescriptionExpanded] = useState(false);

  const toggleDescription = () => {
    setDescriptionExpanded(!isDescriptionExpanded);
  };

  const formatTime = (time: string) => time.slice(0, -3);
  return (
    <li className="restaurant-card">
      <div className="restaurant-card-container">
        <img
          src={`${restaurant.logoUrl}`}
          alt={`${restaurant.name} Logo`}
          className="restaurant-logo"
        />
        <div className="restaurant-card-details">
          <Link to={`/restaurants/${restaurant.id}`} className="restaurant-name">
            {restaurant.name}
          </Link>

          <p className="restaurant-description">
            {isDescriptionExpanded
              ? restaurant.description
              : `${restaurant.description.slice(0, 240)}`}
            {restaurant.description.length > 240 && (
              <span className="toggle-description" onClick={toggleDescription}>
                {isDescriptionExpanded ? " Show less" : " ..."}
              </span>
            )}
          </p>
                      
          <div className="restaurant-footer">
            <p className="restaurant-address">
              {restaurant.address.street}, {restaurant.address.city},{" "}
              {restaurant.address.country}
            </p>

            <span className="restaurant-hours">
              OPENED {formatTime(restaurant.openingHour)} - {formatTime(restaurant.closingHour)}
            </span>

            <button
              className="btn-book"
              onClick={() => onBookTable(restaurant.id)}
            >
              Book a Table
            </button>
          </div>
        </div>
        
      </div>
    </li>
  );
};

export default RestaurantCard;