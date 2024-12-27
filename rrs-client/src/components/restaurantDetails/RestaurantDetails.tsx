import React from "react";
import { DetailedRestaurantDto } from "../../models/DetailedRestaurantDto";
import "./RestaurantDetails.scss"

interface RestaurantDetailsProps {
  restaurant: DetailedRestaurantDto;
}

const RestaurantDetails: React.FC<RestaurantDetailsProps> = ({ restaurant }) => {
  return (
    <div>
      <div className="restaurant-header">
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

        <h1>{restaurant.name}</h1>
      </div>
      
      <p> {restaurant.description} </p>
      <p>
        Address: {restaurant.address.street}, {restaurant.address.city},{" "}
        {restaurant.address.country}
      </p>
      <p>Phone: {restaurant.phoneNumber}</p>
      <p>Website: {restaurant.website || "N/A"}</p>
      <p>
        Opening Hours: {restaurant.openingHour} - {restaurant.closingHour}
      </p>
      <h3>Managers:</h3>
      <ul>
        {restaurant.manageres.map((manager) => (
          <li key={manager.appUser.id}>{manager.appUser.userName}</li>
        ))}
      </ul>
    </div>
  );
};

export default RestaurantDetails;
