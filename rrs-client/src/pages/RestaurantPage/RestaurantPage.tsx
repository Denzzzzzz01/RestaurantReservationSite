import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { getRestaurantById } from "../../services/RestaurantService";
import { DetailedRestaurantDto } from "../../models/DetailedRestaurantDto";

const RestaurantPage = () => {
  const { id } = useParams<{ id: string }>();
  const [restaurant, setRestaurant] = useState<DetailedRestaurantDto | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchRestaurant = async () => {
      try {
        if (id) {
          const data = await getRestaurantById(id);
          setRestaurant(data);
        }
      } catch (error) {
        setError("Failed to fetch restaurant details");
      } finally {
        setLoading(false);
      }
    };

    fetchRestaurant();
  }, [id]);

  if (loading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>{error}</div>;
  }

  if (!restaurant) {
    return <div>Restaurant not found</div>;
  }

  return (
    <div>
      <h1>{restaurant.name}</h1>
      <p>Address: {restaurant.address.street}, {restaurant.address.city}, {restaurant.address.country}</p>
      <p>Phone: {restaurant.phoneNumber}</p>
      <p>Website: {restaurant.website || "N/A"}</p>
      <p>Opening Hours: {restaurant.openingHour} - {restaurant.closingHour}</p>
      <h3>Managers:</h3>
      <ul>
        {restaurant.manageres.map((manager) => (
          <li key={manager.appUser.id}>{manager.appUser.userName}</li>
        ))}
      </ul>
    </div>
  );
};

export default RestaurantPage;
