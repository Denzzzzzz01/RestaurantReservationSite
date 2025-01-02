import { RestaurantDto } from "../../models/RestaurantDto";
import RestaurantCard from "../restaurantCard/RestaurantCard";
import './RestaurantsList.scss';

const RestaurantsList: React.FC<{
    restaurants: RestaurantDto[];
    onBookTable: (id: string) => void;
  }> = ({ restaurants, onBookTable }) => (
    <ul>
      {restaurants.map((restaurant) => (
        <RestaurantCard
          key={restaurant.id}
          restaurant={restaurant}
          onBookTable={onBookTable}
        />
      ))}
    </ul>
  );

  export default RestaurantsList;